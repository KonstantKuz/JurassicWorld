using System;
using System.Text.RegularExpressions;

namespace Dino.Inventory.Model
{
    public class ItemId : IEquatable<ItemId>
    {
        private static Regex _regex = new Regex(@"(\D+)(\d+)");
        public string Name { get; }
        public int Number { get; }
        
        public ItemId(string name, int number)
        {
            Name = name;
            Number = number;
        }

        public static ItemId Create(string name, int number)
        {
            return new ItemId(name, number);
        }
        public bool Equals(ItemId other)
        {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Name == other.Name && Number == other.Number;
        }

        public override string ToString()
        {
            return $"InventoryItem: Id:= {Name}, Number:= {Number}";
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            if (ReferenceEquals(this, obj)) {
                return true;
            }
            if (obj.GetType() != this.GetType()) {
                return false;
            }
            return Equals((ItemId) obj);
        }

        public override int GetHashCode()
        {
            unchecked {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Number;
            }
        }
        
        public (string, int) GetNameAndRank()
        {
            var matchObj = _regex.Match(Name);
            if (!matchObj.Success) return (Name, 0);
            return (matchObj.Groups[1].Captures[0].Value, int.Parse(matchObj.Groups[2].Captures[0].Value));
        }
    }
}