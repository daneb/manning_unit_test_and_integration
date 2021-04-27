using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Controllers;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Fixtures;
using Xunit;

namespace ShoppingCartService.Test.Controllers
{
    public class ShoppingCartControllerTest : IDisposable
    {
        private ShoppingCartManager _shoppingCartManager;
        private MongoUtility _mongoUtility;
        private string _databaseName;

        public ShoppingCartControllerTest()
        {
            MongoSetup.Start();
            ShippingCalculator shippingCalculator = new ShippingCalculator();
            
            _mongoUtility = new MongoUtility();
            var settings = _mongoUtility.RetrieveDatabaseSettings();
            _mongoUtility.CreateDatabase("ShoppingCartDatabaseSettings");
            _databaseName = settings.DatabaseName;
            
            var config = _mongoUtility.InitMapper();
            ShoppingCartRepository shoppingCartRepository = new ShoppingCartRepository(settings);
            _shoppingCartManager =
                new ShoppingCartManager(
                    shoppingCartRepository,
                    new AddressValidator(),
                    config.CreateMapper(),
                    new CheckOutEngine(shippingCalculator, config.CreateMapper()));
            
        }

        [Fact]
        public void Create_Item_In_Cart()
        {
            var shoppingCartController =
                new ShoppingCartController(_shoppingCartManager, null);

            var cartCustomerDto = GenerateCreateCartDto();
            var actionResult = shoppingCartController.Create(cartCustomerDto);

            var result = actionResult.Result as CreatedResult;
            result?.Value.Should().Be(201);

        }

        [Fact]
        public void Get_All_Items_In_Cart()
        {
            var shoppingCartController =
                new ShoppingCartController(_shoppingCartManager, null);

            var cartCustomerDto = GenerateCreateCartDto();
            shoppingCartController.Create(cartCustomerDto);
            shoppingCartController.Create(cartCustomerDto);
            var allItems = shoppingCartController.GetAll();

            allItems.Count().Should().Be(2);
        }
        
        [Fact]
        public void Get_Item_From_Cart()
        {
            var shoppingCartController =
                new ShoppingCartController(_shoppingCartManager, null);

            var cartCustomerDto = GenerateCreateCartDto();
            var actionResult = shoppingCartController.Create(cartCustomerDto);
            var result = actionResult.Result as CreatedAtRouteResult;
            var shoppingCart = result?.Value as ShoppingCartDto;
            var item = shoppingCartController.FindById(shoppingCart?.Id);
            
            item.Should().NotBeNull();
        }
        
        [Fact]
        public void Calculate_Total_By_CartId()
        {
            var shoppingCartController =
                new ShoppingCartController(_shoppingCartManager, null);

            var cartCustomerDto = GenerateCreateCartDto();
            var actionResult = shoppingCartController.Create(cartCustomerDto);
            var result = actionResult.Result as CreatedAtRouteResult;
            var shoppingCart = result?.Value as ShoppingCartDto;
            var item = shoppingCartController.CalculateTotals(shoppingCart?.Id);

            item.Value.Total.Should().Be(59.4);
        }

        private CustomerDto GenerateCreateCustomerDto()
        {
            CustomerDto customerDto = new CustomerDto()
            {
                Address = Builders.Utility.AddressGenerate(),
                Id = "1",
                CustomerType = CustomerType.Premium
            };
            return customerDto;
        }

        private CreateCartDto GenerateCreateCartDto()
        {

            var customerDto = GenerateCreateCustomerDto();
            
            var createCartDto = new CreateCartDto
                {
                    Customer = customerDto,
                    Items = new List<ItemDto>
                    {
                    new ItemDto("some-product-1", "some-product-name", 10, 2),
                    new ItemDto("some-product-2", "some-product-name", 10, 2),
                    new ItemDto("some-product-3", "some-product-name", 10, 2) 
                    },
                    ShippingMethod = ShippingMethod.Standard
                };

            return createCartDto;
        }

        public void Dispose()
        {
            _mongoUtility.DropDatabase(_databaseName);
            MongoSetup.Stop();
        }
    }
}