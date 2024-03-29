using System;
using System.Collections;
using System.Collections.Generic;
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
            foreach (var dot in gridParent.GetComponentsInChildren<NumberDot>())
            {
                int value = GetNextDotValue();
                dot.SetValue(value);
                dot.gameObject.GetComponent<SpriteRenderer>().color = GetNumberValueColor(value);

                _dotsList.Add(dot);
                GamePointsData.Add(_dotsList.IndexOf(dot), value);
                _gridDots.Add(_dotsList.IndexOf(dot), dot);
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
            Debug.Log("Swipe detected. responding here");
            //check if the touch pos is inside a dot collision box
            
            if(_touchInputManager.IsTouching)
            {
                StartCoroutine(nameof(HighlightSwipedDots));
                    // NumberDot dot = hit.collider.GetComponent<NumberDot>();
                    // if (dot != null)
                    // {
                    //     Debug.Log("Dot found");
                    //     //check if the dot can be moved
                    //     if (IsPositionBelowDotEmpty(dot.GetPosition()))
                    //     {
                    //         MoveDotToNextAvailableColumn(dot.GetPosition());
                    //     }
                    //     else
                    //     {
                    //         PopDot(dot.GetPosition());
                    //     }
                    // }
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
                    Debug.Log(hit.collider.name);
                    
                    //adding the dot to the list of highlighted dots
                    NumberDot dot = hit.collider.GetComponent<NumberDot>();
                    _highlightedDots.Add(dot);
                    
                    //highlight the dot
                    dot.HighlightDot(_touchInputManager.IsTouching);
                }
                yield return null;
            }
        }

        private void OnEnable()
        {
            OnGridInitialized += InitializeFinished;
            _touchInputManager.OnStartTouchInput += RespondToTouchInput;
        }

        private void OnDisable()
        {
            OnGridInitialized -= InitializeFinished;
            _touchInputManager.OnStartTouchInput -= RespondToTouchInput;
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
