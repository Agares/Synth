using System;

namespace Synthesizer
{
    public sealed class SineGenerator : ISampleProvider
    {
        private int _sampleIndex;
        private float _previousSampleFrequency = float.NaN;
        private float _phaseShift;

        private const float TwoPi = (float) (Math.PI * 2);
        private const double Epsilon = 1E-9;

        private readonly float _sampleRate;

        private readonly ISampleProvider _frequencySource;
        private readonly ISampleProvider _amplitudeSource;

        private AudioChannelBuffer _amplitudeBuffer;
        private AudioChannelBuffer _frequencyBuffer;

        public SineGenerator(OutputFormat outputFormat, ISampleProvider frequencySource, ISampleProvider amplitudeSource)
        {
            // todo how do we handle multiple channels here?
            _sampleRate = outputFormat.SampleRate;
            _frequencySource = frequencySource;
            _amplitudeSource = amplitudeSource;
        }

        public int Read(AudioChannelBuffer channelBuffer)
        {
            if (_amplitudeBuffer == null || _amplitudeBuffer.Length != channelBuffer.Length)
            {
                _amplitudeBuffer = AudioChannelBuffer.CreateFromRawBuffer(new float[channelBuffer.Length], 0, channelBuffer.Length);
            }

            if (_frequencyBuffer == null || _frequencyBuffer.Length != channelBuffer.Length)
            {
                _frequencyBuffer = AudioChannelBuffer.CreateFromRawBuffer(new float[channelBuffer.Length], 0, channelBuffer.Length);
            }

            _amplitudeSource.Read(_amplitudeBuffer);
            _frequencySource.Read(_frequencyBuffer);

            for (int i = 0; i < channelBuffer.Length; i++)
            {
                var frequency = _frequencyBuffer[i];
                var amplitude = _amplitudeBuffer[i];

                if (HasFrequencyChanged(frequency))
                {
                    UpdatePhaseShift(frequency);
                }

                EnsureMinimalSampleIndex(frequency);
                var sample = amplitude * CalculateSample(frequency);

                _previousSampleFrequency = frequency;
                ++_sampleIndex;

                channelBuffer[i] = sample;
            }

            return channelBuffer.Length;
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