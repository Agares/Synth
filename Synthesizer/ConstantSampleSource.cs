namespace Synthesizer
{
    public class ConstantSampleSource : ISampleSource
    {
        private readonly float _value;

        public ConstantSampleSource(float value)
        {
            _value = value;
        }

        public float ReadNextSample()
        {
            return _value;
        }
    }
}