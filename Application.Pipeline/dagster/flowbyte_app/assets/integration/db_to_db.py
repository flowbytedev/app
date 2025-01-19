import os
import pandas as pd
from dagster import MetadataValue, Output, asset, StaticPartitionsDefinition
from flowbyte.sql import MSSQL
from flowbyte_app.modules import sql, log

env = os.getenv('ENV')

integration_tables_text = os.getenv(f'INTEGRATION_TABLES')
integration_tables_list = integration_tables_text.split(',')
integration_tables_partitions = StaticPartitionsDefinition(integration_tables_list)


server, database, username, password = sql.get_connection_details("DESTINATION")
sql_flowbyte = MSSQL(
    host=server,
    username=username,
    password=password,
    database=database,
    driver="ODBC Driver 17 for SQL Server",
    connection_type="pyodbc"

    )

server, database, username, password = sql.get_connection_details("SOURCE")
sql_source = MSSQL(
    host=server,
    username=username,
    password=password,
    database=database,
    driver="ODBC Driver 17 for SQL Server",
    connection_type="pyodbc"

    )




@asset(owners=["kevork.keheian@flowbyte.dev", "team:data-eng"], compute_kind="sql", group_name="config", io_manager_key="parquet_io_manager", partitions_def=integration_tables_partitions)
def get_field_mapping(context):
    """
    Get Field Mappings
    """

    # Get table_name for partition
    table_name = context.partition_key


    sql_flowbyte.connect()

    query = f"""SELECT
                        source_column,
                        destination_column,
                        filter_query

            FROM [dbo].[field_mapping]

            WHERE [destination_table] = '{table_name}'
            """

    if env == "DEV":
        log.log_debug(f"DEBUG: {query}")

    df = sql_flowbyte.get_data(query, chunksize=1000)

    if env == "DEV":
        log.log_info(df)

    metadata = {
        "row_sql": MetadataValue.md("```SQL\n" + query + "\n```")
    }

    return Output(value=df, metadata=metadata)


@asset(owners=["kevork.keheian@flowbyte.dev", "team:data-eng"], compute_kind="sql", group_name="extract", io_manager_key="parquet_io_manager", partitions_def=integration_tables_partitions)
def get_source_data(context, get_field_mapping):
    """
    Get Store from SOURCE
    """

    # Get table_name for partition
    table_name = context.partition_key

    sql_source.connect()

    # get list of source columns from field mapping
    field_mapping = get_field_mapping['source_column'].tolist()
    
    # [col1], [col2], [col3]
    field_mapping = ', '.join([f'[{x}]' for x in field_mapping])
    
    # check if there are any filters
    if get_field_mapping['filter_query'].isnull().all():
        filters = ''
    else:
        field_mapping = ', '.join(field_mapping)

        filters = get_field_mapping['filter_query'].tolist()
        filters = ' AND '.join(filters)
        filters = f"WHERE {filters}"
        

    # get the first row of the field mapping to use as the table name
    # table_name = get_field_mapping['source_table'].iloc[0]

    query = f"""SELECT
                        {field_mapping}

            FROM {table_name}

            {filters}
            """
    
    if env == "DEV":
        log.log_info(query)

    df = sql_source.get_data(query, chunksize=1000)

    metadata = {
        "row_sql": MetadataValue.md("```SQL\n" + query + "\n```")
    }

    return Output(value=df, metadata=metadata)


@asset(owners=["kevork.keheian@flowbyte.dev", "team:data-eng"], compute_kind="sql", group_name="transform", io_manager_key="parquet_io_manager", partitions_def=integration_tables_partitions)
def transform_data(context, get_field_mapping, get_source_data):
    """
    Transform Stores
    """

    # Get table_name for partition
    table_name = context.partition_key

    field_mapping = get_field_mapping
    df = get_source_data
    

    # rename columns
    columns = field_mapping['source_column'].tolist()

    for i in range(len(columns)):
        df.rename(columns={columns[i]: field_mapping['destination_column'].iloc[i]}, inplace=True)

    metadata = {
        "columns": MetadataValue.md(", ".join(df.columns))
    }

    return Output(value=df, metadata=metadata)


@asset(owners=["kevork.keheian@flowbyte.dev", "team:data-eng"], compute_kind="api", group_name="load", io_manager_key="parquet_io_manager", partitions_def=integration_tables_partitions)
def add_destination_data(context, transform_data):
    """
    Add Stores to Destination
    """
    # Get table_name for partition
    table_name = context.partition_key


    sql_flowbyte.connect()

    df = transform_data
    
    # get list of destination columns from field mapping
    columns = df.columns.tolist()

    schema = "dbo"
    
    sql_flowbyte.insert_data(schema=schema, table_name=table_name, insert_records=df, chunksize=10000)


    metadata = {
        "columns": MetadataValue.md(", ".join(columns)),
        "row_count": MetadataValue.md(str(len(df))),
        "columns_count": MetadataValue.md(str(len(columns)))
    }

    return Output(value=None, metadata=metadata)




