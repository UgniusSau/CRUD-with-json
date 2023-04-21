using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using VismaShortage.Models;
using VismaShortage.Utils;

namespace VismaShortage.Tests.Unit
{
    [Collection("ShortageUtilsTests")]
    public class ShortageUtilsTests
    {
        //test data
        User userBob = new User("bob", "password2", false);
        User userTom = new User("tom", "password2", false);
        User userAdmin= new User("admin", "password2", true);

        List<Shortage> shortages = new List<Shortage>
        {
            new Shortage("Beans", "bob", Room.Kitchen, Category.Food, 5, new DateOnly(2021, 4, 21)),
            new Shortage("Projector", "tom", Room.MeetingRoom, Category.Electronics, 8, new DateOnly(2021, 4, 22)),
            new Shortage("Cable", "tom", Room.MeetingRoom, Category.Electronics, 6, new DateOnly(2021, 4, 20)),
        };

        [Fact]
        public void PrintShortagesByRights_Displays_AllShortagesSortedByPriority()
        {
            //Arrange
            string expectedOutput = $"All shortages:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Projector{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 8{Environment.NewLine}" +
                $"Created on: 2021-04-22{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Cable{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 6{Environment.NewLine}" +
                $"Created on: 2021-04-20{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Beans{Environment.NewLine}" +
                $"Name: bob{Environment.NewLine}" +
                $"Room: Kitchen{Environment.NewLine}" +
                $"Category: Food{Environment.NewLine}" +
                $"Priority: 5{Environment.NewLine}" +
                $"Created on: 2021-04-21";

            var output = new StringWriter();
            Console.SetOut(output);

            var input = new StringReader("");
            Console.SetIn(input);
            //Act
            ShortageUtils.PrintShortagesByRights(shortages, userAdmin);

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void PrintShortagesByRights_Displays_UserShortagesSortedByPriority()
        {
            //Arrange
            string expectedOutput = $"Shortages created by bob:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Beans{Environment.NewLine}" +
                $"Name: bob{Environment.NewLine}" +
                $"Room: Kitchen{Environment.NewLine}" +
                $"Category: Food{Environment.NewLine}" +
                $"Priority: 5{Environment.NewLine}" +
                $"Created on: 2021-04-21";

            var output = new StringWriter();
            Console.SetOut(output);

            var input = new StringReader("");
            Console.SetIn(input);
            //Act
            ShortageUtils.PrintShortagesByRights(shortages, userBob);

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Title_Displays_Shortages()
        {
            //Arrange
            string expectedOutput = $"Shortages created by bob:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Beans{Environment.NewLine}" +
                $"Name: bob{Environment.NewLine}" +
                $"Room: Kitchen{Environment.NewLine}" +
                $"Category: Food{Environment.NewLine}" +
                $"Priority: 5{Environment.NewLine}" +
                $"Created on: 2021-04-21";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userBob, 't', "bean");
            
            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Title_Displays_EmptyShortages()
        {
            //Arrange
            string expectedOutput = $"Shortages created by tom:";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userTom, 't', "Beans");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Date_Displays_Shortages()
        {
            //Arrange
            string expectedOutput = $"Shortages created by bob:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Beans{Environment.NewLine}" +
                $"Name: bob{Environment.NewLine}" +
                $"Room: Kitchen{Environment.NewLine}" +
                $"Category: Food{Environment.NewLine}" +
                $"Priority: 5{Environment.NewLine}" +
                $"Created on: 2021-04-21";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userBob, 'd', "2021-04-21 2021-04-21");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Date_Displays_InvalidInput()
        {
            //Arrange
            string expectedOutput = $"Invalid start/end date input! Please try again.";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userBob, 'd', "2021-04-21 a");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Category_Displays_Shortages()
        {
            //Arrange
            string expectedOutput = $"Shortages created by tom:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Projector{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 8{Environment.NewLine}" +
                $"Created on: 2021-04-22{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Cable{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 6{Environment.NewLine}" +
                $"Created on: 2021-04-20";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userTom, 'c', "Electronics");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Category_Displays_InvalidInput()
        {
            //Arrange
            string expectedOutput = $"This category doesn't exist please try again.";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userTom, 'c', "Electronic");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Room_DisplaysShortages()
        {
            //Arrange
            string expectedOutput = $"Shortages created by tom:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Projector{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 8{Environment.NewLine}" +
                $"Created on: 2021-04-22{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Title: Cable{Environment.NewLine}" +
                $"Name: tom{Environment.NewLine}" +
                $"Room: Meeting room{Environment.NewLine}" +
                $"Category: Electronics{Environment.NewLine}" +
                $"Priority: 6{Environment.NewLine}" +
                $"Created on: 2021-04-20";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userTom, 'r', "Meeting room");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void FilterAndPrintShortagesByFilter_Room_Displays_InvalidInput()
        {
            //Arrange
            string expectedOutput = $"This room doesn't exist please try again.";

            var output = new StringWriter();
            Console.SetOut(output);
            var input = new StringReader("");
            Console.SetIn(input);

            //Act
            ShortageUtils.FilterAndPrintShortagesByFilter(shortages, userTom, 'r', "Meeting");

            //Assert
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ValidateShortage_Returns_Shortage()
        {
            //Arrange
            string input = $"a{Environment.NewLine}Meeting room{Environment.NewLine}Electronics{Environment.NewLine}5{Environment.NewLine}";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeTrue();
            shortage.Should().NotBeNull();
            shortage.Title.Should().Be("a");
            shortage.Name.Should().Be(userTom.Username);
            shortage.Room.Should().Be(Room.MeetingRoom);
            shortage.Category.Should().Be(Category.Electronics);
            shortage.Priority.Should().Be(5);
            shortage.CreatedOn.Should().Be(DateOnly.FromDateTime(DateTime.Today));
        }

        [Fact]
        public void ValidateShortage_Displays_TitleError()
        {
            //Arrange
            string input = $"{Environment.NewLine}Meeting room{Environment.NewLine}Electronics{Environment.NewLine}5{Environment.NewLine}";
            string expectedOutput = $"Create new shortage:{Environment.NewLine}"+
                $"Enter a Title{Environment.NewLine}" +
                $"Please enter a title.";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var output = new StringWriter();
            Console.SetOut(output);
            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeFalse();
            shortage.Should().BeNull();
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ValidateShortage_Displays_RoomError()
        {
            //Arrange
            string input = $"new{Environment.NewLine}Meetin room{Environment.NewLine}";
            string expectedOutput = $"Create new shortage:{Environment.NewLine}" +
                $"Enter a Title{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Room{Environment.NewLine}" +
                $"Invalid room. Please enter a valid room.";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var output = new StringWriter();
            Console.SetOut(output);
            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeFalse();
            shortage.Should().BeNull();
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ValidateShortage_Displays_CategoryError()
        {
            //Arrange
            string input = $"new{Environment.NewLine}Meeting room{Environment.NewLine}Oth";
            string expectedOutput = $"Create new shortage:{Environment.NewLine}" +
                $"Enter a Title{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Room{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Category{Environment.NewLine}" +
                $"Invalid category. Please enter a valid category.";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var output = new StringWriter();
            Console.SetOut(output);
            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeFalse();
            shortage.Should().BeNull();
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ValidateShortage_Displays_LowerPriorityError()
        {
            //Arrange
            string input = $"new{Environment.NewLine}Meeting room{Environment.NewLine}Other{Environment.NewLine}0";
            string expectedOutput = $"Create new shortage:{Environment.NewLine}" +
                $"Enter a Title{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Room{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Category{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Priority 1-10{Environment.NewLine}" +
                $"Invalid priority. Please enter a valid priority between 1 and 10.";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var output = new StringWriter();
            Console.SetOut(output);
            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeFalse();
            shortage.Should().BeNull();
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ValidateShortage_Displays_HigherPriorityError()
        {
            //Arrange
            string input = $"new{Environment.NewLine}Meeting room{Environment.NewLine}Other{Environment.NewLine}11";
            string expectedOutput = $"Create new shortage:{Environment.NewLine}" +
                $"Enter a Title{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Room{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Category{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter a Priority 1-10{Environment.NewLine}" +
                $"Invalid priority. Please enter a valid priority between 1 and 10.";

            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            var output = new StringWriter();
            Console.SetOut(output);
            //Act
            (bool check, Shortage? shortage) = ShortageUtils.ValidateShortage(userTom.Username);

            //Assert
            check.Should().BeFalse();
            shortage.Should().BeNull();
            output.ToString().Trim().Should().Be(expectedOutput);
        }


    }
}
