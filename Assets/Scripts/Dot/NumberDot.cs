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

        private Vector2 originalScale;
        private bool _isHighlighted = false;


        [SerializeField] private TextMeshPro valueText;

        public NumberDot(int value)
        {
            SetValue(value);
            OnDotInitialized?.Invoke();
        }

        private void Awake()
        {
            originalScale = transform.localScale;
        }

        //called whenevver input highlights the dot
        public void HighlightDot(bool isHighlighted)
        {
            // Decide on the new scale based on whether the dot is highlighted or not
            Vector2 newScale = isHighlighted ? new Vector2(originalScale.x + ScaleFactor, originalScale.y + ScaleFactor) : originalScale;
            // Start the coroutine to lerp to the new scale
            StartCoroutine(LerpToNewScale(newScale));
        }

        private IEnumerator LerpToNewScale(Vector2 newScale)
        {
            float elapsedTime = 0;
            float duration = 0.1f;
            Vector2 initialScale = transform.localScale;
            while (elapsedTime < duration)
            {
                transform.localScale = Vector2.Lerp(initialScale, newScale, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            // Ensure the final scale is set accurately after the lerp
            transform.localScale = newScale;
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
