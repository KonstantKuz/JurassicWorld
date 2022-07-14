using Dino.Location;
using Dino.Units.Enemy.Model;
using UnityEngine;
using UnityEngine.Assertions;
using Zenject;

namespace Dino.Units.StateMachine
{
    public partial class EnemyStateMachine : UnitStateMachine
    {
        private EnemyBehaviourModel _behaviourModel;
        private PatrolPath _patrolPath;

        [Inject] protected World _world;

        public override void Init(IUnit unit)
        {
            base.Init(unit);
            
            var enemyModel = (EnemyUnitModel) _owner.Model;
            Assert.IsTrue(enemyModel != null, "Unit model must be EnemyUnitModel");
            _behaviourModel = enemyModel.EnemyBehaviourModel;
            Target = _world.Player.SelfTarget;
                
            unit.OnDeath += OnDeath;
            
            InitPatrolPath();
            SetState(new PatrolState(this));
        }

        private void InitPatrolPath()
        {
            var patrolPaths = _world.GetChildrenComponents<PatrolPath>();
            var minDistance = Mathf.Infinity;
            foreach (var path in patrolPaths)
            {
                if (path.IsBusy || path.Path.Length == 0)
                {
                    continue;
                }
                
                var distance = Vector3.Distance(transform.position, path.Path[0].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    _patrolPath = path;
                }
            }

            _patrolPath.IsBusy = true;
        }
    }

}