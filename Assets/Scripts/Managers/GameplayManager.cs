using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Dot;
using Input;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform gridParent;
        
        public Action OnGridInitialized;
        public Dictionary<int, int> GamePointsData;
        
        private Dictionary<int, NumberDot> _gridDots;
        private List<NumberDot> _dotsList = new();
        private List<NumberDot> _highlightedDots = new();
        
        private List<int> _possibleSpawnValues = new();
        private TouchInputManager _touchInputManager;
        
        private int _currentDotPosition;
        private int _currentDotValue;
        private int _previousDotPosition;
        private int _previousDotValue;

        protected override void Awake()
        {
            base.Awake();
            _touchInputManager = TouchInputManager.Instance;
            Initialize();
        }

        private void Start()
        {
        }

        private void Initialize()
        {
            GamePointsData = new Dictionary<int, int>();
            _gridDots = new Dictionary<int, NumberDot>();
            _dotsList = new List<NumberDot>();
            _possibleSpawnValues.AddRange(DataConstants.Instance.DotStartingValues);
            GatherGridData();
            
        }

        private void GatherGridData()
        {
            List<NumberDot> dots = gridParent.GetComponentsInChildren<NumberDot>().ToList();
            for (int i = 0; i < dots.Count; i++)
            {
                NumberDot currentDot = dots[i];
                int value = GetNextDotValue();
                currentDot.SetValue(value);
                currentDot.gameObject.GetComponent<SpriteRenderer>().color = GetNumberValueColor(value);
                currentDot.SetPosition(i);

                _dotsList.Add(dots[i]);
                GamePointsData.Add(_dotsList.IndexOf(currentDot), value);
                _gridDots.Add(_dotsList.IndexOf(currentDot), currentDot);
            }
            OnGridInitialized?.Invoke();
        }

        private bool IsPositionBelowDotEmpty(int dotPosition)
        {
            if(dotPosition < 0 || dotPosition >= _dotsList.Count) return false;
            Debug.Log("Is it empty" + (GamePointsData[dotPosition + DataConstants.Instance.GridSize] == 0).ToString());
            return GamePointsData[dotPosition + DataConstants.Instance.GridSize] == 0;
        }
        
        //called recursively to move the dot to the bottom of the grid
        private void MoveDotToNextAvailableColumn(int dotPosition)
        {
            if (dotPosition < 0 || dotPosition >= _dotsList.Count) return;
            if (IsPositionBelowDotEmpty(dotPosition))
            {
                int nextPosition = dotPosition + DataConstants.Instance.GridSize;
                GamePointsData[nextPosition] = GamePointsData[dotPosition];

            }
        }

        private void PopDot(int dotPosition)
        {
            GamePointsData[dotPosition] = 0;
            
            //TODO: Play Effects and remove number dot
        }

        private int GetNextDotValue()
        {
            if (_possibleSpawnValues.Count == 0)
            {
                Debug.LogWarning("No possible values to spawn");
                return DataConstants.Instance.DefaultDotSpawnValue;
            }
            
            int randomIndex = Random.Range(0, _possibleSpawnValues.Count);
            return _possibleSpawnValues[randomIndex];
        }

        private void RespondToTouchInput()
        {
            if (_touchInputManager.IsTouching)
            {
                StartCoroutine(nameof(HighlightSwipedDots));
            }
        }
        
        private IEnumerator HighlightSwipedDots()
        {
            _highlightedDots.Clear();
            while (_touchInputManager.IsTouching)
            {
                Vector2 touchPos = _touchInputManager.GetPrimaryPosition();
                RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
                if (hit.collider != null)
                {
                    NumberDot dot = hit.collider.GetComponent<NumberDot>();


                    _currentDotPosition = dot.GetPosition();
                    _currentDotValue = dot.GetValue();


                    
                    //highlight the dot if the list has only one element or their position is adjacent
                    if (_highlightedDots.Count == 0 || HasValidNeighbour(_currentDotValue, _previousDotValue))
                    {
                        Debug.Log(_highlightedDots);
                        if (_highlightedDots.Count > 1 && dot == _highlightedDots[_highlightedDots.Count - 2])
                        {
                            // backtracking to the second-last dot
                            _highlightedDots.Last().HighlightDot(false);
                            _highlightedDots.RemoveAt(_highlightedDots.Count - 1);
                            
                            _previousDotPosition = _highlightedDots.Last().GetPosition();
                            _previousDotValue = _highlightedDots.Last().GetValue();
                            yield return null;
                        }
                        else if (!_highlightedDots.Contains(dot))
                        {
                            // Normal forward highlighting logic here
                            dot.HighlightDot(_touchInputManager.IsTouching);
                            _highlightedDots.Add(dot);
                            _previousDotPosition = _currentDotPosition;
                            _previousDotValue = _currentDotValue;
                        }
                    }
                }
                yield return null;
            }
        }

        private bool HasValidNeighbour(int currentDotValue, int previousDotValue)
        {
            return currentDotValue == previousDotValue &&
                   GridUtils.CheckDotIsNeighbour(_currentDotPosition, _previousDotPosition);
        }

        private void TouchInputEnded()
        { 
            //Debug.Log("Touch input ended");
        }
        
        private void OnEnable()
        {
            OnGridInitialized += InitializeFinished;
            _touchInputManager.OnStartTouchInput += RespondToTouchInput;
            _touchInputManager.OnEndTouchInput += TouchInputEnded;
        }

        private void OnDisable()
        {
            OnGridInitialized -= InitializeFinished;
            _touchInputManager.OnStartTouchInput -= RespondToTouchInput;
            _touchInputManager.OnEndTouchInput -= TouchInputEnded;
        }

        private void InitializeFinished()
        {
            //Debug.Log(_gridDotPositions);
        }
        
        private Color GetNumberValueColor(int value)
        {
            //return color based on value
            int index = (int)Mathf.Log(value) % DataConstants.Instance.ColorGradient.Count;
            return DataConstants.Instance.ColorGradient[index];
        }

    }
}
