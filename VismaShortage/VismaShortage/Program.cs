using VismaShortage.Models;
using VismaShortage.Utils;

namespace VismaShortage
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<User>? users = InOutUtils.ReadUsers();

            if(users == null || !users.Any())
            {
                Console.WriteLine("Please fill in users.json");
                return;
            }

            Console.WriteLine("Welcome to Visma Resource Shortage Management system!");
            User user = UserUtils.ValidateUser(users);
            UserInputUtils.HandleUserInputs(user);
        }
    }
}