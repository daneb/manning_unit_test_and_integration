using System;
using ShoppingCartService.BusinessLogic.Exceptions;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.BusinessLogic
{
    public class CouponEngine
    {
        public double CalculateDiscount(CheckoutDto checkout, Coupon coupon)
        {

            if (checkout == null || coupon == null)
                return 0;

            if (DateTime.Now.Subtract(coupon.ExpiryDate).Days > 30)
                throw new CouponExpiredException("Coupon past 30 days and has expired.");

            if (coupon.Amount > checkout.Total)
                throw new InvalidCouponException("Coupon amount cannot be greater than total.");

            if (coupon.Amount < 0)
                throw new InvalidCouponException("Coupon amount cannot be negative.");

            if (coupon.Type == CouponType.Percentage && coupon.Amount >= 100)
                throw new InvalidCouponException("Coupon amount cannot be equal to 100.");
            
            return checkout.Total * coupon.Amount;
            
        }
    }
}