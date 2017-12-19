using Xunit;

namespace Synthesizer.Tests
{
    public class NoteTest
    {
        [Theory]
        [InlineData(4, Notes.A, 440.0f)]
        [InlineData(3, Notes.A, 220.0f)]
        [InlineData(2, Notes.A, 110.0f)]
        [InlineData(5, Notes.A, 880.0f)]
        [InlineData(6, Notes.A, 1760.0f)]
        [InlineData(4, Notes.ASharp, 466.163761518f)]
        [InlineData(4, Notes.GSharp, 415.30469758f)]
        [InlineData(0, Notes.GSharp, 25.9565435987f)]
        [InlineData(5, Notes.C, 523.251130601f)]
        public void CanConvertOctaveNoteToFrequency(int octave, Notes noteI, float frequency)
        {
            var note = Note.FromOctaveNote(octave, noteI);
            
            Assert.Equal(frequency, (float)note.Frequency, 5);
        }

        [Theory]
        [InlineData(0, 16.3515978313f)]  // C0
        [InlineData(12, 32.7031956626f)] // C1
        [InlineData(11, 30.8677063285f)] // B0
        [InlineData(1, 17.323914436f)]   // C#0
        public void CanConvertMidiNoteToFrequency(int midiNote, float frequency)
        {
            var note = Note.FromMidiNote(midiNote);
            
            Assert.Equal(frequency, note.Frequency);
        }
    }
}