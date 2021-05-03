using System;
using FluentAssertions;
using FluentAssertions.Specialized;
using ShoppingCartService.BusinessLogic;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Builders;
using Xunit;

namespace ShoppingCartService.Test.BusinessLogic
{
    public class CouponTest
    {
        [Fact]
        public void When_Coupon_Is_Null_Return_Zero()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems()};
            var checkout = new CheckoutDto(shoppingCartDto, 10, 1, 100);

            var result = sut.CalculateDiscount(checkout, null);

            result.Should().Be(0);
        }

        [Fact]
        public void Coupon_Cannot_Be_Greater_Than_Cart_Total()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems() };
            var checkout = new CheckoutDto(shoppingCartDto, 10, 1, 10);

            Action result = () => sut.CalculateDiscount(checkout, new Coupon() 
                { Amount = 100, ExpiryDate = DateTime.Now.AddDays(-60) });

            result.Should().Throw<CouponExpiredException>()
                .WithMessage("Coupon past 30 days and has expired.");

        }

        [Fact]
        public void Coupon_Cannot_Be_A_Negative_Number()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems() };
            var checkout = new CheckoutDto(shoppingCartDto, 10, 1, 10);

            Action result = () => sut.CalculateDiscount(checkout, 
                new Coupon() { Amount = -2, ExpiryDate = DateTime.Now.AddDays(10) });

            result.Should().Throw<InvalidCouponException>()
                .WithMessage("Coupon amount cannot be negative.");
            
        }

        [Fact]
        public void Coupon_Calculated_As_A_Percentage()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems() };
            var checkout = new CheckoutDto(shoppingCartDto, 10, 1, 10);

            var result = sut.CalculateDiscount(checkout, 
                new Coupon() { Amount = 2, Type = CouponType.Percentage, ExpiryDate = DateTime.Now.AddDays(10) });

            result.Should().Be(20);

        }
        
        [Fact]
        public void Coupon_No_More_Than_OneHundred()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems() };
            var checkout = new CheckoutDto(shoppingCartDto, 20, 10, 200);

            Action result = () => sut.CalculateDiscount(checkout, 
                new Coupon() { Amount = 100, Type = CouponType.Percentage, ExpiryDate = DateTime.Now.AddDays(10) });

            result.Should().Throw<InvalidCouponException>()
                .WithMessage("Coupon amount cannot be equal to 100.");

        }
        
        [Fact]
        public void Coupon_Expired()
        {
            var sut = new CouponEngine();
            var shoppingCartDto = new ShoppingCartDto() {Id = "1", Items = Utility.GenerateListOfDtoItems() };
            var checkout = new CheckoutDto(shoppingCartDto, 20, 10, 200);

            Action result = () => sut.CalculateDiscount(checkout, 
                new Coupon() { Amount = 100, Type = CouponType.Percentage, ExpiryDate = DateTime.Now.AddDays(-60) });

            result.Should().Throw<CouponExpiredException>()
                .WithMessage("Coupon past 30 days and has expired.");

        }
        
        
    }
}