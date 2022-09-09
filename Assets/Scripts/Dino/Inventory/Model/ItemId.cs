using System;

namespace Dino.Inventory.Model
{
    public readonly struct ItemId : IEquatable<ItemId>
    {
        public string FullName { get; }

        public ItemId(string fullName)
        {
            FullName = fullName;
        }

        public static ItemId Create(string fullName)
        {
            return new ItemId(fullName);
        }

        public bool Equals(ItemId other)
        {
            return FullName == other.FullName;
        }
        public override bool Equals(object obj)
        {
            return obj is ItemId other && Equals(other);
        }
        public override int GetHashCode()
        {
            return (FullName != null ? FullName.GetHashCode() : 0);
        }
    }
}