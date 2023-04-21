using Newtonsoft.Json;
using VismaShortage.Models;
using VismaShortage.Utils;
using FluentAssertions;

namespace VismaShortage.Tests.Unit
{
    [Collection("UserUtilsTests")]
    public class UserUtilsTests
    {
        //testing data
        List<User> users = new List<User>
        {
            new User("user2", "password2", false),
            new User("admin", "password3", true),
            new User("user1", "password1", false)
        };

        [Fact]
        public void ReadUsers_CorrectCredentials_Displays_ValidationOfUser()
        {
            // Arrange
            string input = $"user1{Environment.NewLine}password1{Environment.NewLine}";
            string expectedOutput = $"Enter your username:{Environment.NewLine}" +
                $"Enter your password:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                "Login successful! You are a regular user.";

            // Redirect standard output
            var output = new StringWriter();
            Console.SetOut(output);

            // Redirect standard input
            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            // Act
            User result = UserUtils.ValidateUser(users);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("user1");
            output.ToString().Trim().Should().Be(expectedOutput);
        }

        [Fact]
        public void ReadUsers_WrongThanRightCredentials_Displays_ValidationOfUser()
        {
            string input = $"admin{Environment.NewLine}wrongpassword{Environment.NewLine}admin{Environment.NewLine}password3{Environment.NewLine}";
            string expectedOutput = $"Enter your username:{Environment.NewLine}" +
                $"Enter your password:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Invalid username or password.{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Enter your username:{Environment.NewLine}" +
                $"Enter your password:{Environment.NewLine}" +
                $"{Environment.NewLine}" +
                $"Login successful! You are an administrator.";

            // Redirect standard output
            var output = new StringWriter();
            Console.SetOut(output);

            // Redirect standard input
            var inputReader = new StringReader(input);
            Console.SetIn(inputReader);

            // Act
            User result = UserUtils.ValidateUser(users);

            // Assert
            result.Should().NotBeNull();
            result.Username.Should().Be("admin");
            output.ToString().Trim().Should().Be(expectedOutput);
        }
    }
}