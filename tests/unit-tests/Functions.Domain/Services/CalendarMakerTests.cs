using ArsenalExtractor.Functions.Domain.Helpers;
using ArsenalExtractor.Functions.Domain.Models;
using ArsenalExtractor.Functions.Domain.Services;
using Xunit;

namespace UnitTests.Functions.Domain.Services
{
    public class CalendarMakerTests
    {
        [Fact]
        public void GenerateICal_ShouldCorrectlyReturnText()
        {
            //arrange
            var menusInput = new List<Menu>
            {
                new Menu() {
                    WeekInfo = new WeekInfo() {
                        DayStart = "19",
                        MonthStart = "09",
                        DayEnd = "25",
                        MonthEnd = "09",
                        Year = "2022"
                    },
                    MenuDetails = new List<List<string>> {
                        new List<string> {
                            "Lundi",
                            "Chef 1",
                            "Végé 1"
                        },
                        new List<string> {
                            "Mardi",
                            "Chef 2",
                            "Végé 2"
                        },
                        new List<string> {
                            "Mercredi",
                            "Chef 3",
                            "Végé 3"
                        },
                        new List<string> {
                            "Jeudi",
                            "Chef 4",
                            "Végé 4"
                        },
                        new List<string> {
                            "Vendredi",
                            "Chef 5",
                            "Végé 5"
                        }
                    }

                }
            };

            var dateHelper = new DateHelper();

            var sut = new CalendarMaker(dateHelper);

            //act
            var result = sut.GenerateICal(menusInput);

            //assert
            Assert.Contains("DTSTART:20220919T120000", result);
            Assert.Contains("DTSTART:20220920T120000", result);
            Assert.Contains("DTSTART:20220921T120000", result);
            Assert.Contains("DTSTART:20220922T120000", result);
            Assert.Contains("DTSTART:20220923T120000", result);
            Assert.Contains("DTEND:20220919T133000", result);
            Assert.Contains("DTEND:20220920T133000", result);
            Assert.Contains("DTEND:20220921T133000", result);
            Assert.Contains("DTEND:20220922T133000", result);
            Assert.Contains("DTEND:20220923T133000", result);
            Assert.Contains("SUMMARY:Lundi", result);
            Assert.Contains("SUMMARY:Mardi", result);
            Assert.Contains("SUMMARY:Mercredi", result);
            Assert.Contains("SUMMARY:Jeudi", result);
            Assert.Contains("SUMMARY:Vendredi", result);
            Assert.Contains("DESCRIPTION:Jour: Lundi | Chef: Chef 1 | Végé: Végé 1", result);
            Assert.Contains("DESCRIPTION:Jour: Mardi | Chef: Chef 2 | Végé: Végé 2", result);
            Assert.Contains("DESCRIPTION:Jour: Mercredi | Chef: Chef 3 | Végé: Végé 3", result);
            Assert.Contains("DESCRIPTION:Jour: Jeudi | Chef: Chef 4 | Végé: Végé 4", result);
            Assert.Contains("DESCRIPTION:Jour: Vendredi | Chef: Chef 5 | Végé: Végé 5", result);

        }
        [Fact]
        public void GenerateICal_SelectedFavMenuShouldBeInSumary()
        {
            //arrange
            var menusInput = new List<Menu>
            {
                new Menu() {
                    WeekInfo = new WeekInfo() {
                        DayStart = "19",
                        MonthStart = "09",
                        DayEnd = "25",
                        MonthEnd = "09",
                        Year = "2022"
                    },
                    MenuDetails = new List<List<string>> {
                        new List<string> {
                            "Lundi",
                            "Chef 1",
                            "Végé 1"
                        },
                        new List<string> {
                            "Mardi",
                            "Chef 2",
                            "Végé 2"
                        },
                        new List<string> {
                            "Mercredi",
                            "Chef 3",
                            "Végé 3"
                        },
                        new List<string> {
                            "Jeudi",
                            "Chef 4",
                            "Végé 4"
                        },
                        new List<string> {
                            "Vendredi",
                            "Chef 5",
                            "Végé 5"
                        }
                    }

                }
            };

            var dateHelper = new DateHelper();

            var sut = new CalendarMaker(dateHelper);

            //act
            var result = sut.GenerateICal(menusInput, "vege");

            //assert
            Assert.Contains("SUMMARY:Végé 1", result);
            Assert.Contains("SUMMARY:Végé 2", result);
            Assert.Contains("SUMMARY:Végé 3", result);
            Assert.Contains("SUMMARY:Végé 4", result);
            Assert.Contains("SUMMARY:Végé 5", result);

        }
    }
}

