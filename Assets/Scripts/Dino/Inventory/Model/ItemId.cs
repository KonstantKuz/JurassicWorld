using System;
using System.Text.RegularExpressions;

namespace Dino.Inventory.Model
{
    public class ItemId : IEquatable<ItemId>
    {
        private static Regex _regex = new Regex(@"(\D+)(\d+)");
        public string FullName { get; }
        public int Count { get; }
        
        public ItemId(string fullName, int count)
        {
            FullName = fullName;
            Count = count;
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
            return FullName == other.FullName && Count == other.Count;
        }

        public override string ToString()
        {
            return $"InventoryItem: Id:= {FullName}, Number:= {Count}";
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
                return ((FullName != null ? FullName.GetHashCode() : 0) * 397) ^ Count;
            }
        }
        
        public (string, int) GetNameAndRank()
        {
            var matchObj = _regex.Match(FullName);
            if (!matchObj.Success) return (FullName, 0);
            return (matchObj.Groups[1].Captures[0].Value, int.Parse(matchObj.Groups[2].Captures[0].Value));
        }
    }
}