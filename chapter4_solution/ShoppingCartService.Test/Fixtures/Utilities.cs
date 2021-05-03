using System;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.Mapping;

namespace ShoppingCartService.Test.Fixtures
{
    public class MongoUtility
    {
        private readonly MongoClient DbClient;

        public MongoUtility()
        {
            DbClient = new MongoClient("mongodb://localhost:1234/?connectTimeoutMS=1000");  
        }

        public ShoppingCartDatabaseSettings RetrieveDatabaseSettings()
        {
            var config = InitConfiguration();
            var collectionName = config["ShoppingCartDatabaseSettings:CollectionName"];
            var connectionString = config["ShoppingCartDatabaseSettings:ConnectionString"];
            var database = config["ShoppingCartDatabaseSettings:DatabaseName"];
            
            var shoppingCartDatabaseSettings = new ShoppingCartDatabaseSettings
            {
                CollectionName = collectionName, ConnectionString = connectionString, DatabaseName = database
            };

            return shoppingCartDatabaseSettings;
        }
        
        public IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }

        public void CreateDatabase(string name)
        {
            DbClient.GetDatabase(name); // this will result in a create
        }

        public void DropDatabase(string name)
        {
            DbClient.DropDatabase(name);
        }

        public bool IsItUp()
        {
            bool success = true;
            
            try
            {
                var list = DbClient.ListDatabases();
                success = list.ToList().Count > 0;
            }
            catch (TimeoutException ex)
            {
                success = false;
            }

            return success;

        }

        public MapperConfiguration InitMapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            return config;
        }
    }
}