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
        [SerializeField] private float dotScaleFactor = 1.0f;
        
        private float gridSpacing = 1.5f;
        
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

        private Vector2 GetGridScaleFactor()
        {
            var parentLocalScale = gridParent.parent.localScale;
            var gridLocalScale = gridParent.localScale;
            return new Vector2(gridLocalScale.x * parentLocalScale.x,
                gridLocalScale.y * parentLocalScale.y);
        }

        //Create a 5x5 grid for the game dots
        private void CreateGrid()
        {
            int gamePointPositionCounter = 0;
            
            Vector2 gridPosition = gridParent.transform.position;
            Vector2 gridSize = _gridSpriteRenderer == null ? _gridSpriteRenderer.bounds.size :
                gridParent.GetComponent<SpriteRenderer>().bounds.size;
            Vector2 gridStartPos = new Vector2((gridPosition.x - gridSize.x / 2f), gridPosition.y + gridSize.y / 2f);
            Vector2 gridEndPos = new Vector2((gridPosition.x + gridSize.x / 2f), gridPosition.y - gridSize.y / 2f);
            
            Vector2 localScale = dotPrefab.transform.lossyScale;
            Vector2 gridDotHalfScaleNormalized = new Vector2(localScale.x / 2f, localScale.y / 2f) * GetGridScaleFactor();
            //CalculateGridSpacing(gridStartPos, gridDotHalfScaleNormalized, gridEndPos);
            //if (gridSpacing > gridHaldScale)
            //we want to adjust scale up until the space between dots is = griddotHalfScale


            //we want to adjust scale up until the space between dots is = griddotHalfScale
            
            for (int x = 0; x < GridX; x++)
            {
                for (int y = 0; y < GridY; y++)
                {
                    gamePointPositionCounter++;
                    
                    // float posX = gridStartPos.x + (x * gridSpacing + x * gridDotHalfScaleNormalized.x * 2) + gridDotHalfScaleNormalized.x;
                    // float posY = gridStartPos.y - (y * gridSpacing + y * gridDotHalfScaleNormalized.y * 2) - gridDotHalfScaleNormalized.y;
                    float posX = gridStartPos.x + (x * gridDotHalfScaleNormalized.x + x * gridDotHalfScaleNormalized.x * 2) + gridDotHalfScaleNormalized.x;
                    float posY = gridStartPos.y - (y * gridDotHalfScaleNormalized.y + y * gridDotHalfScaleNormalized.y * 2) - gridDotHalfScaleNormalized.y;
                    Vector2 dotPosition = new Vector3(posX, posY);
                    _gridDotPositions.TryAdd(gamePointPositionCounter, dotPosition);
                }
            }
            InitializeGridGameObjects();
            OnGridInitialized?.Invoke();
        }

        private void CalculateGridSpacing(Vector2 gridStartPos, Vector2 gridDotHalfScale, Vector2 gridEndPos)
        {
            float lastDotPosX = gridStartPos.x + (GridX * gridSpacing + GridX * gridDotHalfScale.x * 2) +
                                gridDotHalfScale.x ;
            if (lastDotPosX > gridEndPos.x)
            {
                float gridSize = gridEndPos.x - gridStartPos.x;
                //adjust dot scale and spacing to fit the grid
                float newSpacing = gridSize / GridX - gridDotHalfScale.x * 2;
                gridSpacing = newSpacing;
            }
        }

        private void InitializeGridGameObjects()
        {

            //calculate dot scale
            Vector2 gridPosition = gridParent.transform.position;
            Vector2 gridSize = _gridSpriteRenderer == null ? _gridSpriteRenderer.bounds.size :
                gridParent.GetComponent<SpriteRenderer>().bounds.size;
            Vector2 gridStartPos = new Vector2((gridPosition.x - gridSize.x / 2f), gridPosition.y + gridSize.y / 2f);
            Vector2 localScale = dotPrefab.transform.lossyScale;
            Vector2 gridDotHalfScaleNormalized = new Vector2(localScale.x / 2f, localScale.y / 2f) * GetGridScaleFactor();

            gridSpacing = gridDotHalfScaleNormalized.x;
            float newScale = _gridDotPositions[1].x - 4 * gridDotHalfScaleNormalized.x;
            
            //TODO: Read values externally from a json/text file in format 2,4,8,16...
            foreach (var dotNum in _gridDotPositions.Keys)
            {
                CreateDot(dotNum, 8, newScale);
            }
        }
        
        private void CreateDot(int pos, int value, float scale)
        {
            Vector3 dotPosition = new Vector3(_gridDotPositions[pos].x, _gridDotPositions[pos].y, 1);
            var dot = Instantiate(dotPrefab, dotPosition, Quaternion.identity, gridParent);

            Vector2 scaleFactor = GetGridScaleFactor();
            dot.transform.localScale += new Vector3(scale / scaleFactor.x, scale / scaleFactor.y, 1) ;
            
            _dotsGoList.Add(dot);
            GamePointsData.TryAdd(pos, value);
        }

        public void CreateLine()
        {
            
        }
    }
}
