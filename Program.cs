using System;

namespace FixIFix
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("FixIFix.exe dumppatch <patch_file>");
            Console.WriteLine("            dumpdll <assmbly_path>");
            Console.WriteLine("            check <patch_file> <apk_file>");
        }

        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintUsage();
                return;
            }
            string command = args[0];
            string filepath = args[1];
            if (command == "dumppatch")
            {
                DumpPatch(filepath);
            } else if (command == "dumpdll")
            {
                DumpDll(filepath);
            } else if (command == "check") 
            {
                if (args.Length < 3)
                {
                    PrintUsage();
                    return;
                }
                string apkFile = args[2];
                Check(filepath, apkFile);
            } else
            {
                PrintUsage();
            }
        }

        static void DumpPatch(string filepath)
        {
            PatchReader reader = new PatchReader(filepath);
            reader.Read();
        }

        static void DumpDll(string filepath)
        {
            // TODO:
        }

        static void Check(string patchFile, string apkFile)
        {
            // TODO:
        }
    }
}
