using CProcessorAssembler;
using System.IO;
using Xunit;

namespace CProcessorAssemblerTest
{
    public class AssembleTest
    {
        private const string TEST_BASE_DIR = @"D:\univ\expc\CProcessorAssembler\CProcessorAssemblerTest\TestData";

        [Theory]
        [InlineData("alu.txt")]
        [InlineData("j.txt")]
        [InlineData("ld.txt")]
        [InlineData("nop.txt")]
        [InlineData("setix.txt")]
        [InlineData("st.txt")]
        [InlineData("mult.txt")]
        public void ValidSourceFileTest(string srcFileName)
        {
            var assembler = new Assembler();
            var srcPath = Path.Combine(TEST_BASE_DIR, "src", srcFileName);
            var memFileName = Path.GetFileNameWithoutExtension(srcFileName) + ".mem";
            var memPath = Path.Combine(TEST_BASE_DIR, "out", memFileName);
            assembler.Execute(srcPath, memPath);

            var ansPath = Path.Combine(TEST_BASE_DIR, "mem", memFileName);
            using var ansReader = new StreamReader(ansPath);
            using var memReader = new StreamReader(memPath);
            while (!ansReader.EndOfStream || !memReader.EndOfStream)
            {
                var srcLine = ansReader.ReadLine();
                var memLine = memReader.ReadLine();
                Assert.Equal(srcLine, memLine, ignoreWhiteSpaceDifferences: true);
            }
        }
    }
}