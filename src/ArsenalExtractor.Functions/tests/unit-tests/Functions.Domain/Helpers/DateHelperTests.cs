using ArsenalExtractor.Functions.Domain.Helpers;
using ArsenalExtractor.Functions.Domain.Models;
using ArsenalExtractor.Functions.Domain.Services;
using Xunit;

namespace UnitTests.Functions.Domain.Services
{
    public class DateHelperTests
    {
        [Fact]
        public void GetMonthNumber_ShouldCorrectlyReturnMonthNumberFromText()
        {
            //arrange
            var monthName1 = "janvier";
            var monthName2 = "février";
            var monthName3 = "Mars";
            var monthName4 = "04";

            var sut = new DateHelper();

            //act
            var result1 = sut.GetMonthNumber(monthName1);
            var result2 = sut.GetMonthNumber(monthName2);
            var result3 = sut.GetMonthNumber(monthName3);
            var result4 = sut.GetMonthNumber(monthName4);

            //assert
            Assert.Equal("01", result1);
            Assert.Equal("02", result2);
            Assert.Equal("03", result3);
            Assert.Equal("04", result4);
        }
        [Fact]
        public void ConvertDate_ShouldCorrectlyReturnDate()
        {
            //arrange
            var day = "06";
            var monthName1 = "janvier";
            var monthName2 = "03";
            var year = "2022";

            var sut = new DateHelper();

            //act
            var result1 = sut.ConvertDate(day, monthName1, year);
            var result2 = sut.ConvertDate(day, monthName2, year); ;

            //assert
            Assert.Equal("2022-01-06", result1);
            Assert.Equal("2022-03-06", result2);
        }
    }
}

