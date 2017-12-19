namespace Synthesizer
{
    public class OutputFormat
    {
        public SampleRate SampleRate { get; }
        public int NumberOfChannels { get; }
        
        public OutputFormat(SampleRate sampleRate, int numberOfChannels)
        {
            SampleRate = sampleRate;
            NumberOfChannels = numberOfChannels;
        }
    }
}