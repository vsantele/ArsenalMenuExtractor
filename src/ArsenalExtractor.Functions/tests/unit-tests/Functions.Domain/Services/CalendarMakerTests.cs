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
                            Soup = "Soup 1",
                            Vegetarian = "Végé 1"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-20"),
                            Day = "Mardi",
                            Soup = "Soup 2",
                            Vegetarian = "Végé 2"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-21"),
                            Day = "Mercredi",
                            Soup = "Soup 3",
                            Vegetarian = "Végé 3"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-22"),
                            Day = "Jeudi",
                            Soup = "Soup 4",
                            Vegetarian = "Végé 4"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-23"),
                            Day = "Vendredi",
                            Soup = "Soup 5",
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
            Assert.Contains("DESCRIPTION:Jour: Lundi\\nVégé: Végé 1\\nSoup: Soup 1", result);
            Assert.Contains("DESCRIPTION:Jour: Mardi\\nVégé: Végé 2\\nSoup: Soup 2", result);
            Assert.Contains("DESCRIPTION:Jour: Mercredi\\nVégé: Végé 3\\nSoup: Soup 3", result);
            Assert.Contains("DESCRIPTION:Jour: Jeudi\\nVégé: Végé 4\\nSoup: Soup 4", result);
            Assert.Contains("DESCRIPTION:Jour: Vendredi\\nVégé: Végé 5\\nSoup: Soup 5", result);

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
                            Soup = "Soup 1",
                            Vegetarian = "Végé 1"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-20"),
                            Day = "Mardi",
                            Soup = "Soup 2",
                            Vegetarian = "Végé 2"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-21"),
                            Day = "Mercredi",
                            Soup = "Soup 3",
                            Vegetarian = "Végé 3"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-22"),
                            Day = "Jeudi",
                            Soup = "Soup 4",
                            Vegetarian = "Végé 4"
                        },
                        new MenuInfo {
                            Date = DateTime.Parse("2022-09-23"),
                            Day = "Vendredi",
                            Soup = "Soup 5",
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

