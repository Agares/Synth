using System;
using System.Threading;
using NAudio.Wave;

namespace Synthesizer.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new WaveOut();
            var sampleSource = new SineGenerator(44100);
            var sampleProvider = new SampleSourceSampleProvider(sampleSource);
            var signalGenerator = new NAudioSampleProvider(sampleProvider);
            output.Init(signalGenerator);
            output.Play();

            while (true)
            {
                Console.WriteLine(sampleSource.Frequency.ToString("F2"));
                var consoleKeyInfo = Console.ReadKey();
                if (consoleKeyInfo.KeyChar == '=')
                {
                    sampleSource.Frequency *= 1.05946309436f;
                }
                else if (consoleKeyInfo.KeyChar == '-')
                {
                    sampleSource.Frequency /= 1.05946309436f;
                }

                Thread.Sleep(100);
            }
        }
    }
}
