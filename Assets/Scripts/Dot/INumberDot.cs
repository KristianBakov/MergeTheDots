namespace Dot
{
    public interface INumberDot
    {
        public int Value { get; set; }
        public void OnInputEntered();
        public void OnInputReleased();
    }
}
