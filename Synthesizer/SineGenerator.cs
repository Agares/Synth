using System;
using System.Runtime.CompilerServices;

namespace Synthesizer
{
    public sealed class SineGenerator : ISampleSource
    {
        private int _sampleIndex;
        private double _previousSampleFrequency = double.NaN;
        private double _phaseShift;

        public double Frequency { get; set; } = 440;

        private int SampleRate { get; }

        public SineGenerator(int sampleRate)
        {
            SampleRate = sampleRate;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadNextSample()
        {
            // save the frequency, so it does not change during the computation of the current sample
            var frequency = Frequency;

            if (HasFrequencyChanged(frequency))
            {
                UpdatePhaseShift(frequency);
            }

            EnsureMinimalSampleIndex(frequency);
            var sample = (float)CalculateSample(frequency);

            _previousSampleFrequency = frequency;
            ++_sampleIndex;

            return sample;
        }

        private bool HasFrequencyChanged(double frequency)
        {
            return !double.IsNaN(_previousSampleFrequency)
                   && Math.Abs(frequency - _previousSampleFrequency) > 1E-9;
        }

        private void UpdatePhaseShift(double frequency)
        {
            _phaseShift += CalculatePhaseShiftBetweeenFrequencies(_previousSampleFrequency, frequency);

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
            if (Math.Abs(_sampleIndex * frequency - SampleRate) < 1E-9)
            {
                _sampleIndex = 0;
            }
        }

        private double CalculatePhaseShiftBetweeenFrequencies(double previousFrequency, double frequency)
        {
            var time = (_sampleIndex - 1) / (double)SampleRate;

            return 2 * Math.PI * time * (previousFrequency - frequency);
        }
    }
}