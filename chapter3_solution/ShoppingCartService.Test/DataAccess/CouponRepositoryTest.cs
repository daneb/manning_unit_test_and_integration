using System;
using FluentAssertions;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Test.Fixtures;
using Xunit;

namespace ShoppingCartService.Test.DataAccess
{
    public class CouponRepositoryTest : IDisposable
    {
        private readonly ShoppingCartDatabaseSettings _shoppingCartDatabaseSettings;
        private readonly MongoUtility _mongoUtility;
        
        public CouponRepositoryTest()
        {
            _mongoUtility = new MongoUtility();
            Fixtures.MongoSetup.Start();
            _mongoUtility.CreateDatabase("ShoppingCartDatabaseSettings");
            _shoppingCartDatabaseSettings = _mongoUtility.RetrieveDatabaseSettings();
        }
        
        [Fact]
        public void Creates_A_Coupon()
        {
            CouponRepository couponRepository = new CouponRepository(_shoppingCartDatabaseSettings);
            var coupon = new Coupon() { Amount = 10, Type = CouponType.Percentage, ExpiryDate = DateTime.Now };
            var result = couponRepository.Create(coupon);
            
            result.Should().BeEquivalentTo(coupon); 

        }

        [Fact]
        public void Deletes_A_Coupon()
        {
            CouponRepository couponRepository = new CouponRepository(_shoppingCartDatabaseSettings);
            var coupon = new Coupon() { Amount = 10, Type = CouponType.Percentage, ExpiryDate = DateTime.Now };
            var createdCoupon = couponRepository.Create(coupon);
            string couponId = createdCoupon.Id;

            couponRepository.Remove(couponId);

            var result = couponRepository.FindById(couponId);
            result.Should().BeNull();

        }
        
        [Fact]
        public void Finds_Coupon_By_Id()
        {
            CouponRepository couponRepository = new CouponRepository(_shoppingCartDatabaseSettings);
            var date = DateTime.Now.AddDays(-10);
            var coupon = new Coupon() { Amount = 10, Type = CouponType.Percentage, ExpiryDate = date };
            var createdCoupon = couponRepository.Create(coupon);
            string couponId = createdCoupon.Id;

            var result = couponRepository.FindById(couponId);
            
            result.Id.Should().Be(createdCoupon.Id);

        }
        
        public void Dispose()
        {
            _mongoUtility.DropDatabase(_shoppingCartDatabaseSettings.DatabaseName);
            Fixtures.MongoSetup.Stop();
        }
    }
}