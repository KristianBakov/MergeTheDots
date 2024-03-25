using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Dot
{
    public class NumberDot : MonoBehaviour, INumberDot
    {
        private int _position;
        private int _value;
        public Action OnDotInitialized;
        [SerializeField] public float ScaleFactor = 0.2f;

        private bool _isHighlighted = false;


        [SerializeField] private TextMeshPro valueText;

        public NumberDot(int value)
        {
            SetValue(value);
            OnDotInitialized?.Invoke();
        }
        
        //called whenevver input highlights the dot
        public void HighlightDot(bool isHighlighted)
        {
            //TODO: Clamp scale factor 
            //highlight dot
            //_isHighlighted = !_isHighlighted;
            var localScale = transform.localScale;
            Vector2 newScale = isHighlighted ? new Vector3(localScale.x + ScaleFactor,
                localScale.y + ScaleFactor, 1) : transform.localScale;
            //lerp to new scale
            StartCoroutine(LerpToNewScale(newScale));
        }

        private IEnumerator LerpToNewScale(Vector2 newScale)
        {
            float elapsedTime = 0;
            float duration = 0.3f;
            Vector2 initialScale = transform.localScale;
            while (elapsedTime < duration)
            {
                transform.localScale = Vector2.Lerp(initialScale, newScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
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
