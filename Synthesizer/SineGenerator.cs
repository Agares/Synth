using System;

namespace Synthesizer
{
    public sealed class SineGenerator : ISampleProvider
    {
        private int _sampleIndex;
        private double _previousSampleFrequency = double.NaN;
        private double _phaseShift;

        public double Frequency { get; set; } = 440;

        public int NumberOfChannels { get; }
        public int SampleRate { get; }

        public SineGenerator(int numberOfChannels, int sampleRate)
        {
            NumberOfChannels = numberOfChannels;
            SampleRate = sampleRate;
        }

        public int Read(AudioBuffer buffer)
        {
            for (var i = 0; i < buffer.SamplesPerChannel; i++)
            {
                ReadSample(buffer, i);
            }

            return buffer.Length;
        }

        private void ReadSample(AudioBuffer buffer, int currentBufferSampleIndex)
        {
            // save the frequency, so it does not change during the computation of the current sample
            var frequency = Frequency;

            if (FrequencyChanged(frequency))
            {
                UpdatePhaseShift(frequency);
            }

            EnsureMinimalSampleIndex(frequency);
            buffer.WriteToAllChannels(currentBufferSampleIndex, (float)CalculateSample(frequency));

            _previousSampleFrequency = frequency;
            ++_sampleIndex;
        }

        private bool FrequencyChanged(double frequency)
        {
            return !double.IsNaN(_previousSampleFrequency) 
                   && Math.Abs(frequency - _previousSampleFrequency) > double.Epsilon;
        }

        private void UpdatePhaseShift(double frequency)
        {
            _phaseShift += CalculatePhaseShiftBetweeenFrequencies(SampleRate, frequency);

            if (_phaseShift >= Math.PI * 2)
            {
                _phaseShift -= Math.PI * 2;
            }
        }

        private double CalculateSample(double frequency)
        {
            return Math.Sin(_sampleIndex / (double)SampleRate * 2.0 * Math.PI * frequency + _phaseShift);
        }

        private void EnsureMinimalSampleIndex(double frequency)
        {
            if (Math.Abs(_sampleIndex * frequency - SampleRate) < double.Epsilon)
            {
                _sampleIndex = 0;
            }
        }

        private double CalculatePhaseShiftBetweeenFrequencies(double sampleRate, double frequency)
        {
            var time = (_sampleIndex - 1) / sampleRate;

            return 2 * Math.PI * time * (_previousSampleFrequency - frequency);
        }
    }
}