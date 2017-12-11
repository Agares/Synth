using System.Linq;
using Xunit;

namespace Synthesizer.Tests
{
    public class SampleSourceSampleProviderTest
    {
        private class SampleSource : ISampleSource
        {
            public float[] Samples { private get; set; }
            private int _index;

            public float ReadNextSample()
            {
                float sample = Samples[_index];
                _index++;
                return sample;
            }
        }

        [Fact]
        public void CanReadSamplesIntoBuffer()
        {
            float[] buffer = new float[1024];

            var audioBuffer = AudioChannelBuffer.CreateFromRawBuffer(buffer, 512, 512);

            var expectedSamples = Enumerable.Range(0, 512).Select(x => (float) x).ToArray();

            var sampleSource = new SampleSource {Samples = expectedSamples};

            var sampleProvider = new SampleSourceSampleProvider(sampleSource);
            sampleProvider.Read(audioBuffer);

            Assert.Equal(expectedSamples, buffer.Skip(512));
        }
    }
}