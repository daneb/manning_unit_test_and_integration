using System.Collections.Generic;
using FluentAssertions;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using Xunit;

namespace ShoppingCartService.Test.BusinessLogic.Validation
{
    public class AddressValidatorTest
    {
        [Theory]
        [MemberData(nameof(Data))]
        public void Validates_To_False_If_Address_Null(Address address, bool expected)
        {
            var sut = new AddressValidator();

            var result = sut.IsValid(address);

            result.Should().Be(expected);
        }
        
        public static List<object[]> Data()
        {
            return new List<object[]>
            {
                new object[] {null, false},
                new object[] {new Address() {Country = "USA", City = "Dallas", Street = "Some Place"}, true},
                new object[] {new Address() {Country = null, City = "Dallas", Street = "Some Place"}, false},
                new object[] {new Address() {Country = "", City = "Dallas", Street = "Some Place"}, false},
                new object[] {new Address() {Country = "USA", City = "", Street = "Some Place"}, false},
                new object[] {new Address() {Country = "USA", City = null, Street = "Some Place"}, false},
                new object[] {new Address() {Country = "USA", City = "Dallas", Street = ""}, false},
                new object[] {new Address() {Country = "USA", City = "Dallas", Street = null}, false}
            };

        }
    }
}