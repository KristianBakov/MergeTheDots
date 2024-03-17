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
        private List<NumberDot> _dotsList;

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
            GatherGridData();
            
        }

        private void GatherGridData()
        {
            foreach (var dot in gridParent.GetComponentsInChildren<NumberDot>())
            {
                dot.SetValue(GetRandomDotStartingValue());
                Debug.Log(dot.name);
                _dotsList.Add(dot);
            }
        }

        private int GetRandomDotStartingValue()
        {
            //starting value are either 2,4 or 8
            return Random.Range(0, 3) * 2 + 2;
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
            
        }

    }
}
