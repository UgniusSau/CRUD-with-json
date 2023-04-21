using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaShortage.Models;

namespace VismaShortage.Utils
{
    public class UserUtils
    {
        public static User ValidateUser(List<User> users)
        {
            while (true)
            {
                Console.WriteLine("Enter your username:");
                string? username = Console.ReadLine();
                Console.WriteLine("Enter your password:");
                string? password = Console.ReadLine();
                Console.WriteLine();

                foreach (User user in users!)
                {
                    if (user.Username == username && user.Password == password)
                    {
                        Console.Write("Login successful!");
                        if (user.IsAdmin)
                        {
                            Console.Write(" You are an administrator.");
                        }
                        else
                        {
                            Console.Write(" You are a regular user.");
                        }
                        return user;
                    }
                }
                Console.WriteLine("Invalid username or password.");

                Console.WriteLine();
            }
        }
    }
}
