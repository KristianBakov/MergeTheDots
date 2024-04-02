using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    private void Start()
    {
        var doc = GetComponent<UIDocument>();
        //doc.rootVisualElement.Add(new LineDrawer(new Vector2(20, 50), new Vector2(50, 50), 10));
    }
}
