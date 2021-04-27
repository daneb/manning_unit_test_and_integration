using System.Collections.Generic;
using FluentAssertions;
using ShoppingCartService.BusinessLogic.Validation;
using ShoppingCartService.Models;
using ShoppingCartService.Test.Builders;
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
                new object[] { Utility.AddressGenerate(), true},
                new object[] { Utility.AddressGenerate(null), false},
                new object[] { Utility.AddressGenerate(""), false},
                new object[] { Utility.AddressGenerate("USA", ""), false},
                new object[] { Utility.AddressGenerate("USA", null), false},
                new object[] { Utility.AddressGenerate("USA", "Dallas", ""), false},
                new object[] { Utility.AddressGenerate("USA", "Dallas", null), false}
            };

        }
    }
}