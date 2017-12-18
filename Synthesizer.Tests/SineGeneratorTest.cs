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
            ISampleSource generator = new SineGenerator(
                44100, 
                new ConstantSampleSource(440.0f),
                new ConstantSampleSource(1.0f)
            );
            var buffer = new float[22050];

            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = generator.ReadNextSample();
            }

            var actual = ToReadableString(buffer);
            var expected = File.ReadAllText(GetTestFileName());

            Assert.Equal(expected, actual);
        }

        private string GetTestFileName(
            string kind = "approved",
            [CallerFilePath] string callerFilePath = null,
            [CallerMemberName] string callerMemberName = null
        )
        {
            var directoryName = Path.GetDirectoryName(callerFilePath);
            if (directoryName == null)
            {
                throw new ArgumentException(nameof(callerFilePath));
            }
            
            var fileName = GetType().Name + "." + callerMemberName + "." + kind + ".txt";
            
            return Path.Combine(directoryName, fileName);
        }

        private static string ToReadableString(IEnumerable<float> buffer)
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
