using ShoppingCartService.Models;

namespace ShoppingCartService.Test.Builders
{
    public static class Utility
    {
        public static Address AddressGenerate(string country = "USA", string city = "Dallas", string street = "SomePlace")
        {
            return new Address() {Country = country, City = city, Street = street};
        }
    }
}