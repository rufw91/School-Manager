using System;

namespace SetupUI
{
    public class BasicPair<T,T1>
    {
        public BasicPair(T key, T1 value)
        {
            Key = key;
            Value = value;
        }
        public T Key
        { get; set; }

        public T1 Value
        { get; set; }

        public override int GetHashCode()
        {
            return (new Random().Next(9999)* new Random().Next(9999)).GetHashCode();
        }
    }
}
