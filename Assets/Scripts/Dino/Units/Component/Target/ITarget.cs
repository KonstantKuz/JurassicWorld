using System;
using UnityEngine;

namespace Dino.Units.Component.Target
{
    /// <summary>
    /// Для сущностей которые могут использоваться в качестве целей для атак, перемещения
    /// </summary>
    public interface ITarget
    {
        string TargetId { get; }
        UnitType UnitType { get; }
        bool IsValid { get; }
        Transform Root { get; }
        Transform Center { get; } 
        Action OnTargetInvalid { get; set; }
    }
}