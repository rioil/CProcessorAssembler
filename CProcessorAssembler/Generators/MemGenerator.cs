using CProcessorAssembler.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace CProcessorAssembler.Generators
{
    /// <summary>
    /// ModelSimでのシミュレーションに用いるメモリファイル(.mem)を作成するクラスです．
    /// </summary>
    public class MemGenerator : Generator
    {
        public MemGenerator(string fileName)
        {
            OutputPath = $"{fileName}.mem";
        }

        internal override void Generate(IEnumerable<Command> commands, Dictionary<string, Command> labels)
        {
            base.Generate(commands, labels);

            int address = 0;
            using var writer = new StreamWriter(OutputPath, false);
            foreach (var command in commands) {
                var bytes = command.ToBytes(labels);
                var comment = command.Comment;
                foreach (var data in bytes) {
                    writer.Write(address.ToString("X4"));
                    writer.Write("\t\t");
                    writer.Write(data.ToString("X2"));
                    writer.WriteLine($"\t\t\t-- {comment}");
                    comment = string.Empty; // 最初の行だけコメントを出す
                    address++;
                }
            }
        }
    }
}
