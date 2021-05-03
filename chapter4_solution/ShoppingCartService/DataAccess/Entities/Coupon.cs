using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ShoppingCartService.DataAccess.Entities
{
    public record Coupon
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [Required]
        public int Amount { get; init; }

        public CouponType Type { get; init; } = CouponType.None;

        public DateTime ExpiryDate { get; init; }
    }

    public enum CouponType
    {
       Percentage,
       None
    }
}