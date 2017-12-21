namespace Synthesizer
{
    public class ConstantSampleSource : ISampleProvider
    {
        private readonly float _value;

        public ConstantSampleSource(float value)
        {
            _value = value;
        }        public int Read(AudioChannelBuffer channelBuffer)        {
            for (int i = 0; i < channelBuffer.Length; i++)
            {
                channelBuffer[i] = _value;
            }

            return channelBuffer.Length;
        }    }
}