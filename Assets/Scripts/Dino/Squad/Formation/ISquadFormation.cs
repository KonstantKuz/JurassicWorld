using UnityEngine;

namespace Dino.Squad.Formation
{
    public interface ISquadFormation
    {
        Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount);
    }
}