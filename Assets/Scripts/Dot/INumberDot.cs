namespace Dot
{
    public interface INumberDot
    {
        public void OnInputEntered();
        public void OnInputReleased();
        public void SetValue(int newValue);
        public int GetValue();
        public int GetPosition();
        public void SetPosition(int newPosition);
        public bool IsValueValid(int value);
    }
}
