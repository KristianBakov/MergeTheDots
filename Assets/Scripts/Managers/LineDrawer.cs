using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Managers
{
    public class LineDrawer : MonoSingleton<LineDrawer>
    {
        public Material lineMaterial;
        private List<GameObject> _lines;
        
        protected override void Awake()
        {
            base.Awake();
            _lines = new List<GameObject>();
        }
        public void DrawLineFromPoints(Vector2 pointA, Vector2 pointB, float lineWidth, Color color)
        {
            var line = new GameObject("Line");
            
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, pointA);
            lineRenderer.SetPosition(1, pointB);
            lineRenderer.material = lineMaterial;
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            lineRenderer.sortingOrder = -10;
            
            _lines.Add(line);
        }

        public void ClearAllLines()
        {
            foreach (var line in _lines)
            {
                Destroy(line);
            }
            _lines.Clear();
        }
    }
}
