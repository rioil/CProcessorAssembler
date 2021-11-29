using System;
using System.IO;
using System.Linq;

namespace CProcessorAssembler
{
    class Program
    {
        const string OUTPUT_EXTENSION = "mem";


        static int Main(string[] args)
        {
            Console.WriteLine("C-Processor Assembler");

            // ソースファイルの存在確認
            string srcPath = string.Empty;
            string outputPath;
            if (!CheckAndSetSrcPath()) { return -1; }
            if (!CheckAndSetOutputPath()) { return -1; }

            var assembler = new Assembler();
            try
            {
                assembler.Execute(srcPath, outputPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Occured: {ex.Message}");
                return -1;
            }

            Console.WriteLine("Completed!");
            return 0;

            bool CheckAndSetSrcPath()
            {
                if(args.Length == 0) {
                    Console.WriteLine("ソースファイルを指定してください．");
                    return false;
                }

                srcPath = args[0];

                if (!File.Exists(srcPath))
                {
                    Console.WriteLine($"指定されたソースファイル {srcPath} が存在しません．");
                    return false;
                }

                return true;
            }

            bool CheckAndSetOutputPath()
            {
                if(args.Length >= 2)
                {
                    outputPath = args[1];
                }
                else
                {
                    outputPath = Path.Combine(
                        Path.GetDirectoryName(srcPath) ?? string.Empty,
                        $"{Path.GetFileNameWithoutExtension(srcPath)}.{OUTPUT_EXTENSION}");
                }
                
                if (File.Exists(outputPath))
                {
                    Console.WriteLine($"{outputPath} に上書き出力します．");
                }

                return true;
            }
        }
    }
}
