using System;
using System.Threading;
using NAudio.Wave;
using Synthesizer.NAudio;

namespace Synthesizer.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var output                = new WaveOut();
            var frequencySampleSource = new ManualInputSampleSource(440.0f);
            var amplitudeSampleSource = new SineGenerator(
                44100, 
                new ConstantSampleSource(0.5f), 
                new ConstantSampleSource(0.2f)
            );
            var sampleSource          = new SineGenerator(44100, frequencySampleSource, amplitudeSampleSource);
            var sampleProvider        = new SampleSourceSampleProvider(sampleSource);
            var nAudioSampleProvider  = new NAudioSampleProvider(sampleProvider);
            
            output.Init(nAudioSampleProvider);
            output.Play();

            while (true)
            {
                Console.WriteLine(frequencySampleSource.Value.ToString("F2"));
                var consoleKeyInfo = Console.ReadKey();
                switch (consoleKeyInfo.KeyChar)
                {
                    case '=':
                        frequencySampleSource.Value *= 1.05946309436f;
                        break;
                    case '-':
                        frequencySampleSource.Value /= 1.05946309436f;
                        break;
                    default: break;
                }

                Thread.Sleep(100);
            }
        }
    }
}
