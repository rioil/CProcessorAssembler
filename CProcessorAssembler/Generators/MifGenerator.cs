using CProcessorAssembler.Commands;
using System.Collections.Generic;
using System.IO;

namespace CProcessorAssembler.Generators
{
    /// <summary>
    /// FPGAボードへの書き込みに用いるメモリ初期化ファイル(.mif)を作成するクラスです．
    /// </summary>
    public class MifGenerator : Generator
    {
        public MifGenerator(string fileName)
        {
            OutputPath = $"{fileName}.mif";
        }

        internal override void Generate(IEnumerable<Command> commands, Dictionary<string, Command> labels)
        {
            base.Generate(commands, labels);

            using var writer = new StreamWriter(OutputPath, false);
            WriteHeader(writer);
            WriteContent(writer, commands, labels);
        }

        /// <summary>
        /// ヘッダーを出力します．
        /// </summary>
        /// <param name="writer"></param>
        private static void WriteHeader(StreamWriter writer)
        {
            const string header = "WIDTH = 8;\nDEPTH = 1024;\n\nADDRESS_RADIX = HEX;\nDATA_RADIX = HEX;\n";
            writer.WriteLine(header);
        }

        /// <summary>
        /// メモリの内容を出力します．
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="commands"></param>
        /// <param name="labels"></param>
        private static void WriteContent(StreamWriter writer, IEnumerable<Command> commands, Dictionary<string, Command> labels)
        {
            writer.WriteLine("CONTENT BEGIN");

            int address = 0;
            foreach (var command in commands) {
                var bytes = command.ToBytes(labels);
                var comment = command.Comment;
                foreach (var data in bytes) {
                    writer.WriteLine($"\t{address++:X3}  :   {data:X2};");
                }
            }

            if (address <= 0x3FF) {
                writer.WriteLine($"\t[{address:X3}..3FF]  :   00;");
            }

            writer.WriteLine("END;");
        }
    }
}
