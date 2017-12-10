namespace Synthesizer
{
    public interface ISampleProvider
    {
        int NumberOfChannels { get; }
        int SampleRate { get; }

        int Read(AudioBuffer buffer);
    }
}