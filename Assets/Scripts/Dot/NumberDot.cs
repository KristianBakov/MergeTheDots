using UnityEngine;

namespace Dot
{
    public class NumberDot : MonoBehaviour, INumberDot
    {
        public int Value { get; set; }
        
        public NumberDot(float posX, float posY, int value)
        {
            Value = value;
        }
        
        public void OnInputEntered()
        {
            
        }

        public void OnInputReleased()
        {
            
        }
    }
}
