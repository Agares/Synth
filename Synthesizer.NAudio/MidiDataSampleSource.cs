using System;
using NAudio.Midi;

namespace Synthesizer.NAudio
{
    public class MidiDataSampleSource : ISampleProvider
    {
        private readonly MidiIn _midiInput;
        private volatile float _currentFrequency = 440.0f;

        public MidiDataSampleSource(MidiIn midiInput)
        {
            _midiInput = midiInput;

            _midiInput.MessageReceived += (sender, args) =>
            {
                if (args.MidiEvent.CommandCode == MidiCommandCode.NoteOn)
                {
                    var @event = (NoteOnEvent) args.MidiEvent;
                    _currentFrequency = Note.FromMidiNote(@event.NoteNumber).Frequency;
                }
            };
            _midiInput.ErrorReceived += (sender, args) => throw new Exception("Oh noes"); // todo some sane error handling i guess?
        }

        public int Read(AudioChannelBuffer channelBuffer)
        {
            for (int i = 0; i < channelBuffer.Length; i++)
            {
                channelBuffer[i] = _currentFrequency;
            }

            return channelBuffer.Length;
        }
    }
}