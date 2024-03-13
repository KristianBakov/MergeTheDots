using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TestScript : MonoBehaviour
{
    public float x_Start, y_Start;
    public int ColumnLength;
    public int RowLength;
    public float x_Space, y_Space;
    public GameObject prefab;
    public Transform gridParent;
    public Transform startT;
    
    private List<GameObject> _dotsGoList;
    [SerializeField] private SpriteRenderer _gridSpriteRenderer;
    
    private Vector2 GetGridScaleFactor()
    {
        var parentLocalScale = gridParent.transform.parent.localScale;
        var gridLocalScale = gridParent.localScale;
        return new Vector2(gridLocalScale.x * parentLocalScale.x,
            gridLocalScale.y * parentLocalScale.y);
    }
    
    void Start()
    {
        _dotsGoList = new List<GameObject>();
        
        Vector2 gridPosition = gridParent.position;
        Vector2 gridSize = _gridSpriteRenderer == null ? _gridSpriteRenderer.bounds.size :
            gridParent.GetComponent<SpriteRenderer>().bounds.size;
        var prefabScale = prefab.transform.localScale;
        Vector2 gridStartOffset = prefabScale * GetGridScaleFactor();
        Vector2 gridStartPos = new Vector2((gridPosition.x - gridSize.x / 2f) + gridStartOffset.x,
                                            (gridPosition.y + gridSize.y / 2f) - gridStartOffset.y);
        
        //calculate how much we need to increase the scale to fit well inside the grid area
        Vector2 newPrefabScale = new Vector2(gridSize.x / (ColumnLength * prefabScale.x),
                                            gridSize.y / (RowLength * prefabScale.y));
        
        for (int i = 0; i < ColumnLength + RowLength; i++)
        {
            Vector3 position;
            position = new Vector3(gridStartPos.x + (x_Space * (i % ColumnLength)), gridStartPos.y + (-y_Space * (i / ColumnLength)));
            var dot = Instantiate(prefab, position, Quaternion.identity, gridParent);
            dot.transform.localScale = newPrefabScale / GetGridScaleFactor();
            _dotsGoList.Add(dot);
        }

    }
}
