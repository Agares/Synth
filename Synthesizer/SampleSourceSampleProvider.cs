using System.Runtime.CompilerServices;

namespace Synthesizer
{
    public class SampleSourceSampleProvider : ISampleProvider
    {
        private readonly ISampleSource _sampleSource;

        public SampleSourceSampleProvider(ISampleSource sampleSource)
        {
            _sampleSource = sampleSource;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Read(AudioChannelBuffer channelBuffer)
        {
            for (int i = 0; i < channelBuffer.Length; i++)
            {
                channelBuffer[i] = _sampleSource.ReadNextSample();
            }

            return channelBuffer.Length;
        }
    }
}