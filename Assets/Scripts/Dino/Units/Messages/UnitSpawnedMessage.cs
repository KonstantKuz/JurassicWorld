namespace Dino.Units.Messages
{
    public readonly struct UnitSpawnedMessage
    {
        public readonly Unit Unit;

        public UnitSpawnedMessage(Unit unit)
        {
            Unit = unit;
        }
    }
}