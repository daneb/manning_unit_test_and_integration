using MongoDB.Driver;
using ShoppingCartService.Config;
using ShoppingCartService.DataAccess.Entities;
using ShoppingCartService.Models;

namespace ShoppingCartService.DataAccess
{
    public class CouponRepository : ICouponRepository
    {
        private readonly IMongoCollection<Coupon> _coupon;
        
        public CouponRepository(IShoppingCartDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            
            _coupon = database.GetCollection<Coupon>(settings.CollectionName);
        }
        
        public Coupon Create(Coupon coupon)
        {
            _coupon.InsertOne(coupon);

            return coupon;
        }

        public Coupon FindById(string id) =>
            _coupon.Find(coupon => coupon.Id == id)
                    .FirstOrDefault();
        
        public void Remove(string id) => _coupon.DeleteOne(c => c.Id == id);
    }
}