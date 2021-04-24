using System;
using System.Collections.Generic;
using FluentAssertions;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
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
            Address address = new Address() {Country = "USA", City = "Dallas"};
            CartFactory cartFactory = new CartFactory();
            return cartFactory.GenerateSameCity(address);
        }

        public static List<object[]> SameCountry()
        {
            Address address = new Address() {Country = "USA", City = "New York"};
            CartFactory cartFactory = new CartFactory();
            return cartFactory.GenerateSameCountry(address);
        }
        
        public static List<object[]> International()
        {
            Address address = new Address() {Country = "Ukraine" };
            CartFactory cartFactory = new CartFactory();
            return cartFactory.GenerateInternational(address);
        }

        private class CartFactory
        {
            public List<object[]> GenerateSameCity(Address address)
            {

                return new List<object[]>
                {
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Expedited), 7.19, 7.20 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Express), 15.0, 15.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Priority), 12.0, 12.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Standard), 6.0, 6.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Expedited), 6.0, 6.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Express), 15.0, 15.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Priority), 6.0, 6.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Standard), 6.0, 6.1 },
                };
            }
            
            public List<object[]> GenerateSameCountry(Address address)
            {
                return new List<object[]>
                {
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Expedited), 14.39, 14.4 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Express), 30.0, 30.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Priority), 24.0, 24.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Standard), 12.0, 12.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Expedited), 12.0, 12.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Express), 30.0, 30.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Priority), 12.0, 12.0 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Standard), 12.0, 12.0 },
                };
            }
            
            public List<object[]> GenerateInternational(Address address)
            {

                return new List<object[]>
                {
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Expedited), 108.0, 108.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Express), 225.0, 225.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Priority), 180.0, 180.1 },
                    new object[] { GenerateCart(address, CustomerType.Standard, ShippingMethod.Standard), 90.0, 90.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Expedited), 90.0, 90.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Express), 225.0, 225.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Priority), 90.0, 90.1 },
                    new object[] { GenerateCart(address, CustomerType.Premium, ShippingMethod.Standard), 90.0, 90.1 },
                };
            }
            
            private Cart GenerateCart(Address address, CustomerType customerType, ShippingMethod shippingMethod)
            {
                Cart cart = new Cart
                {
                    Items = new List<Item>()
                    {
                        new Item() {Quantity = 1}, new Item() {Quantity = 2}, new Item() {Quantity = 3}
                    },
                    ShippingAddress = address,
                    CustomerType = customerType,
                    ShippingMethod = shippingMethod
                };
                
                return cart;
            }

        }
    }
}