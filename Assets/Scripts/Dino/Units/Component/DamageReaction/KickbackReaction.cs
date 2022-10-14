using DG.Tweening;
using Feofun.Extension;
using Logger.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Dino.Units.Component.DamageReaction
{
    public class KickbackReaction : MonoBehaviour
    {
        private Tween _kickBack;

        public static void TryExecuteOn(GameObject target, Vector3 direction, KickbackReactionParams reactionParams)
        {
            if (!target.TryGetComponent(out KickbackReaction kickbackReaction)) { return; }

            kickbackReaction.OnKickback(direction, reactionParams);
        }

        private void OnKickback(Vector3 direction, KickbackReactionParams reactionParams)
        {
            if(gameObject == null) { return; } 

            var resultPosition = transform.position + direction * reactionParams.Distance;
            if (!NavMesh.SamplePosition(resultPosition, out var navMeshHit, reactionParams.Distance, NavMesh.AllAreas))
            {
                this.Logger().Warn("Can't find proper place for kickback. ");
                return;
            }
            
            resultPosition = navMeshHit.position;
            _kickBack = transform.DOMove(resultPosition, reactionParams.Duration).SetEase(Ease.Linear);
        }

        private void OnDestroy() => Dispose();

        private void Dispose()
        {
            _kickBack?.Kill(true); 
        }
    }
}