﻿using System;
using System.Collections.Generic;
using System.Linq;
using DinoWorldSurvival.Extension;
using DinoWorldSurvival.Units.Component.TargetSearcher;
using DinoWorldSurvival.Units.Target;
using DinoWorldSurvival.Units.Weapon.Projectiles.Params;
using Feofun.Extension;
using UnityEngine;
using UnityEngine.Assertions;

namespace DinoWorldSurvival.Units.Weapon
{
    public class MultiTargetRangedWeapon: RangedWeapon
    {
        private ITargetSearcher _targetSearcher;

        private void Awake()
        {
            _targetSearcher = gameObject.RequireComponentInParent<ITargetSearcher>();
        }

        public override void Fire(ITarget firstTarget, IProjectileParams projectileParams, Action<GameObject> hitCallback)
        {
            Assert.IsNotNull(projectileParams);
            var targets = FindAdditionalTargets(firstTarget, projectileParams.Count, projectileParams.DamageRadius);
            var singleShotParams = new ProjectileParams
            {
                Count = 1,
                Speed = projectileParams.Speed,
                DamageRadius = projectileParams.DamageRadius,
                AttackDistance = projectileParams.AttackDistance
            };
            foreach (var target in targets)
            {
                base.Fire(target, singleShotParams, hitCallback);
            }
        }

        private List<ITarget> FindAdditionalTargets(ITarget initialTarget, int targetCount, float minDistanceBetweenTargets)
        {
            var selectedTargets = new List<ITarget> { initialTarget };            
            var possibleTargets = _targetSearcher
                .GetAllOrderedByDistance()
                .Where(it => it != null && it.IsAlive)
                .Except(selectedTargets)                
                .ToList();
            
            SelectDistinctTargets(selectedTargets, targetCount - selectedTargets.Count, minDistanceBetweenTargets, possibleTargets);
            SelectRandomTargets(selectedTargets, targetCount - selectedTargets.Count, possibleTargets);
            return selectedTargets;
        }

        private static void SelectRandomTargets(ICollection<ITarget> selectedTargets, int countToSelect, List<ITarget> possibleTargets)
        {
            for (int i = 0; i < countToSelect; i++)
            {
                if (possibleTargets.Count == 0) return;
                var newTarget = possibleTargets.Random();
                if (newTarget == null) return;
                
                possibleTargets.Remove(newTarget);
                selectedTargets.Add(newTarget);
            }
        }

        private static void SelectDistinctTargets(ICollection<ITarget> selectedTargets, int countToSelect, float minDistanceBetweenTargets, ICollection<ITarget> possibleTargets)
        {
            for (int i = 0; i < countToSelect; i++) {
                var newTarget = possibleTargets
                    .FirstOrDefault(it =>
                        Vector3.Distance(selectedTargets.Last().Center.position, it.Center.position) >= minDistanceBetweenTargets);
                if (newTarget == null) return;
                
                possibleTargets.Remove(newTarget);
                selectedTargets.Add(newTarget);
            }
        }
    }
}