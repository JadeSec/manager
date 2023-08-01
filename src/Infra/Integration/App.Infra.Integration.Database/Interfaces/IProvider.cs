using SqlKata.Compilers;
using System.Data.Common;

namespace App.Infra.Integration.Database.Interfaces
{
    public interface IProvider
    {
        /// <summary>
        /// Use only sqlkata
        /// </summary>
        Compiler Compiler { get; }

        /// <summary>
        /// Connection String 
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Using in Dapper, SqlKata 
        /// </summary>
        DbConnection Connection { get; }
    }
}
