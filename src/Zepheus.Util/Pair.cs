namespace DragonFiesta.Util
{
    public class Pair<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public Pair(T1 pFirst, T2 pSecond)
        {
            this.First = pFirst;
            this.Second = pSecond;
        }
    }
    public class StringPair : Pair<string, string>
    {
        public StringPair() : this("", "")
        {
        }
        public StringPair(string pFirst, string pSecond) : base(pFirst, pSecond)
        {
        }
    }
}
