using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Text;

namespace FixIFix
{
    class AssemblyReader
    {

        public AssemblyReader()
        {

        }
        public void Read(String assmeblyPath)
        {
            AssemblyDefinition assembly = null;
            bool readSymbols = true;
            try
            {
                assembly = AssemblyDefinition.ReadAssembly(assmeblyPath,
                    new ReaderParameters { ReadSymbols = true });
            }
            catch
            {
                Console.WriteLine("Warning: read " + assmeblyPath + " with symbol fail");
                readSymbols = false;
                assembly = AssemblyDefinition.ReadAssembly(assmeblyPath,
                    new ReaderParameters { ReadSymbols = false });
            }

            var resolver = assembly.MainModule.AssemblyResolver as BaseAssemblyResolver;

            //Gets all types which are declared in the Main Module of "MyLibrary.dll"
            foreach (TypeDefinition type in assembly.MainModule.Types)
            {
                //Writes the full name of a type
                Console.WriteLine("Type: " + type.FullName);
                if (type.Name != "<Module>")
                {
                    //Gets all methods of the current type
                    foreach (MethodDefinition method in type.Methods)
                    {
                        Console.WriteLine(method.FullName);
                    }
                    foreach (PropertyDefinition prop in type.Properties)
                    {
                        Console.WriteLine(prop.GetType().FullName + " " + prop.FullName);
                    }
                }
            }
        }
    }
}
