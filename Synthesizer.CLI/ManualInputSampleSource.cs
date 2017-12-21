namespace Synthesizer.CLI
{
    public class ManualInputSampleSource : ISampleProvider
    {
        private volatile float _value;

        public float Value { get => _value; set => _value = value; }

        public ManualInputSampleSource(float value)
        {
            Value = value;
        }        public int Read(AudioChannelBuffer channelBuffer)        {
            for (int i = 0; i < channelBuffer.Length; i++)
            {
                channelBuffer[i] = Value;
            }

            return channelBuffer.Length;
        }    }
}