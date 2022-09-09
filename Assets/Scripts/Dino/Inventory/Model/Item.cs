using System.Text.RegularExpressions;
using Logger.Extension;
using UnityEngine.Assertions;

namespace Dino.Inventory.Model
{
    public class Item
    {
        private static readonly Regex Regex = new Regex(@"(\D+)(\d+)");

        public readonly ItemId Id;
        public readonly string Name;
        public readonly int Rank;
        public readonly InventoryItemType Type;
        public int Amount { get; private set; }
 

        public Item(ItemId id, InventoryItemType type, int amount)
        {
            Assert.IsTrue(amount >= 0, "Error creating item, should add non-negative amount items");
            Amount = amount;
            Type = type;
            var (name, rank) = SplitFullNameToNameAndRank(id.FullName);
            Name = name;
            Rank = rank;
        }

        public static Item Create(ItemId itemId, InventoryItemType type, int amount = 1)
        {
            return new Item(itemId, type, amount);
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
        public bool IsSameItem(Item other)
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
            return $"InventoryItem: Id:= {Id}, ObjectId:= {Name}, Rank:= {Rank}, Amount:= {Amount}, Type:= {Type}";
        }
        public static (string, int) SplitFullNameToNameAndRank(string fullName)
        {
            var matchObj = Regex.Match(fullName);
            if (!matchObj.Success) return (fullName, 0);
            return (matchObj.Groups[1].Captures[0].Value, int.Parse(matchObj.Groups[2].Captures[0].Value));
        }
    }
}