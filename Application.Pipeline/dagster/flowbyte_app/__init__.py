
from dagster import (
    Definitions,
    define_asset_job,
    load_assets_from_modules,
    
)

from flowbyte_app.assets import integration
from flowbyte_app.modules import log, models




integration_assets = load_assets_from_modules([integration])




## INTEGRATION JOBS ##
integration_job = define_asset_job(name="item_details", selection=["get_field_mapping", "get_source_data", "transform_data", "add_destination_data"])



defs = Definitions(
    assets=[*integration_assets],
    jobs=[  
            integration_job,
        ],
    schedules=[ ],
    sensors=[ ],
    resources={
        "parquet_io_manager": models.PandasParquetIOManager(),
    }
)


