namespace Synthesizer
{
    public class SampleRate
    {
        private readonly int _value;

        public SampleRate(int value)
        {
            _value = value;
        }

        public static implicit operator int(SampleRate self)
        {
            return self._value;
        }

        public static implicit operator double(SampleRate self)
        {
            return self._value;
        }

        public static implicit operator float(SampleRate self)
        {
            return self._value;
        }
    }
}