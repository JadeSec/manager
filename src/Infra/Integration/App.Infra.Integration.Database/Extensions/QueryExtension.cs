using SqlKata;
using System.Linq;
using SqlKata.Execution;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;

namespace App.Infra.Integration.Database.Extensions
{
    public static class QueryExtension
    {
        public static QueryFactory Log(this QueryFactory factory)
        {
            factory.Logger = compiled => {
                Console.WriteLine(compiled.ToString());
            };

            return factory;
        }

        public static DbConnection DapperMapEntity<TObject>(this DbConnection dbConnection)
        {
            Dapper.SqlMapper.SetTypeMap(
                    typeof(TObject),
                    new Dapper.CustomPropertyTypeMap(
                    typeof(TObject),
                    (type, columnName) => type.GetProperties()
                                              .FirstOrDefault(prop => prop.GetCustomAttributes(false)
                                                                          .OfType<ColumnAttribute>()
                                                                          .Any(attr => attr.Name.ToLower() == columnName.ToLower()) ||
                                                                                       prop.Name.ToLower() == columnName.ToLower())));

            return dbConnection;
        }
        
        public static async Task<IEnumerable<T>> ToListDapperMapEntityAsync<T>(this Query query)
        {
            DapperMapEntity<T>(default);           
            return await query.GetAsync<T>();
        }
    }
}
