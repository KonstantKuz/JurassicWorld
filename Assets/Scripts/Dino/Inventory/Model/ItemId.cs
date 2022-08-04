using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Dino.Inventory.Model
{
    public class ItemId : IEquatable<ItemId>
    {
        private static readonly Regex Regex = new Regex(@"(\D+)(\d+)");
        public string FullName { get; }
        public int Number { get; }
        public string Name { get; }
        public int Rank { get; }
        
        public ItemId(string fullName, int number)
        {
            FullName = fullName;
            Number = number;
            var (name, rank) = SplitFullNameToNameAndRank(fullName);
            Name = name;
            Rank = rank;
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
            return FullName == other.FullName && Number == other.Number;
        }

        public override string ToString()
        {
            return $"InventoryItem: Id:= {FullName}, ObjectId:= {Name}, Rank:= {Rank} SerialNumber:= {Number}";
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
                return ((FullName != null ? FullName.GetHashCode() : 0) * 397) ^ Number;
            }
        }
        
        private static (string, int) SplitFullNameToNameAndRank(string fullName)
        {
            var matchObj = Regex.Match(fullName);
            if (!matchObj.Success) return (fullName, 0);
            return (matchObj.Groups[1].Captures[0].Value, int.Parse(matchObj.Groups[2].Captures[0].Value));
        }
    }
}