using System;
using UnityEngine;

namespace Input
{
    public class SwipeDetection : MonoBehaviour
    {
        [SerializeField] private float minimumDistance = 0.2f;
        [SerializeField] private float maximumTime = 1f;
        [SerializeField, Range(0f,1f)] private float _directionThreshold = 0.9f;
        private TouchInputManager _touchInputManager;
        private Vector2 _startPosition = Vector2.zero;
        private float _startTime;
        private Vector2 _endPosition = Vector2.zero;
        private float _endTime;

        private void Awake()
        {
            _touchInputManager = TouchInputManager.Instance;
        }

        private void OnEnable()
        {
            _touchInputManager.OnStartTouch += SwipeStart;
            _touchInputManager.OnEndTouch += SwipeEnd;
        }

        private void OnDisable()
        {
            _touchInputManager.OnStartTouch -= SwipeStart;
            _touchInputManager.OnEndTouch -= SwipeEnd;
        }

        private void SwipeStart(Vector2 position, float time)
        {
            _startPosition = position;
            _startTime = time;
        }
        
        private void SwipeEnd(Vector2 position, float time)
        {
            _endPosition = position;
            _endTime = time;
            DetectSwipe();
        }

        private void DetectSwipe()
        {
            if(Vector3.Distance(_startPosition, _endPosition) >= minimumDistance &&
               _endTime - _startTime <= maximumTime)
            {
                Debug.DrawLine(_startPosition, _endPosition, Color.red, 5);
            }
        }

        private void DetectSwipeDirection()
        {
            Vector3 direction = _endPosition - _startPosition;
            Vector2 direction2D = new Vector2(direction.x, direction.y).normalized;
            SwipeDirection(direction2D);
        }

        public Vector2 GetTouchPosition()
        {
            return _touchInputManager.GetPrimaryPosition();
        }

        public Vector2 GetSwipeStartPosition()
        {
            return _startPosition;
        }

        private void SwipeDirection(Vector2 direction)
        {
            if (Vector2.Dot(Vector2.up, direction) > _directionThreshold)
            {
                Debug.Log("Swipe Up");
            }
            else if (Vector2.Dot(Vector2.down, direction) > _directionThreshold)
            {
                Debug.Log("Swipe Down");
            }
            else if (Vector2.Dot(Vector2.right, direction) > _directionThreshold)
            {
                Debug.Log("Swipe Right");
            }
            else if (Vector2.Dot(Vector2.left, direction) > _directionThreshold)
            {
                Debug.Log("Swipe Left");
            }
        }
    }
}
