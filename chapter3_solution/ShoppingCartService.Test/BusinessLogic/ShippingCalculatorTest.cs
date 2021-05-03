using System;
using System.Collections.Generic;
using FluentAssertions;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Builders;
using Xunit;

namespace ShoppingCartService.Test.BusinessLogic
{
    public class ShippingCalculatorTest
    {
        
        [Theory]
        [MemberData(nameof(SameCity))]
        public void Calculates_Shipping_Cost_For_Same_City(Cart cart, double low, double high)
        {
            
            var sut = new ShippingCalculator();

            var result = sut.CalculateShippingCost(cart);

            result.Should().BeInRange(low, high);

        }
        
        [Theory]
        [MemberData(nameof(SameCountry))]
        public void Calculates_Shipping_Cost_For_Same_Country(Cart cart, double low, double high)
        {
            
            var sut = new ShippingCalculator();

            var result = sut.CalculateShippingCost(cart);

            result.Should().BeInRange(low, high);

        }
        
        [Theory]
        [MemberData(nameof(International))]
        public void Calculates_Shipping_Cost_For_International(Cart cart, double low, double high)
        {
            
            var sut = new ShippingCalculator();

            var result = sut.CalculateShippingCost(cart);

            result.Should().BeInRange(low, high);

        }
        
        public static List<object[]> SameCity()
        {
            CartBuilder cartFactory = new CartBuilder();
            return cartFactory.GenerateSameCity(Utility.AddressGenerate());
        }

        public static List<object[]> SameCountry()
        {
            Address address = Utility.AddressGenerate("USA", "New York");
            CartBuilder cartFactory = new CartBuilder();
            return cartFactory.GenerateSameCountry(address);
        }
        
        public static List<object[]> International()
        {
            Address address = Utility.AddressGenerate("Ukraine");
            CartBuilder cartFactory = new CartBuilder();
            return cartFactory.GenerateInternational(address);
        }
        
    }
}