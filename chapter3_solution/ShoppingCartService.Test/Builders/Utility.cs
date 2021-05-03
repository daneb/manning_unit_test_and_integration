using System.Collections.Generic;
using ShoppingCartService.Controllers.Models;
using ShoppingCartService.Models;

namespace ShoppingCartService.Test.Builders
{
    public static class Utility
    {
        public static Address AddressGenerate(string country = "USA", string city = "Dallas", string street = "SomePlace")
        {
            return new Address() {Country = country, City = city, Street = street};
        }

        public static List<ItemDto> GenerateListOfDtoItems()
        {
            return new List<ItemDto>()
            {
                new ItemDto("1", "Apple", 1, 1),
                new ItemDto( "2", "Orange", 2, 2 )
            };
        }
    }
}