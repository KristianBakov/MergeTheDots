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
        [SerializeField] public float DotMoveDuration = 0.2f;
        
        public Action OnGridInitialized;
        public Dictionary<int, int> GamePointsData;
        
        private Dictionary<int, NumberDot> _gridDots;
        private readonly Dictionary<int, Vector2> _dotPositions = new();
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
        

        private void Initialize()
        {
            GamePointsData = new Dictionary<int, int>();
            _gridDots = new Dictionary<int, NumberDot>();
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
                currentDot.SetColor(GetNumberValueColor(value));
                currentDot.SetPosition(i);
                
                GamePointsData.Add(i, value);
                _gridDots.Add(i, currentDot);
                _dotPositions.Add(i, currentDot.transform.position);
            }
            OnGridInitialized?.Invoke();
        }

        // private bool IsPositionBelowDotEmpty(int dotPosition)
        // {
        //     if(dotPosition < 0 || dotPosition >= _dotsList.Count) return false;
        //     Debug.Log("Is it empty" + (GamePointsData[dotPosition + DataConstants.Instance.GridSize] == 0).ToString());
        //     return GamePointsData[dotPosition + DataConstants.Instance.GridSize] == 0;
        // }
        //
        // //called recursively to move the dot to the bottom of the grid
        // private void MoveDotToNextAvailableSlot(int dotPosition)
        // {
        //     if (dotPosition < 0 || dotPosition >= _dotsList.Count) return;
        //     if (IsPositionBelowDotEmpty(dotPosition))
        //     {
        //         int nextPosition = dotPosition + DataConstants.Instance.GridSize;
        //         GamePointsData[nextPosition] = GamePointsData[dotPosition];
        //
        //     }
        //}

        public void SpawnDotAtPosition(int position)
        {
            if(_gridDots.ContainsKey(position)) return;
            
            Vector2 dotSpawnWorldPosition = _dotPositions[position];
            GameObject newDot = Instantiate(dotPrefab, dotSpawnWorldPosition, Quaternion.identity, gridParent);
            NumberDot newDotComponent = newDot.GetComponent<NumberDot>();
            int value = GetNextDotValue();
            newDotComponent.SetValue(value);
            newDotComponent.SetColor(GetNumberValueColor(value));
            newDotComponent.SetPosition(position);
            _gridDots.Add(position, newDotComponent);
            
            //GamePointsData.Add(position, value);
        }

        private void PopDot(int dotPosition)
        {
            //GamePointsData[dotPosition] = 0;
            
            //whatever dot is on top falls down, whatever is left empty above that just spawns in a new dot
            
            if(dotPosition > DataConstants.Instance.GridSize)
            {
                //first row, just spawn in
                Destroy(_gridDots[dotPosition].gameObject);
                _gridDots.Remove(dotPosition);
                SpawnDotAtPosition(dotPosition);
                return;
            }
            
            
            //check is any dot(s) above it
            
            //if there is a dot above it, move it down
            
            //if there is no dot above it, but there is empty space, spawn a new dot
            

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

        public void MoveDotToPosition(int dotPosition, int newDotPosition)
        {
            //lerp the dot to the new position
            StartCoroutine(MoveDotRoutine(dotPosition, newDotPosition));
        }
        
        private IEnumerator MoveDotRoutine(int dotPosition, int newDotPosition)
        {
            float elapsedTime = 0;
            Vector2 initialPosition = _gridDots[dotPosition].transform.position;
            Vector2 targetPosition = _gridDots[newDotPosition].transform.position;
            while (elapsedTime < DotMoveDuration)
            {
                _gridDots[dotPosition].transform.position = Vector2.Lerp(initialPosition, targetPosition, elapsedTime / DotMoveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            _gridDots[dotPosition].transform.position = targetPosition;
            PopDot(dotPosition);
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
                            if (_highlightedDots.Count > 1)
                            {
                                LineDrawer.Instance.DrawLineFromPoints(_gridDots[_previousDotPosition].transform.position,
                                    _gridDots[_currentDotPosition].transform.position, 0.1f, _gridDots[_currentDotPosition].GetColor());   
                            }
                            
                            _previousDotPosition = _currentDotPosition;
                            _previousDotValue = _currentDotValue;
                        }
                    }
                }
                yield return null;
            }
        }

        public bool CheckForMatch()
        {
            return _highlightedDots.Count > 1;
        }

        private bool HasValidNeighbour(int currentDotValue, int previousDotValue)
        {
            return currentDotValue == previousDotValue &&
                   GridUtils.CheckDotIsNeighbour(_currentDotPosition, _previousDotPosition);
        }

        private void TouchInputEnded()
        { 
            LineDrawer.Instance.ClearAllLines();
            if (CheckForMatch())
            {
                //match found
                Debug.Log("Match Found");
                for (int i = 0; i < _highlightedDots.Count - 1; i++)
                {
                    MoveDotToPosition(_highlightedDots[i].GetPosition(), _highlightedDots.Last().GetPosition());
                }
            }
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
