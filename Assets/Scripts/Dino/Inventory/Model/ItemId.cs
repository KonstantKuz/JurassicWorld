using System;
using System.Text.RegularExpressions;
using Logger.Extension;
using UnityEngine.Assertions;

namespace Dino.Inventory.Model
{
    public class ItemId : IEquatable<ItemId>
    {
        private static readonly Regex Regex = new Regex(@"(\D+)(\d+)");
        
        public string FullName { get; } 
        public int Amount { get; private set; }
        public string Name { get; }
        public int Rank { get; }
        public InventoryItemType Type { get; }

        public ItemId(string fullName, InventoryItemType type, int amount)
        {
            Assert.IsTrue(amount >= 0, "Error creating item, should add non-negative amount items");
            FullName = fullName;
            Amount = amount;
            Type = type;
            var (name, rank) = SplitFullNameToNameAndRank(fullName);
            Name = name;
            Rank = rank;
        }

        public static ItemId Create(string fullName, InventoryItemType type, int amount = 1)
        {
            return new ItemId(fullName, type, amount);
        }
        public void IncreaseAmount(int amount)
        {
            Assert.IsTrue(amount >= 0, $"Should add non-negative amount items, {ToString()}");
            ChangeAmount(amount);
        } 
        public void DecreaseAmount(int amount)
        {
            Assert.IsTrue(amount >= 0, $"Should remove non-negative amount items, {ToString()}");
            ChangeAmount(-amount);
        }
        private void ChangeAmount(int delta)
        {
            if (Amount + delta < 0) {
                this.Logger().Warn($"Amount of imtems cannot be negative, change delta:= {delta}, {ToString()}");
                Amount = 0;
                return;
            }
            Amount += delta;
        }
        public bool Equals(ItemId other)
        {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return FullName == other.FullName;
        }

        public bool IsSameItem(ItemId other)
        {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Name == other.Name;
        }

        public override string ToString()
        {
            return $"InventoryItem: Id:= {FullName}, ObjectId:= {Name}, Rank:= {Rank}, Amount:= {Amount}, Type:= {Type}";
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
            return (FullName != null ? FullName.GetHashCode() : 0);
        }

        public static (string, int) SplitFullNameToNameAndRank(string fullName)
        {
            var matchObj = Regex.Match(fullName);
            if (!matchObj.Success) return (fullName, 0);
            return (matchObj.Groups[1].Captures[0].Value, int.Parse(matchObj.Groups[2].Captures[0].Value));
        }
    }
}