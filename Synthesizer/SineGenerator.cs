using System;
using System.Runtime.CompilerServices;

namespace Synthesizer
{
    public sealed class SineGenerator : ISampleSource
    {
        private int _sampleIndex;
        private float _previousSampleFrequency = float.NaN;
        private float _phaseShift;

        private const float TwoPi = (float) (Math.PI * 2);
        private const double Epsilon = 1E-9;

        private readonly float _sampleRate;

        private readonly ISampleSource _frequencySource;
        private readonly ISampleSource _amplitudeSource;

        public SineGenerator(OutputFormat outputFormat, ISampleSource frequencySource, ISampleSource amplitudeSource)
        {
            // todo how do we handle multiple channels here?
            _sampleRate = outputFormat.SampleRate;
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
            var sample = amplitude * CalculateSample(frequency);

            _previousSampleFrequency = frequency;
            ++_sampleIndex;

            return sample;
        }

        private bool HasFrequencyChanged(float frequency)
        {
            return Math.Abs(frequency - _previousSampleFrequency) > Epsilon;
        }

        private void UpdatePhaseShift(float frequency)
        {
            _phaseShift += CalculatePhaseShiftBetweeenFrequencies(_previousSampleFrequency, frequency);

            if (_phaseShift >= TwoPi)
            {
                _phaseShift -= TwoPi;
            }
        }

        private float CalculateSample(float frequency)
        {
            return (float)Math.Sin((float)_sampleIndex / _sampleRate * TwoPi * frequency + _phaseShift);
        }

        private void EnsureMinimalSampleIndex(float frequency)
        {
            if (Math.Abs(_sampleIndex * frequency - _sampleRate) < Epsilon)
            {
                _sampleIndex = 0;
            }
        }

        private float CalculatePhaseShiftBetweeenFrequencies(float previousFrequency, float frequency)
        {
            var time = (_sampleIndex - 1.0f) / _sampleRate;

            return TwoPi * time * (previousFrequency - frequency);
        }
    }
}