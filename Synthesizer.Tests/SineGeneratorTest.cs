using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;

namespace Synthesizer.Tests
{
    public class SineGeneratorTest
    {
        [Fact]
        public void TestGeneratesASineInEmptyBuffer()
        {
            ISampleSource generator = new SineGenerator(44100) {Frequency = 440};
            float[] buffer = new float[22050];

            for (int i = 0; i < buffer.Length; i++)
            {
                buffer[i] = generator.ReadNextSample();
            }

            string actual = ToReadableString(buffer);
            string expected = File.ReadAllText(GetTestFileName());

            Assert.Equal(expected, actual);
        }

        private string GetTestFileName(
            string kind = "approved",
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null
        )
        {
            string directoryName = Path.GetDirectoryName(callerFilePath);
            string fileName = GetType().Name + "." + callerMemberName + "." + kind + ".txt";

            return Path.Combine(directoryName, fileName);
        }

        private string ToReadableString(IEnumerable<float> buffer)
        {
            var builder = new StringBuilder(22050 * 14);

            foreach (var f in buffer)
            {
                builder.AppendLine(f.ToString("F10"));
            }

            return builder.ToString();
        }
    }
}
