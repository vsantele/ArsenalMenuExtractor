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
                        WeekNumber = 38,
                        StartDate = DateTime.Parse("2022-09-19"),
                        EndDate = DateTime.Parse("2022-09-25")
                        },
                    MenuInfos = new List<MenuInfo> {
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-19"),
                            Day = "Lundi",
                            Chef = "Chef 1",
                            Vegetarian = "Végé 1"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-20"),
                            Day = "Mardi",
                            Chef = "Chef 2",
                            Vegetarian = "Végé 2"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-21"),
                            Day = "Mercredi",
                            Chef = "Chef 3",
                            Vegetarian = "Végé 3"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-22"),
                            Day = "Jeudi",
                            Chef = "Chef 4",
                            Vegetarian = "Végé 4"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-23"),
                            Day = "Vendredi",
                            Chef = "Chef 5",
                            Vegetarian = "Végé 5"
                        }
                    }

                }
            };


            var sut = new CalendarMaker();

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
            Assert.Contains("DESCRIPTION:Jour: Lundi\\nChef: Chef 1\\nVégé: Végé 1", result);
            Assert.Contains("DESCRIPTION:Jour: Mardi\\nChef: Chef 2\\nVégé: Végé 2", result);
            Assert.Contains("DESCRIPTION:Jour: Mercredi\\nChef: Chef 3\\nVégé: Végé 3", result);
            Assert.Contains("DESCRIPTION:Jour: Jeudi\\nChef: Chef 4\\nVégé: Végé 4", result);
            Assert.Contains("DESCRIPTION:Jour: Vendredi\\nChef: Chef 5\\nVégé: Végé 5", result);

        }
        [Fact]
        public void GenerateICal_SelectedFavMenuShouldBeInSumary()
        {
            //arrange
            var menusInput = new List<Menu>
            {
                new Menu() {
                    WeekInfo = new WeekInfo() {
                        WeekNumber = 38,
                        StartDate = DateTime.Parse("2022-09-19"),
                        EndDate = DateTime.Parse("2022-09-25")
                        },
                    MenuInfos = new List<MenuInfo> {
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-19"),
                            Day = "Lundi",
                            Chef = "Chef 1",
                            Vegetarian = "Végé 1"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-20"),
                            Day = "Mardi",
                            Chef = "Chef 2",
                            Vegetarian = "Végé 2"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-21"),
                            Day = "Mercredi",
                            Chef = "Chef 3",
                            Vegetarian = "Végé 3"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-22"),
                            Day = "Jeudi",
                            Chef = "Chef 4",
                            Vegetarian = "Végé 4"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-23"),
                            Day = "Vendredi",
                            Chef = "Chef 5",
                            Vegetarian = "Végé 5"
                        }
                    }

                }
            };


            var sut = new CalendarMaker();

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

