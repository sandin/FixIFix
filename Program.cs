using System;
using System.Collections.Generic;
using System.IO;

namespace FixIFix
{
    class Program
    {
        static void PrintUsage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("FixIFix.exe dumppatch <patch_file>");
            Console.WriteLine("            dumpdll <assmbly_path>");
            Console.WriteLine("            checkpatch <patch_file> <assemblies_path>");
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
            } else if (command == "checkpatch") 
            {
                if (args.Length < 3)
                {
                    PrintUsage();
                    return -1;
                }
                string assembliesPath = args[2];
                return CheckPatch(filepath, assembliesPath);
            } else
            {
                Console.WriteLine("Error: unknown command: " + command);
                PrintUsage();
                return -1;
            }
        }

        // cmd: "dumppatch ..\..\..\Test\data\Assembly-CSharp.patch.bytes"
        static int DumpPatch(string patchFilePath)
        {
            if (!File.Exists(patchFilePath))
            {
                Console.WriteLine("Error: file " + patchFilePath + " is not exists");
                return -1;
            }

            PatchReader reader = new PatchReader();
            reader.Read(patchFilePath);
            reader.Dump();
            return 0;
        }

        // cmd: "dumpdll ..\..\..\Test\data\Managed\Assembly-CSharp.dll"
        static int DumpDll(string dllFilePath)
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read(dllFilePath);
            reader.Dump();
            return 0;
        }

        // cmd: "checkpatch ..\..\..\Test\data\Assembly-CSharp.patch.bytes ..\..\..\Test\data\Managed"
        static int CheckPatch(string patchFilePath, string assembliesPath)
        {
            if (!File.Exists(patchFilePath))
            {
                Console.WriteLine("Error: patch file " + patchFilePath + " is not exists");
                return -1;
            }
            if (!Directory.Exists(assembliesPath))
            {
                Console.WriteLine("Error: assemblies directory " + assembliesPath + " is not exists");
                return -1;
            }

            // Parse assemblies
            string[] assembliesFileNames = Directory.GetFiles(assembliesPath, "*.dll");
            List<AssemblyReader> assemblies = new List<AssemblyReader>();
            foreach (string assemblyFileName in assembliesFileNames)
            {
                Console.WriteLine("Info : Load assembly: " + assemblyFileName);
                AssemblyReader assembly = new AssemblyReader();
                assembly.Read(assemblyFileName);
                assemblies.Add(assembly);
            }

            // Parse patch file
            PatchReader reader = new PatchReader();
            var patch = reader.Read(patchFilePath);
            if (patch == null)
            {
                Console.WriteLine("Error: can not parse patch file " + patchFilePath);
                return -1;
            }
            //reader.Dump();

            // Check types first
            List<string> errors = new List<string>();
            foreach (string typeFullName in patch.externTypes)
            {
                if (!HasTypeInAssemblies(typeFullName, assemblies)) {
                    errors.Add("Error: can not found type: `" + typeFullName + "`.");
                }
            }

            // Check methods
            foreach (IFixExternMethod externMethod in patch.externMethods)
            {
                if (!HasMethodInAssemblies(externMethod, assemblies)) {
                    errors.Add("Error: can not found method: `" + externMethod.methodName + "` in type: " + externMethod.declaringType);
                }
            }

            // Check fields
            foreach (IFixFieldInfo field in patch.fieldInfos)
            {
                if (field.isNewField)
                {
                    continue; // this field has been defined in patch file, no need to check it
                }
                if (!HasFieldInAssemblies(field, assemblies))
                {
                    errors.Add("Error: can not found field: `" + field.fieldName + "` in type: " + field.declaringType);
                }
            }

            // Print check result
            if (errors.Count > 0)
            {
                foreach (string error in errors)
                {
                    Console.WriteLine(error);
                }
                Console.WriteLine("Found " + errors.Count + " errors in patch file `" + patchFilePath + "`");
            } 
            else
            {
                Console.WriteLine("Passed");
            }

            return 0;
        }

        private static bool HasTypeInAssemblies(string typeFullName, List<AssemblyReader> assemblies)
        {
            bool found = false;
            foreach (AssemblyReader assembly in assemblies)
            {
                if (assembly.HasType(typeFullName, true))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private static bool HasMethodInAssemblies(IFixExternMethod method, List<AssemblyReader> assemblies)
        {
            bool found = false;
            foreach (AssemblyReader assembly in assemblies)
            {
                if (assembly.HasMethod(method, true))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        private static bool HasFieldInAssemblies(IFixFieldInfo field, List<AssemblyReader> assemblies)
        {
            bool found = false;
            foreach (AssemblyReader assembly in assemblies)
            {
                if (assembly.HasField(field, true))
                {
                    found = true;
                    break;
                }
            }
            return found;
        }
    }
}
