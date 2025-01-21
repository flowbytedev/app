//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.ComponentModel.DataAnnotations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Application.Shared.Models.Data;


///// <summary>
///// Supported data types for our dataset columns.
///// This is a simplified version—Power BI has many more types.
///// </summary>
//public enum ColumnDataType
//{
//    Integer,
//    Decimal,
//    String,
//    DateTime,
//    Boolean
//}


///// <summary>
///// Defines a single column's metadata: name, data type, and nullability.
///// </summary>
//public class ColumnDefinition
//{
//    public string ColumnName { get; set; }
//    public ColumnDataType DataType { get; set; }
//    public bool IsNullable { get; set; }

//    public ColumnDefinition(string columnName, ColumnDataType dataType, bool isNullable = true)
//    {
//        ColumnName = columnName;
//        DataType = dataType;
//        IsNullable = isNullable;
//    }
//}


///// <summary>
///// A single table in our dataset, storing column definitions and row data.
///// </summary>
//public class Table
//{
//    /// <summary>
//    /// Table Schema
//    /// </summary>
//    /// 
//    public string Schema { get; set; }

//    /// <summary>
//    /// Table name.
//    /// </summary>
//    public string Name { get; set; }

//    /// <summary>
//    /// Collection of column definitions.
//    /// </summary>
//    public List<ColumnDefinition> Columns { get; set; }

//    /// <summary>
//    /// The actual rows stored as a list of dictionaries: { columnName : value }.
//    /// </summary>
//    public List<Dictionary<string, object>> Rows { get; set; }

//    public Table(string name)
//    {

//        Name = name;
//        Columns = new List<ColumnDefinition>();
//        Rows = new List<Dictionary<string, object>>();
//    }

//    /// <summary>
//    /// Add a new column definition to the table.
//    /// </summary>
//    public void AddColumn(string columnName, ColumnDataType dataType, bool isNullable = true)
//    {
//        // Ensure the column doesn't already exist
//        if (Columns.Any(c => c.ColumnName.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
//        {
//            throw new InvalidOperationException($"Column '{columnName}' already exists in table '{Name}'.");
//        }

//        Columns.Add(new ColumnDefinition(columnName, dataType, isNullable));
//    }

//    /// <summary>
//    /// Insert a new row into this table.
//    /// </summary>
//    public void AddRow(Dictionary<string, object> rowValues)
//    {
//        // Validate that the passed columns exist and data types match
//        var newRow = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

//        foreach (var colDef in Columns)
//        {
//            if (rowValues.ContainsKey(colDef.ColumnName))
//            {
//                object value = rowValues[colDef.ColumnName];

//                // Optionally validate data types here (simplified)
//                newRow[colDef.ColumnName] = value;
//            }
//            else
//            {
//                // If not provided, set to null or default value
//                newRow[colDef.ColumnName] = null;
//            }
//        }

//        Rows.Add(newRow);
//    }

//    /// <summary>
//    /// Returns all rows that match the given predicate (or all if predicate is null).
//    /// </summary>
//    public List<Dictionary<string, object>> SelectRows(Func<Dictionary<string, object>, bool> predicate = null)
//    {
//        if (predicate == null)
//        {
//            return Rows.ToList();
//        }

//        return Rows.Where(predicate).ToList();
//    }

//    /// <summary>
//    /// Update rows that match the predicate with the new values provided.
//    /// </summary>
//    public void UpdateRows(Func<Dictionary<string, object>, bool> predicate, Dictionary<string, object> updatedValues)
//    {
//        var matchingRows = Rows.Where(predicate).ToList();
//        foreach (var row in matchingRows)
//        {
//            foreach (var colDef in Columns)
//            {
//                if (updatedValues.ContainsKey(colDef.ColumnName))
//                {
//                    row[colDef.ColumnName] = updatedValues[colDef.ColumnName];
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// Delete rows that match the predicate.
//    /// </summary>
//    public void DeleteRows(Func<Dictionary<string, object>, bool> predicate)
//    {
//        Rows.RemoveAll(r => predicate(r));
//    }
//}


///// <summary>
///// Relationship cardinalities (simplified).
///// </summary>
//public enum RelationshipCardinality
//{
//    OneToOne,
//    OneToMany,
//    ManyToOne,
//    ManyToMany
//}


///// <summary>
///// Defines a relationship between two tables.
///// For instance, Orders(From) -> Customers(To) on CustomerId.
///// </summary>
//public class Relationship
//{
//    public string Name { get; set; }
//    public string FromTable { get; set; }
//    public string FromColumn { get; set; }
//    public string ToTable { get; set; }
//    public string ToColumn { get; set; }
//    public RelationshipCardinality Cardinality { get; set; }

//    public Relationship(string name,
//                        string fromTable,
//                        string fromColumn,
//                        string toTable,
//                        string toColumn,
//                        RelationshipCardinality cardinality)
//    {
//        Name = name;
//        FromTable = fromTable;
//        FromColumn = fromColumn;
//        ToTable = toTable;
//        ToColumn = toColumn;
//        Cardinality = cardinality;
//    }
//}


///// <summary>
///// A Power-BI–like dataset class with tables, relationships, measures, and basic data manipulation features.
///// </summary>
//public class PowerBiLikeDataSet
//{
//    /// <summary>
//    /// Dataset name.
//    /// </summary>
//    public string Name { get; set; }

//    /// <summary>
//    /// All tables in this dataset.
//    /// </summary>
//    public List<Table> Tables { get; set; }

//    /// <summary>
//    /// Relationships between tables.
//    /// </summary>
//    public List<Relationship> Relationships { get; set; }

//    /// <summary>
//    /// Simple dictionary of measureName -> measureExpression (e.g. DAX-like expressions).
//    /// In real usage, you'd parse and evaluate these expressions.
//    /// </summary>
//    public Dictionary<string, string> Measures { get; set; }

//    public PowerBiLikeDataSet(string name)
//    {
//        Name = name;
//        Tables = new List<Table>();
//        Relationships = new List<Relationship>();
//        Measures = new Dictionary<string, string>();
//    }

//    /// <summary>
//    /// Create a new table and add it to the dataset.
//    /// </summary>
//    public Table CreateTable(string tableName)
//    {
//        if (Tables.Any(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase)))
//        {
//            throw new InvalidOperationException($"A table named '{tableName}' already exists in dataset '{Name}'.");
//        }

//        var table = new Table(tableName);
//        Tables.Add(table);
//        return table;
//    }

//    /// <summary>
//    /// Adds a measure (calculated field) to the dataset.
//    /// For example: measureName = "TotalSales", measureExpression = "SUM(Orders[SalesAmount])"
//    /// </summary>
//    public void AddMeasure(string measureName, string measureExpression)
//    {
//        if (Measures.ContainsKey(measureName))
//        {
//            throw new InvalidOperationException($"A measure named '{measureName}' already exists in dataset '{Name}'.");
//        }

//        // In a real scenario, you’d parse/validate the measure expression.
//        Measures[measureName] = measureExpression;
//    }

//    /// <summary>
//    /// Adds a relationship between two tables in the dataset.
//    /// </summary>
//    public void AddRelationship(string relationshipName,
//                                string fromTable,
//                                string fromColumn,
//                                string toTable,
//                                string toColumn,
//                                RelationshipCardinality cardinality)
//    {
//        // Basic validation
//        if (!Tables.Any(t => t.Name.Equals(fromTable, StringComparison.OrdinalIgnoreCase)) ||
//            !Tables.Any(t => t.Name.Equals(toTable, StringComparison.OrdinalIgnoreCase)))
//        {
//            throw new InvalidOperationException("One or both of the specified tables do not exist in the dataset.");
//        }

//        if (Relationships.Any(r => r.Name.Equals(relationshipName, StringComparison.OrdinalIgnoreCase)))
//        {
//            throw new InvalidOperationException($"A relationship named '{relationshipName}' already exists.");
//        }

//        var relationship = new Relationship(relationshipName, fromTable, fromColumn, toTable, toColumn, cardinality);
//        Relationships.Add(relationship);
//    }

//    /// <summary>
//    /// Retrieve reference to a table by name.
//    /// </summary>
//    public Table GetTable(string tableName)
//    {
//        var table = Tables.FirstOrDefault(t => t.Name.Equals(tableName, StringComparison.OrdinalIgnoreCase));
//        if (table == null)
//        {
//            throw new InvalidOperationException($"Table '{tableName}' does not exist in dataset '{Name}'.");
//        }
//        return table;
//    }

//    /// <summary>
//    /// Perform a simple inner join between two tables based on a relationship.
//    /// Returns a new list of rows with combined columns from both tables.
//    /// </summary>
//    public List<Dictionary<string, object>> InnerJoin(string relationshipName)
//    {
//        var relationship = Relationships.FirstOrDefault(r => r.Name.Equals(relationshipName, StringComparison.OrdinalIgnoreCase));
//        if (relationship == null)
//        {
//            throw new InvalidOperationException($"Relationship '{relationshipName}' not found.");
//        }

//        Table fromTable = GetTable(relationship.FromTable);
//        Table toTable = GetTable(relationship.ToTable);

//        var resultRows = new List<Dictionary<string, object>>();

//        foreach (var fromRow in fromTable.Rows)
//        {
//            object fromKeyValue = fromRow[relationship.FromColumn];

//            // Match with all toRows that share the same key
//            var matchedRows = toTable.Rows
//                .Where(toRow =>
//                    toRow[relationship.ToColumn] != null &&
//                    toRow[relationship.ToColumn].Equals(fromKeyValue))
//                .ToList();

//            foreach (var toRow in matchedRows)
//            {
//                // Combine columns
//                var combined = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

//                // Add fromTable columns
//                foreach (var col in fromTable.Columns)
//                {
//                    combined[$"{fromTable.Name}.{col.ColumnName}"] = fromRow[col.ColumnName];
//                }

//                // Add toTable columns
//                foreach (var col in toTable.Columns)
//                {
//                    combined[$"{toTable.Name}.{col.ColumnName}"] = toRow[col.ColumnName];
//                }

//                resultRows.Add(combined);
//            }
//        }

//        return resultRows;
//    }

//    /// <summary>
//    /// Perform a simple left join between two tables based on a relationship.
//    /// Rows from the 'FromTable' will appear even if there's no match in the 'ToTable'.
//    /// </summary>
//    public List<Dictionary<string, object>> LeftJoin(string relationshipName)
//    {
//        var relationship = Relationships.FirstOrDefault(r => r.Name.Equals(relationshipName, StringComparison.OrdinalIgnoreCase));
//        if (relationship == null)
//        {
//            throw new InvalidOperationException($"Relationship '{relationshipName}' not found.");
//        }

//        Table fromTable = GetTable(relationship.FromTable);
//        Table toTable = GetTable(relationship.ToTable);

//        var resultRows = new List<Dictionary<string, object>>();

//        foreach (var fromRow in fromTable.Rows)
//        {
//            object fromKeyValue = fromRow[relationship.FromColumn];

//            // Match with all toRows that share the same key
//            var matchedRows = toTable.Rows
//                .Where(toRow =>
//                    toRow[relationship.ToColumn] != null &&
//                    toRow[relationship.ToColumn].Equals(fromKeyValue))
//                .ToList();

//            if (matchedRows.Count > 0)
//            {
//                // If matches exist, add combined rows
//                foreach (var toRow in matchedRows)
//                {
//                    var combined = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
//                    foreach (var col in fromTable.Columns)
//                    {
//                        combined[$"{fromTable.Name}.{col.ColumnName}"] = fromRow[col.ColumnName];
//                    }
//                    foreach (var col in toTable.Columns)
//                    {
//                        combined[$"{toTable.Name}.{col.ColumnName}"] = toRow[col.ColumnName];
//                    }
//                    resultRows.Add(combined);
//                }
//            }
//            else
//            {
//                // If no match, add fromRow with null for toTable columns
//                var combined = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
//                foreach (var col in fromTable.Columns)
//                {
//                    combined[$"{fromTable.Name}.{col.ColumnName}"] = fromRow[col.ColumnName];
//                }
//                foreach (var col in toTable.Columns)
//                {
//                    combined[$"{toTable.Name}.{col.ColumnName}"] = null;
//                }
//                resultRows.Add(combined);
//            }
//        }

//        return resultRows;
//    }
//}