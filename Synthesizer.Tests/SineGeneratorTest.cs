using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xunit;

namespace Synthesizer.Tests
{
    public class SineGeneratorTest
    {
        [Fact]
        public void TestGeneratesASineInEmptyBuffer()
        {
            var generator = new SineGenerator(2, 44100) {Frequency = 440};
            float[] buffer = new float[44100];
            var audioBuffer = AudioBuffer.CreateFromRawBuffer(2, buffer, 0, buffer.Length);

            generator.Read(audioBuffer);

            string actual = ToReadableString(buffer);
            string expected = File.ReadAllText(
                GetTestFileName()
            );

            Assert.Equal(expected, actual);
        }

        private string GetTestFileName(
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null
        )
        {
            string directoryName = Path.GetDirectoryName(callerFilePath);
            string fileName = GetType().Name + "." + callerMemberName + ".approved.txt";

            return Path.Combine(directoryName, fileName);
        }

        private string ToReadableString(IEnumerable<float> buffer)
        {
            return buffer
                .Aggregate("", (s, f) => s + f.ToString("F10") + Environment.NewLine);
        }
    }
}
