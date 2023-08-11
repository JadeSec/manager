using System;
using System.Linq;
using System.Reflection;
using App.Infra.Bootstrap;
using Microsoft.EntityFrameworkCore;
using App.Infra.Bootstrap.Attributes;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.Database.Interfaces;
using SqlKata.Compilers;
using MySql.Data.MySqlClient;
using System.Data.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;

namespace App.Infra.Integration.Database.Providers
{
    [Scoped]
    public class MySQLProvider: DbContext, IProvider, IScoped<MySQLProvider>
    {       
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _environment;

        private string[] _assemblies
            => _configuration.GetSection("Database")
                             .GetSection("Assemblies")
                             .Get<string[]>();

        public string ConnectionString
            => _configuration.GetSection("Database")
                             .GetSection("Providers")
                             .GetValue<string>("MySQL");

        /// <summary>
        /// Using in dapper, SqlKata. 
        /// </summary>
        public DbConnection Connection
            => new MySqlConnection(ConnectionString);

        public Compiler Compiler 
            => new MySqlCompiler();

        public MySQLProvider(IConfiguration configuration, IHostEnvironment environment) : base()
        {
            _environment = environment;
            _configuration = configuration;
        }            

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (string assemblyPath in _assemblies)
            {
                Assembly assembly = Assembly.Load(assemblyPath);

                foreach (Type type in assembly.ExportedTypes
                                              .Where(x => x.FullName.Contains("Entity") &&
                                                          x.IsPublic &&
                                                          !x.IsAbstract &&
                                                          !x.IsInterface &&
                                                          !x.IsEnum &&
                                                          x.IsClass))
                {
                    if (type.IsClass)
                    {
                        var method = modelBuilder.GetType()
                                                 .GetMethod("Entity", new Type[] { });

                        method = method.MakeGenericMethod(new Type[] { type });

                        method.Invoke(modelBuilder, null);
                    }
                }
            }

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                if (_environment.IsDevelopment())
                    optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));

                optionsBuilder.UseMySql(ConnectionString);                
            }
        }
    }
}
