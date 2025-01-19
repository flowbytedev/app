import pandas as pd
from upath import UPath
import os
import pyarrow as pa
import pyarrow.parquet as pq
from typing import Optional
from dagster import (
    OutputContext,
    UPathIOManager,
    Output,
    MetadataValue,
    InputContext,
    Config
)


from flowbyte_app.modules import log



class PandasParquetIOManager(UPathIOManager):
    extension: str = ".parquet"
    

    def __init__(self, base_path=None, **kwargs):
        # Use "xyz" as the default base path if none is provided
        super().__init__(base_path=base_path or UPath(os.getenv("STORAGE_PATH")), **kwargs)

    @property
    def base_path(self):
        """Override base_path to use the instance's _base_path."""
        return self._base_path
    

    # def create_folder(self, folder_path, folder_name):
    #     if not os.path.isdir(folder_path + folder_name):
    #         new_path = folder_path + folder_name
    #         os.makedirs(new_path)
        

    def dump_to_path(self, context: OutputContext, obj: object, path: UPath):
        
       
        
        storage_path = os.getenv('STORAGE_PATH')
        
        # self.create_folder(storage_path, context.asset_key.path[0])
       
        # path = UPath(storage_path)
        if context.has_partition_key:
            path =  UPath(str(storage_path) + '/' +str(context.asset_key.path[0]) + '/' + str(context.asset_partition_key)  + ".parquet")
            
        else:
            path = UPath(str(storage_path) + '/' + str(context.asset_key.path[0])  + ".parquet")
        

        # log.log_info(f"dump_to_path: {path}")
        
        
       
        if isinstance(obj, pd.DataFrame):
            with path.open("wb") as file:
                obj.to_parquet(file, compression="snappy")
            preview = MetadataValue.md(obj.head(100).to_markdown())
            rows, columns = obj.shape

        elif isinstance(obj, pa.Table):
            with path.open("wb") as file:
                pq.write_table(obj, file)
            preview = None
            rows, columns = obj.num_rows, obj.num_columns

        elif isinstance(obj, str):
            preview = MetadataValue.md(obj)
            rows, columns = 1, 1

        elif isinstance(obj, int):
            preview = f"Inserted {obj} records"
            rows, columns = obj, None

        else:
            preview = ""
            rows, columns = 0, 0

        metadata = {
            "num_rows": rows,
            "column_count": columns,
            "preview": preview,
        }

        if context.metadata:
            metadata.update(context.metadata)

        context.add_output_metadata(metadata)

        return obj  




    def load_from_path(self, context: InputContext, path: UPath) -> pd.DataFrame:
    
        # 1. Identify base path (no extension yet)
        storage_path = os.getenv('STORAGE_PATH')
        if context.has_partition_key:
            # e.g. /my_storage/some_asset/partition_key
            base_path = UPath(os.path.join(storage_path, context.asset_key.path[0], context.asset_partition_key))
        else:
            # e.g. /my_storage/some_asset
            base_path = UPath(os.path.join(storage_path, context.asset_key.path[0]))

        
        # log.log_info(f"load_from_path: {base_path}")

        # 2. Define recognized extensions and how to read them
        EXTENSION_READERS = {
            ".parquet": pd.read_parquet,
            ".csv": pd.read_csv,
            ".txt": pd.read_csv,   # often .txt can be read as CSV with default settings
            "": pd.read_pickle,    # no extension => assume pickle
        }

        # 3. Try each extension in turn
        for ext, read_func in EXTENSION_READERS.items():
            candidate_path = base_path.with_suffix(ext)  # e.g. base_path + ".parquet"
            if candidate_path.exists():
                # 4. Once we find a match, open and use the corresponding reader
                try:
                    with candidate_path.open("rb") as file:
                        return read_func(file)
                except Exception as e:
                    log.log_info("No File Found")

        # If none of the candidate paths exist, handle gracefully
        # raise FileNotFoundError(f"No file found at {base_path} with any recognized extension: {list(EXTENSION_READERS.keys())}")



                    
            



class DateRange(Config):
    start_date: Optional[str] = None
    end_date: Optional[str] = None