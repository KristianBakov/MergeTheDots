using System;
using TMPro;
using UnityEngine;

namespace Dot
{
    public class NumberDot : MonoBehaviour, INumberDot
    {
        private int _position;
        private int _value;
        public Action OnDotInitialized; 


        [SerializeField] private TextMeshPro valueText;

        public NumberDot(int value)
        {
            SetValue(value);
            OnDotInitialized?.Invoke();
        }

        private void OnMouseEnter()
        {
            //enlarge scale slightly on hover
        }

        public void OnInputEntered()
        {

        }

        public void OnInputReleased()
        {
            
        }
        
        public bool IsValueValid(int value)
        {
            return value >= 2 && value % 2 == 0;
        }
        
        public void SetValue(int newValue)
        {
            if(!IsValueValid(newValue)) return;
            _value = newValue;
            valueText.text = _value.ToString();
        }

        public int GetValue()
        {
            return _value;
        }

        public int GetPosition()
        {
            return _position;
        }

        public void SetPosition(int newPosition)
        {
            _position = newPosition;
        }
    }
}
