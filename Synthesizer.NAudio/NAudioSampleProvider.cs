using NAudio.Wave;

namespace Synthesizer.NAudio
{
    public sealed class NAudioSampleProvider : global::NAudio.Wave.ISampleProvider
    {
        public WaveFormat WaveFormat { get; }

        private readonly ISampleProvider _sampleProvider;

        public NAudioSampleProvider(ISampleProvider sampleProvider)
        {
            _sampleProvider = sampleProvider;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(44100, 1);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return _sampleProvider.Read(AudioChannelBuffer.CreateFromRawBuffer(buffer, offset, count));
        }
    }
}