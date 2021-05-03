using System;
using System.Text.Json.Serialization;
using FluentAssertions;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Builders;
using ShoppingCartService.Test.Fixtures;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace ShoppingCartService.Test.DataAccess
{
    public class ShoppingCartRepositoryIntegrationTests : IDisposable
    {
        private readonly ShoppingCartDatabaseSettings _shoppingCartDatabaseSettings;
        private readonly MongoUtility _mongoUtility;
        
        public ShoppingCartRepositoryIntegrationTests()
        {
            _mongoUtility = new MongoUtility();
            Fixtures.MongoSetup.Start();
            _mongoUtility.CreateDatabase("ShoppingCartDatabaseSettings");
            _shoppingCartDatabaseSettings = _mongoUtility.RetrieveDatabaseSettings();
        }
        
        
        [Fact]
        public void Finds_A_New_Cart()
        {
            var cartBuilder = new CartBuilder();
            var address = Builders.Utility.AddressGenerate();
            var shoppingCartRepository = new ShoppingCartRepository(_shoppingCartDatabaseSettings);
            var cart = cartBuilder.GenerateCart(address, CustomerType.Premium, ShippingMethod.Expedited);
            var storedCart = shoppingCartRepository.Create(cart);

            var result = shoppingCartRepository.FindById(storedCart.Id);
            
            result.Should().BeEquivalentTo(cart);
        }
        
        [Fact]
        public void Creates_A_Cart()
        {
            var cartBuilder = new CartBuilder();
            var address = Builders.Utility.AddressGenerate();
            var shoppingCartRepository = new ShoppingCartRepository(_shoppingCartDatabaseSettings);
            var cartOne = cartBuilder.GenerateCart(address, CustomerType.Premium, ShippingMethod.Expedited);
            var cartTwo = cartBuilder.GenerateCart(address, CustomerType.Premium, ShippingMethod.Express);
            var cartThree = cartBuilder.GenerateCart(address, CustomerType.Premium, ShippingMethod.Priority);
            shoppingCartRepository.Create(cartOne);
            shoppingCartRepository.Create(cartTwo);
            shoppingCartRepository.Create(cartThree);
            
            var result = shoppingCartRepository.FindAll();

            result.Should().HaveCount(3);

        }

        public void Dispose()
        {
            _mongoUtility.DropDatabase(_shoppingCartDatabaseSettings.DatabaseName);
            Fixtures.MongoSetup.Stop();
        }
    }
}