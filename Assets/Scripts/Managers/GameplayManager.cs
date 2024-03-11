using System;
using System.Collections.Generic;
using Dot;
using UnityEngine;
using Utils;

namespace Managers
{
    public class GameplayManager : MonoSingleton<GameplayManager>
    {
        [SerializeField] private GameObject dotPrefab;
        [SerializeField] private Transform gridParent;
        private List<GameObject> dots;
        
        public Action OnGridInitialized;
        
        public Dictionary<int, int> gamePointsData;
        private Dictionary<int, Vector2> _gridDotPositions;
        
        private const int GridX = 5;
        private const int GridY = 5;
        [SerializeField]  float GridSpacing = 5.5f;
        public bool test;

        private void Start()
        {
            Initialize();
        }

        private void Update()
        {
            if (test)
            {
                ClearDots();
                Initialize();
                test = false;
            }
        }

        private void Initialize()
        {
            gamePointsData = new Dictionary<int, int>();
            _gridDotPositions = new Dictionary<int, Vector2>();
            dots = new List<GameObject>();
            CreateGrid();
        }

        //Create a 5x5 grid for the game dots
        private void CreateGrid()
        {
            int gamePointPosition = 0;
            
            for (int x = 0; x < GridX; x++)
            {
                for (int y = 0; y < GridY; y++)
                {
                    gamePointPosition++;
                    
                    //Create a grid equally spaced as a 5x5 grid
                    //float posX = startX + x * (cellSizeX + gapX) + cellSizeX / 2;
                    //float posY = startY + y * (cellSizeY + gapY) + cellSizeY / 2;
                    float posX = x * GridSpacing;
                    float posY = y * GridSpacing;
                    //scale thing basically x / 9, y /16 and additional 0.7 for the other object. this is part of the prefab so we odnt havbe to change it here


                    Vector2 gridSpawnPos = new Vector2(posX, posY);
                    _gridDotPositions.TryAdd(gamePointPosition, gridSpawnPos);
                    
                    //TODO: Read values externally from a json/text file in format 2,4,8,16...
                    CreateDot(gamePointPosition, 8);
                }
            }
            
            OnGridInitialized?.Invoke();
        }

        void ClearDots()
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
            _gridDotPositions.Clear();
        }
        private void CreateDot(int pos, int value)
        {
            Vector3 dotPosition = new Vector3(_gridDotPositions[pos].x, _gridDotPositions[pos].y, 1);
            var dot = Instantiate(dotPrefab, dotPosition, Quaternion.identity, gridParent);
            
            dots.Add(dot);
            
            gamePointsData.TryAdd(pos, value);
        }
    }
}
