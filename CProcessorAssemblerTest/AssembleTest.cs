using CProcessorAssembler;
using CProcessorAssembler.Generators;
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
        public void ValidSourceFileAssembleTest(string srcFileName)
        {
            var srcPath = CreateSrcPath(srcFileName);
            var memPath = CreateMemPath(srcFileName);
            var ansPath = CreateAnsPath(srcFileName);

            // Generatorには拡張子なしのファイルパスを渡す
            var extension = Path.GetExtension(srcPath);
            var filePathWitoutExtension = memPath.Remove(memPath.Length - extension.Length, extension.Length);
            var generator = new MemGenerator(filePathWitoutExtension);

            var assembler = new Assembler();
            assembler.Execute(srcPath, generator);

            AssertMemOutput(memPath, ansPath);
        }

        [Theory]
        [InlineData("alu.txt")]
        [InlineData("j.txt")]
        [InlineData("ld.txt")]
        [InlineData("nop.txt")]
        [InlineData("setix.txt")]
        [InlineData("st.txt")]
        [InlineData("mult.txt")]
        public void AppRunTest(string srcFileName)
        {
            var srcPath = CreateSrcPath(srcFileName);
            var memPath = CreateMemPath(srcFileName);
            var ansPath = CreateAnsPath(srcFileName);

            Program.Main(new string[] { srcPath, memPath });

            AssertMemOutput(memPath, ansPath);
        }


        private void AssertMemOutput(string memPath, string ansPath)
        {
            using var ansReader = new StreamReader(ansPath);
            using var memReader = new StreamReader(memPath);
            while (!ansReader.EndOfStream || !memReader.EndOfStream) {
                var srcLine = ansReader.ReadLine();
                var memLine = memReader.ReadLine();
                Assert.Equal(srcLine, memLine, ignoreWhiteSpaceDifferences: true);
            }
        }

        private string CreateSrcPath(string srcFileName)
        {
            return Path.Combine(TEST_BASE_DIR, "src", srcFileName);
        }

        private string CreateMemPath(string srcFileName)
        {
            var memFileName = Path.GetFileNameWithoutExtension(srcFileName) + ".mem";
            return Path.Combine(TEST_BASE_DIR, "out", memFileName);
        }

        private string CreateAnsPath(string srcFileName)
        {
            var ansFileName = Path.GetFileNameWithoutExtension(srcFileName) + ".mem";
            return Path.Combine(TEST_BASE_DIR, "mem", ansFileName);
        }
    }
}