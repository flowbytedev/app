using Application.Data;
using Application.Shared.Models;
using Application.Shared.Models.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class DatabaseService
{
    private readonly ApplicationDbContext _appContext;

    public DatabaseService(ApplicationDbContext appContext)
    {
        _appContext = appContext;
    }


    public async Task<List<Database>> GetDatabasesAsync()
    {
        var databases = await _appContext.Database.ToListAsync();
        return databases;
    }

    public async Task<string> GetConnectionString(Database database)
    {
        // generate connection string base on the host and database and databaseCredential
        // TODO: Add user selected credential, instead of using the first one
        var credential = await _appContext.DatabaseCredential.FirstOrDefaultAsync(x => x.Host == database.Host && x.DatabaseName == database.Name);


        if (credential == null)
        {
            throw new Exception("Database credential not found");
        }

        var connectionStringBuilder = new SqlConnectionStringBuilder
        {
            DataSource = database.Host,
            InitialCatalog = database.Name,
            IntegratedSecurity = false,
            UserID = credential.Username,
            Password = credential.Password
        };

        connectionStringBuilder.TrustServerCertificate = true;
        connectionStringBuilder.Encrypt = true;
        return connectionStringBuilder.ConnectionString;
    }


    public async Task<List<Table>> GetAllTablesAsync(Database database)
    {
        // Ensure the database name is set in the connection string
        var connectionString = await GetConnectionString(database);

        var tables = new List<Table>();

        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();

                string query = @"
                SELECT TABLE_SCHEMA, TABLE_NAME
                FROM INFORMATION_SCHEMA.TABLES
                WHERE TABLE_TYPE = 'BASE TABLE'";

                using (var command = new SqlCommand(query, sqlConnection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            tables.Add(new Table
                            {
                                Schema = reader["TABLE_SCHEMA"].ToString(),
                                Name = reader["TABLE_NAME"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log or rethrow as needed)
            Console.WriteLine($"Error fetching tables: {ex.Message}");
            throw;
        }

        return tables;
    }


    public async Task<List<string>> GetColumnsForTableAsync(Database database, string tableName)
    {
        var columns = new List<string>();
        var connectionString = await GetConnectionString(database);
        try
        {
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                await sqlConnection.OpenAsync();
                string query = @"
                SELECT COLUMN_NAME
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_NAME = @TableName";
                using (var command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@TableName", tableName);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            columns.Add(reader["COLUMN_NAME"].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception (log or rethrow as needed)
            Console.WriteLine($"Error fetching columns: {ex.Message}");
            throw;
        }
        return columns;

    }



    // Retrieve the primary key column(s) for a given table
    public async Task<List<string>> GetPrimaryKeyColumns(string tableName, string? connectionString)
    {
        var primaryKeys = new List<string?>();


        // get the primary keys from the table
        string query = @"
            SELECT COLUMN_NAME 
            FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE 
            WHERE OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + CONSTRAINT_NAME), 'IsPrimaryKey') = 1 
            AND TABLE_NAME = @TableName";


        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();

            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@TableName", tableName);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        primaryKeys.Add(reader["COLUMN_NAME"].ToString());
                    }
                }
            }
        }

        return primaryKeys;
    }




    // Retrieve the data for a given table
    public async Task<List<Dictionary<string, object>>> GetTableDataAsync(string schema, string tableName, string filterClause = "", int pageNumber = 1, int pageSize = 100, Database? database = null)
    {
        var result = new List<Dictionary<string, object>>();
        string connectionString = string.Empty;

        if (database is not null)
        {
            connectionString = await GetConnectionString(database);
            //return await GetTableDataAsync(query, connectionString);
        }
        else
        {
            // return empty list
            return result;
        }

        // calculate offset
        var offset = pageNumber < 0 ? 0 : pageSize * pageNumber; // (pageNumber) * pageSize;

        //Retrieve primary key columns
        var primaryKeyColumns = await GetPrimaryKeyColumns(tableName, connectionString);

        // Determine the ORDER BY clause
        string orderByClause = primaryKeyColumns.Any() ? string.Join(", ", primaryKeyColumns) : "(SELECT NULL)";

        string query = "";

        if(!String.IsNullOrEmpty(filterClause)) {
            query = $"SELECT * FROM {schema}.[{tableName}] WHERE {filterClause} ORDER BY {orderByClause} OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        }
        else
        {
            query = $"SELECT * FROM {schema}.[{tableName}] ORDER BY {orderByClause} OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
        }


        using var connection = new SqlConnection(connectionString);
        using var command = new SqlCommand(query, connection);

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        // Get the column schema dynamically
        var columnSchema = reader.GetColumnSchema();

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object>();

            // Loop through the columns and add the values to the dictionary
            foreach (var column in columnSchema)
            {
                var columnName = column.ColumnName;
                row[columnName] = reader[columnName] is DBNull ? null : reader[columnName];
            }

            result.Add(row);
        }

        return result;
    }



    // Retrieve the total number of rows in a table
    public async Task<int> GetTotalRowCountAsync(Database database, string tableName)
    {
        int totalRows = 0;

        var connectionString = await GetConnectionString(database);

        using (var connection = new SqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand($"SELECT COUNT(*) FROM {tableName}", connection))
            {
                totalRows = (int)(await command.ExecuteScalarAsync());
            }
        }

        return totalRows;
    }

}

