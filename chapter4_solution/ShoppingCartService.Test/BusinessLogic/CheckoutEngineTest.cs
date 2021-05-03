using System.Collections.Generic;
using System.Configuration;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Mapping;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Builders;
using Xunit;

namespace ShoppingCartService.Test.BusinessLogic
{
    public class CheckoutEngineTest
    {
        [Theory]
        [InlineData(CustomerType.Premium, 9.0, 10.0, 10)]
        [InlineData(CustomerType.Standard, 10.0, 0, 10)]
        public void Calculate_Totals(CustomerType customerType, double total, double discount, double shippingCost)
        {
            Address address = Utility.AddressGenerate();
            CartBuilder cartBuilder = new CartBuilder();
            Cart cart = cartBuilder.GenerateCart(address, customerType, ShippingMethod.Expedited);
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
    }
}