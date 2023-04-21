using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaShortage.Models;

namespace VismaShortage.Utils
{
    public class UserInputUtils
    {
        public static void HandleUserInputs(User user)
        {
            while (true)
            {
                Console.WriteLine("==========================");
                Console.WriteLine("Please select an option:");
                Console.WriteLine("[1] Register a new shortage");
                Console.WriteLine("[2] Delete a shortage");
                Console.WriteLine("[3] List all shortages");
                Console.WriteLine("[4] Exit");
                Console.WriteLine("==========================");
                Console.WriteLine();

                string? userOptionInput = Console.ReadLine();
                Console.WriteLine();

                switch (userOptionInput)
                {
                    case "1":
                        bool shortageIsValid = false;
                        Shortage? shortage = null;

                        while (!shortageIsValid)
                        {
                            (shortageIsValid, shortage) = ShortageUtils.ValidateShortage(user.Username);
                        }

                        ShortageUtils.RegisterNewShortage(shortage!);
                        break;

                    case "2":
                        Console.WriteLine("Shortages for deletion:");
                        Console.WriteLine();
                        bool shortageExist = ShortageUtils.ListAllShortages(user, "none");

                        if (!shortageExist)
                        {
                            break;
                        }

                        Console.WriteLine("Enter shortage title for deletion");
                        string? userTitleInput = Console.ReadLine();
                        Console.WriteLine("Enter shortage room for deletion");
                        string? userRoomInput = Console.ReadLine();
                        Console.WriteLine();

                        if (string.IsNullOrEmpty(userTitleInput) || string.IsNullOrEmpty(userRoomInput))
                        {
                            Console.WriteLine("No shortage has been selected.");
                        }
                        else
                        {
                            ShortageUtils.DeleteShortage(user, userTitleInput, userRoomInput);
                        }
                        break;

                    case "3":
                        Console.WriteLine("Select option for filter:");
                        Console.WriteLine("[t (title)] Filter by Title");
                        Console.WriteLine("[d (start-date end-date)] Filter by Date");
                        Console.WriteLine("[c (category name)] Filter by Category");
                        Console.WriteLine("[r (room name)] Filter by Title");
                        Console.WriteLine("[Press Enter] No filter");
                        Console.WriteLine();
                        string? userFilterInput = Console.ReadLine();
                        Console.WriteLine();

                        if (string.IsNullOrEmpty(userFilterInput))
                        {
                            ShortageUtils.ListAllShortages(user, "none");
                        }
                        else
                        {
                            ShortageUtils.ListAllShortages(user, userFilterInput);
                        }
                        break;

                    case "4":
                        Console.WriteLine("Thank you for using Visma Resource Shortage Management system!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }
    }
}
