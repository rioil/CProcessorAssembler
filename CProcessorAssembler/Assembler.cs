using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CProcessorAssembler.Commands;
using CProcessorAssembler.Utils;
using CProcessorAssembler.Generators;

namespace CProcessorAssembler
{
    public class Assembler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="generators"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FormatException"></exception>
        public void Execute(string srcPath, params Generator[] generators)
        {
            ReadCommands(srcPath, out var commands, out var labels);
            foreach (var generator in generators) {
                generator.Generate(commands, labels);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FormatException"></exception>
        private void ReadCommands(string srcPath, out List<Command> commands, out Dictionary<string, Command> labels)
        {
            var lines = File.ReadAllLines(srcPath);
            var lineNo = 1;
            var processingLabelNames = new List<string>();

            var commandList = new List<Command>();
            var labelList = new Dictionary<string, Command>();

            ushort address = 0;
            foreach (var line in lines) {
                try {
                    // 空行はスキップ
                    if (string.IsNullOrWhiteSpace(line)) {
                        Console.WriteLine($"Skipping empty line (l.{lineNo})");
                        continue;
                    }
                    // --始まりはコメント行として処理
                    else if (line.StartsWith("--")) {
                        Console.WriteLine($"Skipping comment line (l.{lineNo})");
                        continue;
                    }
                    // :以降をラベルとして処理（空白以降は無視）
                    else if (line.StartsWith(":")) {
                        processingLabelNames.Add(line.Split(' ')[0][1..]);
                        continue;
                    }

                    var tokens = line.Split(' ');
                    var ope = tokens[0].ToUpper();
                    Command command = ope switch
                    {
                        "SETIX" => new SETIX(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2))),
                        "SETIXH" => new SETIXH(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "SETIXL" => new SETIXL(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "LDIA" => new LDIA(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "LDIB" => new LDIB(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "LDDA" => new LDDA(string.Join(" ", tokens.Skip(1))),
                        "LDDB" => new LDDB(string.Join(" ", tokens.Skip(1))),
                        "STDA" => new STDA(string.Join(" ", tokens.Skip(1))),
                        "STDB" => new STDB(string.Join(" ", tokens.Skip(1))),
                        "STDI" => new STDI(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "ADDA" => new ADDA(string.Join(" ", tokens.Skip(1))),
                        "SUBA" => new SUBA(string.Join(" ", tokens.Skip(1))),
                        "ANDA" => new ANDA(string.Join(" ", tokens.Skip(1))),
                        "ORA" => new ORA(string.Join(" ", tokens.Skip(1))),
                        "NOTA" => new NOTA(string.Join(" ", tokens.Skip(1))),
                        "INCA" => new INCA(string.Join(" ", tokens.Skip(1))),
                        "DECA" => new DECA(string.Join(" ", tokens.Skip(1))),
                        "ADDB" => new ADDB(string.Join(" ", tokens.Skip(1))),
                        "SUBB" => new SUBB(string.Join(" ", tokens.Skip(1))),
                        "ANDB" => new ANDB(string.Join(" ", tokens.Skip(1))),
                        "ORB" => new ORB(string.Join(" ", tokens.Skip(1))),
                        "NOTB" => new NOTB(string.Join(" ", tokens.Skip(1))),
                        "INCB" => new INCB(string.Join(" ", tokens.Skip(1))),
                        "DECB" => new DECB(string.Join(" ", tokens.Skip(1))),
                        "CMP" => new CMP(string.Join(" ", tokens.Skip(1))),
                        "SLL" => new SLL(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "SRL" => new SRL(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "SLA" => new SLA(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "SRA" => new SRA(ParseByteOperand(), string.Join(" ", tokens.Skip(2))),
                        "NOP" => new NOP(string.Join(" ", tokens.Skip(1))),
                        "JP" => new JP(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2))),
                        "JPC" => new JPC(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2))),
                        "JPZ" => new JPZ(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2))),
                        _ => throw new NotSupportedException($"Operator {ope} is not supported."),
                    };

                    // アドレスの設定
                    command.MemoryAddress = address;
                    address += command.GetByteLen();

                    // コマンドリストに追加
                    commandList.Add(command);

                    // 処理待ちラベルの処理
                    processingLabelNames.ForEach(name => labelList.Add(name, command));
                    processingLabelNames.Clear();

                    // Byte型オペランドの解析を行うローカル関数
                    byte ParseByteOperand()
                    {
                        var operand = StringUtil.ParseNumericString(tokens?.ElementAtOrDefault(1));
                        if (operand < byte.MinValue || byte.MaxValue < operand) {
                            if (sbyte.MinValue <= operand || operand <= sbyte.MaxValue) {
                                var sbyteValue = Convert.ToSByte(operand);
                                return (byte)sbyteValue;
                            }
                            else {
                                throw new ArgumentOutOfRangeException(line);
                            }
                        }

                        return Convert.ToByte(operand);
                    }
                }
                finally {
                    lineNo++;
                }
            }

            commands = commandList;
            labels = labelList;
        }
    }

    public enum EEndianness
    {
        Little,
        Big,
    }
}
