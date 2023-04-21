using System.Text.RegularExpressions;
using VismaShortage.Models;

namespace VismaShortage.Utils
{
    public class ShortageUtils
    {
        public static void PrintShortagesByRights(List<Shortage> shortages, User user)
        {
            if (user.IsAdmin)
            {
                Console.WriteLine("All shortages:");
                Console.WriteLine();
                List<Shortage> sortedShortages = shortages.OrderByDescending(s => s.Priority).ToList();
                InOutUtils.PrintShortages(sortedShortages);
            }
            else
            {
                Console.WriteLine($"Shortages created by {user.Username}:");
                Console.WriteLine();
                List<Shortage> sortedShortages = shortages.Where(s => s.Name == user.Username).OrderByDescending(s => s.Priority).ToList();
                InOutUtils.PrintShortages(sortedShortages);
            }
        }

        public static void FilterAndPrintShortagesByFilter(List<Shortage> shortages, User user, char filter, string filterValue)
        {
            List<Shortage> filteredShortages;
            switch (filter)
            {
                case 't':
                    filteredShortages = shortages.Where(s => Regex.IsMatch(s.Title, filterValue, RegexOptions.IgnoreCase)).ToList();
                    PrintShortagesByRights(filteredShortages, user);
                    break;

                case 'd':
                    string[] dateStrings = filterValue.Split(' ');
                    string startDateString;
                    string endDateString;

                    if (dateStrings.Length == 2)
                    {
                        startDateString = dateStrings[0];
                        endDateString = dateStrings[1];
                    }
                    else
                    {
                        Console.WriteLine("Invalid start/end date input! Please try again.");
                        return;
                    }
                
                    DateOnly startDate, endDate;

                    if (!DateOnly.TryParse(startDateString, out startDate) || !DateOnly.TryParse(endDateString, out endDate))
                    {
                        Console.WriteLine("Invalid start/end date input! Please try again.");
                        return;
                    }

                    filteredShortages = shortages.Where(s => s.CreatedOn >= startDate && s.CreatedOn <= endDate).ToList();
                    PrintShortagesByRights(filteredShortages, user);
                    break;

                case 'c':
                    Category category;

                    if (Enum.TryParse(filterValue, true, out category))
                    {
                        filteredShortages = shortages.Where(s => s.Category == category).ToList();
                        PrintShortagesByRights(filteredShortages, user);

                    }
                    else
                    {
                        Console.WriteLine("This category doesn't exist please try again.");
                        Console.WriteLine();
                    }

                    break;

                case 'r':
                    Room room;
                    string correctedFilterValue = Regex.Replace(filterValue, " room", "Room");

                    if (Enum.TryParse(correctedFilterValue, true, out room))
                    {
                        filteredShortages = shortages.Where(s => s.Room == room).ToList();
                        PrintShortagesByRights(filteredShortages, user);
                    }
                    else
                    {
                        Console.WriteLine("This room doesn't exist please try again.");
                        Console.WriteLine();
                    }

                    break;
            }
        }

        public static (bool, Shortage?) ValidateShortage(string username)
        {
            Console.WriteLine("Create new shortage:");
            Console.WriteLine("Enter a Title");
            string? title = Console.ReadLine();

            if (string.IsNullOrEmpty(title))
            {
                Console.WriteLine("Please enter a title.");
                return (false, null);
            }
            Console.WriteLine();

            Console.WriteLine("Enter a Room");
            string? roomInput = Console.ReadLine();

            if (string.IsNullOrEmpty(roomInput))
            {
                return (false, null);
            }

            string correctedRoomInput = Regex.Replace(roomInput, " room", "Room");

            if (!Enum.TryParse(correctedRoomInput, true, out Room room))
            {
                Console.WriteLine("Invalid room. Please enter a valid room.");
                return (false, null);
            }
            Console.WriteLine();

            Console.WriteLine("Enter a Category");
            string? categoryInput = Console.ReadLine();

            if (!Enum.TryParse(categoryInput, true, out Category category))
            {
                Console.WriteLine("Invalid category. Please enter a valid category.");
                return (false, null);
            }
            Console.WriteLine();

            Console.WriteLine("Enter a Priority 1-10");

            if (!int.TryParse(Console.ReadLine(), out int priority) || priority < 1 || priority > 10)
            {
                Console.WriteLine("Invalid priority. Please enter a valid priority between 1 and 10.");
                Console.WriteLine();
                return (false, null);
            }
            else
            {
                Shortage newShortage = new(title, username, room, category, priority, DateOnly.FromDateTime(DateTime.Now));
                return (true, newShortage);
            }
        }
        public static bool ListAllShortages(User user, string filter)
        {
            List<Shortage>? shortages = InOutUtils.ReadShortages();

            if(shortages == null || !shortages.Any())
            {
                Console.WriteLine("There are no shortages to display.");
                Console.WriteLine();
                return false;
            }

            if (filter == "none")
            {
                PrintShortagesByRights(shortages, user);
            }
            else
            {
                if(filter.Length < 3)
                {
                    Console.WriteLine("Invalid filter value. Please try again.");
                    return false;
                }
                char filterOption = filter[0];
                string filterValue = filter.Substring(2);
                FilterAndPrintShortagesByFilter(shortages, user, filterOption, filterValue);
            }

            return true;
        }
        public static void RegisterNewShortage(Shortage shortage)
        {
            List<Shortage> shortages = InOutUtils.ReadShortages()!;

            if (!shortages.Any())
            {
                shortages.Add(shortage);
                InOutUtils.WriteShortages(shortages);
                Console.WriteLine("Shortage has been registered.");
            }

            bool exists = false;
            int index = -1;
            for (int i = 0; i < shortages.Count; i++)
            {
                if (shortages[i].Title == shortage.Title && shortages[i].Room == shortage.Room)
                {
                    exists = true;
                    if (shortages[i].Priority < shortage.Priority)
                    {
                        index = i;
                    }
                    break;
                }
            }

            if (exists)
            {
                if (index != -1)
                {
                    shortages[index] = shortage;
                    InOutUtils.WriteShortages(shortages);
                    Console.WriteLine("Existing shortage updated with higher priority.");
                }
                else
                {
                    Console.WriteLine("This shortage already exists.");
                }
            }
            else
            {
                shortages.Add(shortage);
                InOutUtils.WriteShortages(shortages);
                Console.WriteLine("Shortage has been registered.");
            }
            Console.WriteLine();
        }

        public static void DeleteShortage(User user, string title, string room)
        {
            List<Shortage> shortages = InOutUtils.ReadShortages()!;
            Room roomToDelete;
            string correctedRoomValue = Regex.Replace(room, " room", "Room");
            
            if (Enum.TryParse(correctedRoomValue, true, out roomToDelete))
            {
                if (user.IsAdmin)
                {
                    if (shortages.Any(s => s.Title == title && s.Room == roomToDelete))
                    {
                        shortages.RemoveAll(s => s.Title == title && s.Room == roomToDelete);
                        InOutUtils.WriteShortages(shortages);
                        Console.WriteLine($"Shortage {title} has been removed");
                    }
                    else
                    {
                        Console.WriteLine("This shortage doesn't exist. Please try again.");
                        Console.WriteLine();
                    }
                }
                else
                {
                    if (shortages.Any(s => s.Title == title && s.Name == user.Username && s.Room == roomToDelete))
                    {
                        shortages.RemoveAll(s => s.Title == title && s.Name == user.Username);
                        InOutUtils.WriteShortages(shortages);
                        Console.WriteLine($"Shortage {title} has been removed");
                    }
                    else
                    {
                        Console.WriteLine("This shortage doesn't exist. Please try again.");
                        Console.WriteLine();
                    }
                }
            }
            else
            {
                Console.WriteLine("This room doesn't exist please try again.");
                Console.WriteLine();
            }
        }
    }
}
