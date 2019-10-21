namespace JoinExtensions
{
    public class JoinItem<TLeft, TRight>
    {
        public TLeft Left { get; set; }
        public TRight Right { get; set; }
    }
}