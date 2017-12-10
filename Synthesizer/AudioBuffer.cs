using System;

namespace Synthesizer 
{
    public sealed class AudioBuffer
    {
        private readonly int _numberOfChannels;
        private readonly float[] _buffer;
        private readonly int _offset;

        public int Length { get; }
        public int SamplesPerChannel => Length/_numberOfChannels;

        private AudioBuffer(int numberOfChannels, float[] buffer, int offset, int length)
        {
            _numberOfChannels = numberOfChannels;
            _buffer = buffer;
            _offset = offset;
            Length = length;
        }

        public static AudioBuffer CreateFromRawBuffer(int numberOfChannels, float[] buffer, int offset, int length)
        {
            return new AudioBuffer(numberOfChannels, buffer, offset, length);
        }

        public float this[int index]
        {
            get => _buffer[_offset + index];
            set => _buffer[_offset + index] = value;
        }

        public void WriteToAllChannels(int sampleIndex, float val)
        {
            for (int i = 0; i < _numberOfChannels; i++)
            {
                this[sampleIndex * _numberOfChannels + i] = val;
            }
        }
    }
}