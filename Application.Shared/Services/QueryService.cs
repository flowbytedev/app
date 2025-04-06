using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;

namespace Application.Shared.Services;


public enum ComparisonType
{
    Equal,
    LessThan,
    GreaterThan
}

//public class FilterCondition
//{
//    public string PropertyName { get; set; }
//    public object? Value { get; set; }
//    public string Comparison { get; set; } = "eq";
//}



[NotMapped]
public class QueryService<T>
{
    public T? Filter { get; set; } 

    public IQueryable<T> ApplyFilters(IQueryable<T> query)
    {
        var itemType = typeof(T);
        var parameter = Expression.Parameter(itemType, "i");


        foreach (var property in itemType.GetProperties())
        {

            var propertyType = property.PropertyType;

            
            var propertyValue = property.GetValue(this.Filter);

            // Handle nullable boolean properties separately
            if ((propertyType == typeof(bool) || propertyType == typeof(bool?))  && propertyValue != null)
            {
                propertyValue = (bool?)propertyValue;
                var equals = Expression.Equal(Expression.Property(parameter, property.Name), Expression.Constant(propertyValue));
                var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                query = query.Where(lambda);

            }

            // if property is datetime
            //else if ((propertyType == typeof(DateTime) || propertyType != typeof(DateTime?)) && propertyValue != null)
            //{
            //    //var equals = Expression.Equal(Expression.Property(parameter, property.Name), Expression.Constant(propertyValue));
            //    //var lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
            //    //query = query.Where(lambda);
            //    continue;
            //}

            else if ((propertyType != typeof(bool) && propertyType != typeof(bool?) && propertyType != typeof(DateTime) && propertyType != typeof(DateTime?)) && propertyValue != null)
            {
                
                var left = Expression.Property(parameter, property.Name);

                var right = Expression.Constant(propertyValue);


                Expression<Func<T, bool>>? lambda;

                // if the property name is cdc_key, we want to get all that are greater than the value
                if (property.Name == "CdcKey")
                {
                    // var greaterThan = Expression.GreaterThan(left, right);
                    // lambda = Expression.Lambda<Func<T, bool>>(greaterThan, parameter);
                    // query = query.Where(lambda);

                    // CDC Key filter are treated in the controller methods
                    continue;
                }
                else {
                    var equals = Expression.Equal(left, right);
                    lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                    query = query.Where(lambda);
                }

                // var equals = Expression.Equal(left, right);
                // lambda = Expression.Lambda<Func<T, bool>>(equals, parameter);
                // query = query.Where(lambda);
                
                
            }
        }

        return query;
    }



    public IQueryable<T> ApplyOrdering<T>(IQueryable<T> query, string orderBy, bool descending = false)
    {
        if (string.IsNullOrEmpty(orderBy))
            return query;  // Return original query if no order specified

        var itemType = typeof(T);
        var property = itemType.GetProperty(orderBy);
        
        if (property == null)
            throw new ArgumentException($"Property '{orderBy}' does not exist on type '{itemType.Name}'");

        var parameter = Expression.Parameter(itemType, "i");
        var propertyAccess = Expression.Property(parameter, property);

        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        var methodName = descending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            new Type[] { itemType, property.PropertyType },
            query.Expression,
            Expression.Quote(orderByExpression)
        );

        return query.Provider.CreateQuery<T>(resultExpression);
    } 
    

}