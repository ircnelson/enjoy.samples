using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace EnjoySample.Restaurant.Read.SQLite.Infrastructure
{
    public class SqlCreateBuilder
    {
        private readonly IDictionary<Type, string> _columnTypes;

        public SqlCreateBuilder()
        {
            _columnTypes = new Dictionary<Type, string>
            {
                {typeof (bool), "bool"},
                {typeof (Int32), "int"},
                {typeof (string), "nvarchar(250)"},
                {typeof (double), "numeric"},
                {typeof (decimal), "numeric"},
                {typeof (float), "numeric"},
                {typeof (Guid), "uniqueidentifier"},
            };
        }

        public string CreateSqlCreateStatementFromDto(Type dtoType)
        {
            var tableName = dtoType.Name;

            return $"CREATE TABLE {tableName} ({GetColumns(dtoType)});";
        }

        private string GetColumns(Type dtoType)
        {
            var properties = dtoType.GetProperties();

            return string.Join(",", properties
                .Where(Where)
                .Select(GetColumn).ToArray());
        }

        private static bool Where(PropertyInfo propertyInfo)
        {
            return !propertyInfo.PropertyType.IsGenericType;
        }

        private string GetColumn(PropertyInfo propertyInfo)
        {
            return propertyInfo.Name == "Id"
                ? $"[{propertyInfo.Name}] [{GetColumnType(propertyInfo)}] primary key"
                : propertyInfo.Name.EndsWith("Id")
                    ? $"[{propertyInfo.Name}] [{GetColumnType(propertyInfo)}] foreing key"
                    : $"[{propertyInfo.Name}] [{GetColumnType(propertyInfo)}]";
        }

        private string GetColumnType(PropertyInfo propertyInfo)
        {
            var type = propertyInfo.PropertyType;
            if (!_columnTypes.ContainsKey(type))
                throw new Exception($"The key {type.Name} was not found!");

            return _columnTypes[type];
        }
    }
}