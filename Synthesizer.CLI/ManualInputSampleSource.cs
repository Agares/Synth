namespace Synthesizer.CLI
{
    public class ManualInputSampleSource : ISampleSource
    {
        public float Value { get; set; }

        public ManualInputSampleSource(float value)
        {
            Value = value;
        }

        public float ReadNextSample()
        {
            return Value;
        }
    }
}