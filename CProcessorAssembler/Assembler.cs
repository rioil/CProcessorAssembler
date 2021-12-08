using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using CProcessorAssembler.Commands;
using CProcessorAssembler.Utils;

namespace CProcessorAssembler
{
    public class Assembler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcPath"></param>
        /// <param name="outputPath"></param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="FormatException"></exception>
        public void Execute(string srcPath, string outputPath)
        {
            ReadCommands(srcPath, out var commands, out var labels);
            CreateMemoryImage(outputPath, commands, labels);
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
                    if (string.IsNullOrWhiteSpace(line)) {
                        Console.WriteLine($"Skipping empty line (l.{lineNo})");
                        continue;
                    }
                    else if (line.StartsWith("--")) {
                        Console.WriteLine($"Skipping comment line (l.{lineNo})");
                        continue;
                    }
                    else if (line.StartsWith(":")) {
                        processingLabelNames.Add(line[1..]);
                        continue;
                    }

                    var tokens = line.Split(' ');
                    var ope = tokens[0].ToUpper();
                    Command command;
                    switch (ope) {
                        case "SETIX":
                            command = new SETIX(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "SETIXH":
                            command = new SETIXH(ParseByteOperand(), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "SETIXL":
                            command = new SETIXL(ParseByteOperand(), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "LDIA":
                            command = new LDIA(ParseByteOperand(), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "LDIB":
                            command = new LDIB(ParseByteOperand(), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "LDDA":
                            command = new LDDA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "LDDB":
                            command = new LDDB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "STDA":
                            command = new STDA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "STDB":
                            command = new STDB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "STDI":
                            command = new STDI(ParseByteOperand(), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "ADDA":
                            command = new ADDA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "SUBA":
                            command = new SUBA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "ANDA":
                            command = new ANDA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "ORA":
                            command = new ORA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "NOTA":
                            command = new NOTA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "INCA":
                            command = new INCA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "DECA":
                            command = new DECA(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "ADDB":
                            command = new ADDB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "SUBB":
                            command = new SUBB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "ANDB":
                            command = new ANDB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "ORB":
                            command = new ORB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "NOTB":
                            command = new NOTB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "INCB":
                            command = new INCB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "DECB":
                            command = new DECB(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "CMP":
                            command = new CMP(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "NOP":
                            command = new NOP(string.Join(" ", tokens.Skip(1)));
                            break;
                        case "JP":
                            command = new JP(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "JPC":
                            command = new JPC(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2)));
                            break;
                        case "JPZ":
                            command = new JPZ(tokens.ElementAtOrDefault(1), string.Join(" ", tokens.Skip(2)));
                            break;
                        default:
                            throw new NotSupportedException($"Operator {ope} is not supported.");
                    }

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

        private void CreateMemoryImage(string path, IEnumerable<Command> commands, Dictionary<string, Command> labels)
        {
            int address = 0;
            using var writer = new StreamWriter(path, false);
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

    public enum EEndianness
    {
        Little,
        Big,
    }
}
