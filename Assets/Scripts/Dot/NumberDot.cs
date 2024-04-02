using System;
using System.Collections;
using Input;
using TMPro;
using UnityEngine;

namespace Dot
{
    public class NumberDot : MonoBehaviour, INumberDot
    {
        private int _position;
        private int _value;
        private Color _color;
        public Action OnDotInitialized;
        public Action<int> OnDotReceivedInput;
        [SerializeField] public float ScaleFactor = 0.2f;

        private Vector2 originalScale;
        private bool _isHighlighted = false;
        private SpriteRenderer _spriteRenderer;


        [SerializeField] private TextMeshPro valueText;

        public NumberDot(int value)
        {
            SetValue(value);
            OnDotInitialized?.Invoke();
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            originalScale = transform.localScale;
        }

        private void OnEnable()
        {
            TouchInputManager.Instance.OnEndTouchInput += OnInputReleased;
        }

        private void OnDisable()
        {
            TouchInputManager.Instance.OnEndTouchInput -= OnInputReleased;
        }

        //called whenevver input highlights the dot
        public void HighlightDot(bool isHighlighted)
        {
            Vector2 newScale = isHighlighted ? new Vector2(originalScale.x + ScaleFactor, originalScale.y + ScaleFactor) : originalScale;
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

            transform.localScale = newScale;
        }

        private void OnMouseEnter()
        {
            //enlarge scale slightly on hover

        }

        private void OnMouseExit()
        {
            if(TouchInputManager.Instance.IsTouching) return;
            OnInputReleased();
        }

        public void OnInputEntered()
        {

        }

        public void OnInputReleased()
        {
            HighlightDot(false);
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
        
        public void SetColor(Color newColor)
        {
            _color = newColor;
            _spriteRenderer.color = _color;
        }

        public Color GetColor()
        {
            return _color;
        }

        public void SetPosition(int newPosition)
        {
            _position = newPosition;
        }
    }
}
