using System;

namespace Dino.Inventory.Model
{
    public class InventoryItem : IEquatable<InventoryItem>
    {
        public string Id { get; }
        public int Number { get; }
        public InventoryItem(string id, int number)
        {
            Id = id;
            Number = number;
        }
        
        public bool Equals(InventoryItem other)
        {
            if (ReferenceEquals(null, other)) {
                return false;
            }
            if (ReferenceEquals(this, other)) {
                return true;
            }
            return Id == other.Id && Number == other.Number;
        }

        public override string ToString()
        {
            return $"InventoryItem: Id:= {Id}, Number:= {Number}";
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
            return Equals((InventoryItem) obj);
        }

        public override int GetHashCode()
        {
            unchecked {
                return ((Id != null ? Id.GetHashCode() : 0) * 397) ^ Number;
            }
        }
    }
}