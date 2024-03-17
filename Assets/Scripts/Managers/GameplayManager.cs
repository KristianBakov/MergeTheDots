using System;
using System.Collections.Generic;
using Data;
using Dot;
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
        
        private Dictionary<int, Vector2> _gridDotPositions;
        private List<NumberDot> _dotsList = new();
        
        private List<int> _possibleSpawnValues = new();

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        private void Initialize()
        {
            GamePointsData = new Dictionary<int, int>();
            _gridDotPositions = new Dictionary<int, Vector2>();
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

                _dotsList.Add(dot);
                GamePointsData.Add(_dotsList.IndexOf(dot), value);
                _gridDotPositions.Add(_dotsList.IndexOf(dot), dot.transform.position);
            }
            OnGridInitialized?.Invoke();
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

        private void OnEnable()
        {
            OnGridInitialized += InitializeFinished;
        }

        private void OnDisable()
        {
            OnGridInitialized -= InitializeFinished;
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
