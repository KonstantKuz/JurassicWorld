using DG.Tweening;
using Feofun.Extension;
using Logger.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Component.DamageReaction
{
    public class KickbackReaction : MonoBehaviour
    {
        private Unit _owner;
        private Tween _kickBack;

        private void Awake()
        {
            _owner = gameObject.RequireComponent<Unit>();
        }

        public static void TryExecuteOn(GameObject target, Vector3 direction, KickbackReactionParams reactionParams)
        {
            if (!target.TryGetComponent(out KickbackReaction kickbackReaction)) { return; }

            kickbackReaction.OnKickback(direction, reactionParams);
        }

        private void OnKickback(Vector3 direction, KickbackReactionParams reactionParams)
        {
            if(gameObject == null) { return; } 
            if(!_owner.IsActive) { return; }

            var resultPosition = transform.position + direction * reactionParams.Distance;
            if (!NavMesh.SamplePosition(resultPosition, out var navMeshHit, reactionParams.Distance, NavMesh.AllAreas))
            {
                this.Logger().Warn("Can't find proper place for kickback. ");
                return;
            }
            
            resultPosition = navMeshHit.position;
            _kickBack = transform.DOMove(resultPosition, reactionParams.Duration).SetEase(Ease.Linear);

            _owner.IsActive = false;
            _kickBack.onComplete = () => { _owner.IsActive = true; };
        }

        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            _kickBack?.Kill(true); 
        }
    }
}