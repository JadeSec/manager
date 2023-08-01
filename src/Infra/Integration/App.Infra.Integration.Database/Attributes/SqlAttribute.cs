using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace App.Infra.Integration.Database.Attributes
{
    /// <summary>
    /// Use with Dapper:
    /// 
    /// [Sql(UPDATE table SET column = 1 WHERE id = @id)]
    /// public void Abc(){
    ///   using (var connection = Dapper)
    ///   {                
    ///      return await connection.ExecuteAsync(Sql.Query, new
    ///      {
    ///        id = id
    ///      });
    ///    }    
    /// }   
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SqlAttribute: Attribute
    {
        public string Query { get; set; }

        public SqlAttribute(string query)
        {
            Query = query;
        }
    }

    public static class Sql
    {
        public static string Query
        {
            get
            {                
                StackTrace stackTrace = new StackTrace();                
                StackFrame frame =  stackTrace.GetFrames()
                                              .Where(x => x.GetMethod().GetCustomAttribute<SqlAttribute>() != null)
                                              .FirstOrDefault();             

                return frame.GetMethod()
                            .GetCustomAttribute<SqlAttribute>()
                            .Query;
            }       
       }
    }
}
