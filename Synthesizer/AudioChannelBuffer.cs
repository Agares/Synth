namespace Synthesizer 
{
    public sealed class AudioChannelBuffer
    {
        private readonly float[] _buffer;
        private readonly int _offset;

        public int Length { get; }

        private AudioChannelBuffer(float[] buffer, int offset, int length)
        {
            _buffer = buffer;
            _offset = offset;
            Length = length;
        }

        public static AudioChannelBuffer CreateFromRawBuffer(float[] buffer, int offset, int length)
        {
            return new AudioChannelBuffer(buffer, offset, length);
        }

        public float this[int index]
        {
            get => _buffer[_offset + index];
            set => _buffer[_offset + index] = value;
        }
    }
}