using System;
using NAudio.Midi;

namespace Synthesizer.NAudio
{
    public class MidiDataSampleSource : ISampleSource
    {
        private readonly MidiIn _midiInput;
        private float _currentFrequency = 440.0f;

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

        public float ReadNextSample()
        {
            return _currentFrequency;
        }
    }
}