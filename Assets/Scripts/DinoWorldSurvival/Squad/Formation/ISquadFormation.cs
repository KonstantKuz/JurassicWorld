using UnityEngine;

namespace DinoWorldSurvival.Squad.Formation
{
    public interface ISquadFormation
    {
        Vector3 GetUnitOffset(int unitIdx, float unitRadius, int unitsCount);
    }
}