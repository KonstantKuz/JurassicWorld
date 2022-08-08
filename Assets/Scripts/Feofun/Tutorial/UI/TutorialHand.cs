using System;
using DG.Tweening;
using UnityEngine;

namespace Feofun.Tutorial.UI
{
    public enum HandDirection
    {
        Up,
        Down,
        Right,
        Left,
    }
    public class TutorialHand : MonoBehaviour
    {
        private Animator _animator;
        private Transform _target;
        private Tween _dragMove;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
        }

        public void ShowOnElement(Component target, HandDirection direction = HandDirection.Up) =>
            ShowOnElement(target.transform, direction);

        public void ShowOnElement(Transform target, HandDirection direction = HandDirection.Up)
        {
            gameObject.SetActive(true);
            Attach(target, direction);
            _animator.Play("PressButton");
        }

        public void ShowPressingFingerOnObject(Transform target)
        {
            Detach();
            gameObject.SetActive(true);
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
            _animator.Play("PressAndHold");
        }

        public void ShowDrag(Transform target)
        {
            Detach();
            gameObject.SetActive(true);
            transform.position = Camera.main.WorldToScreenPoint(target.transform.position);
        }

        public Tween ShowDrag(Transform from, Transform to, float time = 0.5f)
        {
            Detach();
            gameObject.SetActive(true);
            var fromPos = Camera.main.WorldToScreenPoint(from.position);
            var toPos = Camera.main.WorldToScreenPoint(to.position);
            transform.position = fromPos;
            _dragMove = transform.DOMove(toPos, time);
            _dragMove.SetUpdate(true);
            _dragMove.onComplete += Hide;
            _dragMove.onComplete += () => _dragMove = null;
            return _dragMove;
        }

        public Tween ShowDragUI(RectTransform from, RectTransform to, float time = 0.5f)
        {
            Detach();
            gameObject.SetActive(true);
            var fromPos = from.TransformPoint(from.rect.center);
            var toPos = to.TransformPoint(to.rect.center);
            transform.position = fromPos;
            _dragMove = transform.DOMove(toPos, time);
            _dragMove.SetUpdate(true);
            _dragMove.onComplete += Hide;
            _dragMove.onComplete += () => _dragMove = null;
            return _dragMove;
        }

        public void Hide()
        {
            Detach();
            gameObject.SetActive(false);
        }

        private void Attach(Transform target, HandDirection direction)
        {
            _target = target;
            UpdatePos();
            SetDirection(direction);
        }

        private void Detach()
        {
            _target = null;
            transform.localPosition = Vector3.zero;
            transform.eulerAngles = Vector3.zero;
            _dragMove?.Kill();
            _dragMove = null;
        }

        private void Update()
        {
            UpdatePos();
        }

        private void UpdatePos()
        {
            if (_target == null) {
                return;
            }
            transform.position = _target.position;
        }

        private void SetDirection(HandDirection direction)
        {
            transform.eulerAngles = direction switch {
                    HandDirection.Up => new Vector3(0, 0, 0),
                    HandDirection.Down => new Vector3(0, 0, 180),
                    HandDirection.Left => new Vector3(0, 0, 90),
                    HandDirection.Right => new Vector3(0, 0, -90),
                    _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }
    }
}