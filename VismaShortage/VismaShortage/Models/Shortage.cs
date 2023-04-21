using System;
using System.Collections.Generic;
using System.Linq;
namespace VismaShortage.Models
{
    public record Shortage(string Title, string Name, Room Room, Category Category, int Priority, DateOnly CreatedOn);
}
