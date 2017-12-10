using Xunit;

namespace Synthesizer.Tests
{
    public class AudioBufferTest
    {
        [Fact]
        public void CanBeCreatedFromRawBuffer()
        {
            AudioBuffer.CreateFromRawBuffer(2, new float[1], 0, 1);
        }

        [Fact]
        public void ExposesCorrectSegment()
        {
            var buffer = new float[] {1, 2, 3};

            var audioBuffer = AudioBuffer.CreateFromRawBuffer(2, buffer, 1, 1);

            Assert.Equal(1, audioBuffer.Length);
            Assert.Equal(2, audioBuffer[0]);
        }

        [Fact]
        public void ExposesCorrectNumberOfSamplesPerChannel()
        {
            var buffer = new float[256];

            var audioBuffer = AudioBuffer.CreateFromRawBuffer(2, buffer, 64, 128);

            Assert.Equal(64, audioBuffer.SamplesPerChannel);
        }

        [Fact]
        public void CanWriteToAllChannels()
        {
            var rawBuffer = new float[256];

            var audioBuffer = AudioBuffer.CreateFromRawBuffer(2, rawBuffer, 64, 128);

            audioBuffer.WriteToAllChannels(5, 0.5f);
            audioBuffer.WriteToAllChannels(6, 0.7f);

            Assert.Equal(0.5f, rawBuffer[64 + (5 * 2)]);
            Assert.Equal(0.5f, rawBuffer[64 + (5 * 2 + 1)]);
            Assert.Equal(0.7f, rawBuffer[64 + (6 * 2)]);
            Assert.Equal(0.7f, rawBuffer[64 + (6 * 2 + 1)]);
        }
    }
}