using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public interface ICouponRepository
    {
        Coupon Create(Coupon coupon);
        Coupon FindById(string id);
        void Remove(string id);
    }
}