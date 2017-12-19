using System;
using System.Threading;
using NAudio.Midi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using Synthesizer.NAudio;

namespace Synthesizer.CLI
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(MidiIn.DeviceInfo(0).ProductName);
            var midiIn                = new MidiIn(0);
            var output = new WaveOut();
            midiIn.Start();
            var frequencySampleSource = new MidiDataSampleSource(midiIn);
            var sampleRate            = new SampleRate(44100);
            var outputFormat = new OutputFormat(sampleRate, 1);
            var amplitudeSampleSource = new SineGenerator(
                outputFormat, 
                new ConstantSampleSource(0.5f), 
                new ConstantSampleSource(0.2f)
            );
            var sampleSource          = new SineGenerator(outputFormat, frequencySampleSource, amplitudeSampleSource);
            var sampleProvider        = new SampleSourceSampleProvider(sampleSource);
            var nAudioSampleProvider  = new NAudioSampleProvider(sampleProvider, outputFormat);
            
            output.Init(nAudioSampleProvider);
            output.Play();

            while (true)
            {
                Thread.Sleep(100);
            }
        }
    }
}
