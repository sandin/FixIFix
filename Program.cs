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
            Console.WriteLine("            check <patch_file> <assmblies_path>");
        }

        static int Main(string[] args)
        {
            if (args.Length < 2)
            {
                PrintUsage();
                return 1;
            }
            string command = args[0];
            string filepath = args[1];
            if (command == "dumppatch")
            {
                return DumpPatch(filepath);
            } else if (command == "dumpdll")
            {
                return DumpDll(filepath);
            } else if (command == "check") 
            {
                if (args.Length < 3)
                {
                    PrintUsage();
                    return -1;
                }
                string apkFile = args[2];
                return Check(filepath, apkFile);
            } else
            {
                PrintUsage();
                return -1;
            }
        }

        static int DumpPatch(string patchFilePath)
        {
            PatchReader reader = new PatchReader();
            var patch = reader.Read(patchFilePath);
            PatchReader.Dump(patch);
            return 0;
        }

        static int DumpDll(string dllFilePath)
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read(dllFilePath);
            return 0;
        }

        static int Check(string patchFilePath, string apkFilePath)
        {
            // TODO:
            return 0;
        }
    }
}
