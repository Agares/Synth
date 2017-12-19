using System;

namespace Synthesizer
{
    public sealed class Note
    {
        public float Frequency { get; }
        
        private Note(int octave, Notes note)
        {
            var noteDiff = (int) note - (int) Notes.A - 12 * (4-octave);
            
            Frequency = (float) (440.0 / Math.Pow(2, -(noteDiff/12.0)));
        }
        
        public static Note FromOctaveNote(int octave, Notes note)
        {
            return new Note(octave, note);
        }

        public static Note FromMidiNote(int midiNote)
        {
            return new Note(midiNote/12, (Notes)(midiNote % 12)); 
        }
    }
}