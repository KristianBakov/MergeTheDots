using System;
using System.Collections.Generic;
using Dot;
using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Managers
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        private const int GridX = 5;
        private const int GridY = 5;
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private float gridSpacing = 1.7f;
        
        public Action OnGridInitialized;
        public Dictionary<int, int> GamePointsData;
        
        private Dictionary<int, Vector2> _gridDotPositions;
        private List<GameObject> _dotsGoList;
        
        private SpriteRenderer _gridSpriteRenderer;

        protected override void Awake()
        {
            base.Awake();
            _gridSpriteRenderer = gridParent.GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            GamePointsData = new Dictionary<int, int>();
            _gridDotPositions = new Dictionary<int, Vector2>();
            _dotsGoList = new List<GameObject>();
            CreateGrid();
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

        //Create a 5x5 grid for the game dots
        private void CreateGrid()
        {
            int gamePointPositionCounter = 0;
            
            Vector2 gridPosition = gridParent.transform.position;
            Vector2 size = _gridSpriteRenderer == null ? _gridSpriteRenderer.bounds.size :
                gridParent.GetComponent<SpriteRenderer>().bounds.size;
            Vector2 gridStartPos = new Vector2((gridPosition.x - size.x / 2f), gridPosition.y + size.y / 2f);
            Vector2 localScale = dotPrefab.transform.lossyScale;
            var parent = gridParent.parent;
            Vector2 gridLocalScaleFactor = new Vector2(gridParent.localScale.x * parent.localScale.x,
                gridParent.localScale.y * parent.localScale.y);
            Vector2 gridDotHalfScale = new Vector2(localScale.x / 2f, localScale.y / 2f) * gridLocalScaleFactor;
            
            
            for (int x = 0; x < GridX; x++)
            {
                for (int y = 0; y < GridY; y++)
                {
                    gamePointPositionCounter++;

                    float posX = gridDotHalfScale.x + (gridStartPos.x + (x * gridSpacing));
                    float posY = -gridDotHalfScale.y + gridStartPos.y - (y * gridSpacing);
                    Vector2 dotPosition = new Vector3(posX, posY);
                    _gridDotPositions.TryAdd(gamePointPositionCounter, dotPosition);
                }
            }
            CreateDotGameObjects();
            OnGridInitialized?.Invoke();
        }

        private void CreateDotGameObjects()
        {
            //TODO: Read values externally from a json/text file in format 2,4,8,16...
            foreach (var dotNum in _gridDotPositions.Keys)
            {
                CreateDot(dotNum, 8);
            }
        }
        
        private void CreateDot(int pos, int value)
        {
            Vector3 dotPosition = new Vector3(_gridDotPositions[pos].x, _gridDotPositions[pos].y, 1);
            var dot = Instantiate(dotPrefab, dotPosition, Quaternion.identity, gridParent);
            
            _dotsGoList.Add(dot);
            GamePointsData.TryAdd(pos, value);
        }

        public void CreateLine()
        {
            
        }
    }
}
