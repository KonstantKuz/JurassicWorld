using Feofun.DroppingLoot.Model;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

namespace Feofun.DroppingLoot.Tween
{
    public class DroppingTrajectoryTween : MonoBehaviour
    {
        private DroppingObjectTrajectory _trajectory;
        private UnityAction _onCompleteCallback;
        
        private RectTransform _rectTransform;
        private bool _isPlaying;
        private float _time;

        public void Drop(DroppingObjectTrajectory trajectory, [CanBeNull] UnityAction onCompleteCallback)
        {
            _trajectory = trajectory;
            _onCompleteCallback = onCompleteCallback;
            _isPlaying = true;
        }
        private void Start()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
        }
        private void Update()
        {
            UpdatePosition();
        }
        private void UpdatePosition()
        {
            if (!_isPlaying) {
                return;
            }
            _time += Time.deltaTime;

            if (_time < _trajectory.Time) {
                _rectTransform.position = CalcParabolaWithHeight(_trajectory.StartPosition, _trajectory.RemovePosition, _trajectory.Height, _time / _trajectory.Time);
            } else {
                _isPlaying = false;
                _onCompleteCallback?.Invoke();
            }
        }
        private static Vector2 CalcParabolaWithHeight(Vector2 start, Vector2 end, float height, float t)
        {
            return new Vector2(((Vector3) Vector2.Lerp(start, end, t)).x, Func(t) + Mathf.Lerp(start.y, end.y, t));
            float Func(float x) => (float) (-4.0 * height * Mathf.Pow(x, 2f) + 4.0 * height * x);
        }
    }
}