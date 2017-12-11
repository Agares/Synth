using Xunit;

namespace Synthesizer.Tests
{
    public class AudioChannelBufferTest
    {
        [Fact]
        public void CanBeCreatedFromRawBuffer()
        {
            AudioChannelBuffer.CreateFromRawBuffer(new float[1], 0, 1);
        }

        [Fact]
        public void ExposesCorrectSegment()
        {
            var buffer = new float[] {1, 2, 3};

            var audioBuffer = AudioChannelBuffer.CreateFromRawBuffer(buffer, 1, 1);

            Assert.Equal(1, audioBuffer.Length);
            Assert.Equal(2, audioBuffer[0]);
        }

        [Fact]
        public void ExposesCorrectLength()
        {
            var buffer = new float[256];

            var audioBuffer = AudioChannelBuffer.CreateFromRawBuffer(buffer, 64, 128);

            Assert.Equal(128, audioBuffer.Length);
        }
    }
}