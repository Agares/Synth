namespace Synthesizer
{
    public interface ISampleProvider
    {
        int Read(AudioChannelBuffer channelBuffer);
    }
}