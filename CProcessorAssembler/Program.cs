using CProcessorAssembler.Generators;
using System;
using System.IO;
using System.Linq;

namespace CProcessorAssembler
{
    public class Program
    {
        const string OUTPUT_EXTENSION = "mem";


        public static int Main(string[] args)
        {
            Console.WriteLine("C-Processor Assembler");

            // ソースファイルの存在確認
            string srcPath = string.Empty;
            if (!CheckAndSetSrcPath()) { return -1; }

            if (!CheckAndSetOutputPathWitoutExtension()) { return -1; }

            var assembler = new Assembler();
            string outputPathWithoutExtension;
            var memGenerator = new MemGenerator(outputPathWithoutExtension);
            var mifGenerator = new MifGenerator(outputPathWithoutExtension);
            try {
                assembler.Execute(srcPath, memGenerator, mifGenerator);
            }
            catch (Exception ex) {
                Console.WriteLine($"Error Occured: {ex.Message}");
                return -1;
            }

            Console.WriteLine("Completed!");
            return 0;

            bool CheckAndSetSrcPath()
            {
                if (args.Length == 0) {
                    Console.WriteLine("ソースファイルを指定してください．");
                    return false;
                }

                srcPath = args[0];

                if (!File.Exists(srcPath)) {
                    Console.WriteLine($"指定されたソースファイル {srcPath} が存在しません．");
                    return false;
                }

                return true;
            }

            bool CheckAndSetOutputPathWitoutExtension()
            {
                outputPathWithoutExtension = Path.Combine(
                    Path.GetDirectoryName(args.ElementAtOrDefault(1)) ?? string.Empty,
                    Path.GetFileNameWithoutExtension(srcPath));

                // 出力先フォルダが存在しなければ作成
                var outputDir = Path.GetDirectoryName(outputPathWithoutExtension);
                if (!string.IsNullOrEmpty(outputDir)) {
                    Directory.CreateDirectory(outputDir);
                }

                return true;
            }
        }
    }
}
