using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Config;
using ShoppingCartService.Controllers;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Fixtures;
using Xunit;

namespace ShoppingCartService.Test.Controllers
{
    public class CouponControllerTest : IDisposable
    {

        private MongoUtility _mongoUtility;

        private readonly CouponRepository _couponRepository;
        
        public CouponControllerTest() 
        {
            MongoSetup.Start();
            
            _mongoUtility = new MongoUtility();
            var shoppingCartDatabaseSettings = _mongoUtility.RetrieveDatabaseSettings();
            _mongoUtility.CreateDatabase("ShoppingCartDatabaseSettings");
            _couponRepository = new CouponRepository(shoppingCartDatabaseSettings);
        }

        [Fact]
        public void Create_A_Coupon()
        {
            CouponController couponController = new CouponController(_couponRepository, null);

            var coupon= GenerateCoupon();
            var actionResult = couponController.Create(coupon);

            var result = actionResult.Result as CreatedAtRouteResult;
            
            result.Should().NotBe(null);
            result?.StatusCode.Should().Be(201);
        }

        [Fact]
        public void Remove_A_Coupon()
        {
            CouponController couponController = new CouponController(_couponRepository, null);
            var coupon= GenerateCoupon();
            var actionResult = couponController.Create(coupon);
            var response = actionResult.Result as CreatedAtRouteResult;
            var couponId = (response.Value as Coupon).Id;
            
            var result = couponController.DeleteCoupon(couponId);

            result.Should().BeOfType<NoContentResult>();
        }
        
        private Coupon GenerateCoupon()
        {
            Coupon coupon = new Coupon()
            {
                Amount = 10,
                Type = CouponType.Percentage,
                ExpiryDate = DateTime.Now,
            };
            
            return coupon;
        }

        public void Dispose()
        {
            MongoSetup.Stop();
        }
    }
}