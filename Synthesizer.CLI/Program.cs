using System;
using System.Threading;
using NAudio.Wave;
using ISampleProvider = Synthesizer.ISampleProvider;

namespace Synthesizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new WaveOut();
            var sampleProvider = new SineGenerator(2, 44100);
            var signalGenerator = new NAudioSampleProvider(sampleProvider);
            output.Init(signalGenerator);
            output.Play();

            while (true)
            {
                Console.WriteLine(sampleProvider.Frequency.ToString("F2"));
                var consoleKeyInfo = Console.ReadKey();
                if (consoleKeyInfo.KeyChar == '=')
                {
                    sampleProvider.Frequency *= 1.05946309436f;
                }
                else if (consoleKeyInfo.KeyChar == '-')
                {
                    sampleProvider.Frequency /= 1.05946309436f;
                }

                Thread.Sleep(100);
            }
        }
    }

    internal sealed class NAudioSampleProvider : NAudio.Wave.ISampleProvider
    {
        private readonly ISampleProvider _sampleProvider;

        public NAudioSampleProvider(ISampleProvider sampleProvider)
        {
            _sampleProvider = sampleProvider;
            WaveFormat = WaveFormat.CreateIeeeFloatWaveFormat(_sampleProvider.SampleRate, _sampleProvider.NumberOfChannels);
        }

        public int Read(float[] buffer, int offset, int count)
        {
            return _sampleProvider.Read(AudioBuffer.CreateFromRawBuffer(WaveFormat.Channels, buffer, offset, count));
        }

        public WaveFormat WaveFormat { get; }
    }
}
