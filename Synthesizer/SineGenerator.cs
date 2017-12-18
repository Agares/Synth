using System;
using System.Runtime.CompilerServices;

namespace Synthesizer
{
    public sealed class SineGenerator : ISampleSource
    {
        private int _sampleIndex;
        private double _previousSampleFrequency = double.NaN;
        private double _phaseShift;

        private readonly ISampleSource _frequencySource;
        private readonly ISampleSource _amplitudeSource;

        private int SampleRate { get; }

        public SineGenerator(int sampleRate, ISampleSource frequencySource, ISampleSource amplitudeSource)
        {
            SampleRate = sampleRate;
            _frequencySource = frequencySource;
            _amplitudeSource = amplitudeSource;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public float ReadNextSample()
        {
            var frequency = _frequencySource.ReadNextSample();
            var amplitude = _amplitudeSource.ReadNextSample();

            if (HasFrequencyChanged(frequency))
            {
                UpdatePhaseShift(frequency);
            }

            EnsureMinimalSampleIndex(frequency);
            var sample = amplitude * (float)CalculateSample(frequency);

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