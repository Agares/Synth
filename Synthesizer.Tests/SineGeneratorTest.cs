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
            ISampleProvider generator = new SineGenerator(
                new OutputFormat(new SampleRate(44100), 1), 
                new ConstantSampleSource(440.0f),
                new ConstantSampleSource(1.0f)
            );
            var buffer = new float[22050];
            var acb = AudioChannelBuffer.CreateFromRawBuffer(buffer, 0, buffer.Length);

            generator.Read(acb);

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
