using CProcessorAssembler.Commands;
using System;
using System.Collections.Generic;
using System.IO;

namespace CProcessorAssembler.Generators
{
    /// <summary>
    /// ファイル生成を行うクラスの基底クラス
    /// </summary>
    public abstract class Generator
    {
        /// <summary>
        /// 出力先ファイルパス
        /// </summary>
        public string OutputPath { get; protected set; } = default!;

        /// <summary>
        /// コマンドリストからファイルを生成します．
        /// </summary>
        /// <param name="commands">コマンドリスト</param>
        /// <param name="labels">ラベルリスト</param>
        internal virtual void Generate(IEnumerable<Command> commands, Dictionary<string, Command> labels)
        {
            if (File.Exists(OutputPath)) {
                Console.WriteLine($"{OutputPath} に上書き出力します．");
            }
        }
    }
}
