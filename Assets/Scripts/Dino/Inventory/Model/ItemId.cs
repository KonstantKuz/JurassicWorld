using System;

namespace Dino.Inventory.Model
{
    public class ItemId : IEquatable<ItemId>
    {
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
    }
}