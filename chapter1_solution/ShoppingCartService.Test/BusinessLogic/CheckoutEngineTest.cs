using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartService.Test.BusinessLogic
{
    public class CheckoutEngineTest
    {
        [Theory]
        [InlineData(CustomerType.Premium, 21.6, 10.0, 10)]
        [InlineData(CustomerType.Standard, 24.0, 0, 10)]
        public void Calculate_Totals(CustomerType customerType, double total, double discount, double shippingCost)
        {
            Cart cart = GenerateCart(customerType);
            var shippingCalculator = Substitute.For<IShippingCalculator>();
            shippingCalculator.CalculateShippingCost(cart).Returns(10);
            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
    
            var sut = new CheckOutEngine(shippingCalculator, mapper);
            
            var result = sut.CalculateTotals(cart);

            result.Total.Should().Be(total);
            result.CustomerDiscount.Should().Be(discount);
            result.ShippingCost.Should().Be(shippingCost);
            result.ShoppingCart.Items.Should().NotContainNulls();
            result.ShoppingCart.CustomerType.Should().Be(customerType);
            result.ShoppingCart.ShippingMethod.Should().Be(ShippingMethod.Expedited);

        }

        private Cart GenerateCart(CustomerType customerType)
        {
            Address address = new Address() {Country = "USA", City = "Dallas"};
            Cart cart = new Cart
            {
                Items = new List<Item>()
                {
                    new Item() {Quantity = 1, Price = 1},
                    new Item() {Quantity = 2, Price = 2},
                    new Item() {Quantity = 3, Price = 3}
                },
                ShippingAddress = address,
                CustomerType = customerType,
                ShippingMethod = ShippingMethod.Expedited
            };

            return cart;
        }
    }
}