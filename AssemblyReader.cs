using IFix;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FixIFix
{
    public class AssemblyReader
    {

        public AssemblyReader()
        {

        }
        public void Read(string assmeblyPath, string assmeblySearchDirectory = null)
        {
            try
            {
                assembly = AssemblyDefinition.ReadAssembly(assmeblyPath,
                    new ReaderParameters { ReadSymbols = true });
                readSymbols = true;
            }
            catch
            {
                Console.WriteLine("Warning: read " + assmeblyPath + " with symbol fail");
                readSymbols = false;
                assembly = AssemblyDefinition.ReadAssembly(assmeblyPath,
                    new ReaderParameters { ReadSymbols = false });
            }

            resolver = assembly.MainModule.AssemblyResolver as BaseAssemblyResolver;
            if (assmeblySearchDirectory == null)
            {
                assmeblySearchDirectory = System.IO.Path.GetDirectoryName(assmeblyPath);
            }
            resolver.AddSearchDirectory(assmeblySearchDirectory);
        }

        public void Dump() { 
            //Gets all types which are declared in the Main Module of "MyLibrary.dll"
            foreach (TypeDefinition type in assembly.GetAllType())
            {
                //Writes the full name of a type
                Console.WriteLine("Type: " + type.GetAssemblyQualifiedName() + ", FullName=" + type.FullName);
                if (type.Name != "<Module>")
                {
                    //Gets all methods of the current type
                    foreach (MethodDefinition method in type.Methods)
                    {
                        Console.WriteLine("\tMethod: " + method.FullName);
                    }
                    foreach (FieldDefinition field in type.Fields)
                    {
                        Console.WriteLine("\tField: " + field.FieldType.FullName + " " + field.Name);
                    }
                    Console.WriteLine("");
                }
            }
        }

        #region Type

        public bool HasType(string typeFullName, bool ignoreAssemblyName = false)
        {
            return GetType(typeFullName, ignoreAssemblyName) != null;
        }

        public TypeReference GetType(string typeFullName, bool ignoreAssemblyName = false)
        {
            if (assembly != null)
            {
                foreach (ModuleDefinition module in assembly.Modules)
                {
                    TypeReference type = module.GetType(typeFullName, !ignoreAssemblyName  /* runtimeName */);
                    if (type != null)
                    {
                        return type;                
                    }
                }
            }
            return null;
        }

        private bool IsTypeEquals(TypeReference typeDefinition, string typeFullName, bool ignoreAssemblyName = false)
        {
            TypeReference type = GetType(typeFullName, ignoreAssemblyName);
            if (type == null || typeDefinition == null)
            {
                return false;
            }
            return typeDefinition.FullName == type.FullName;
        }

        #endregion

        #region Method

        public bool HasMethod(IFixExternMethod method, bool ignoreAssemblyName = false)
        {
            return GetMethod(method, ignoreAssemblyName) != null;
        }
 
        private MethodDefinition GetMethod(IFixExternMethod method, bool ignoreAssemblyName = false)
        {
            TypeReference typeRef = GetType(method.declaringType, ignoreAssemblyName);
            if (typeRef != null)
            {
                TypeDefinition targetTypeDefinition = null;
                if (typeRef.IsGenericInstance)
                {
                    GenericInstanceType genericIns = ((GenericInstanceType)typeRef);
                    TypeReference genericTypeRef = genericIns.ElementType;
                    try
                    {
                        targetTypeDefinition = genericTypeRef.Resolve();
                    } catch (Exception ignore) { 
                    }
                }
                else if (typeRef.IsDefinition)
                {
                    try
                    {
                        targetTypeDefinition = typeRef.Resolve();
                    } catch (Exception ignore) {
                    }
                } 
                else if (typeRef.IsNested)
                {
                    try
                    {
                        targetTypeDefinition = typeRef.Resolve();
                    }
                    catch (Exception ignore)
                    {
                    }
                }
                else  
                {
                    try
                    {
                        targetTypeDefinition = typeRef.Resolve();
                    }
                    catch (Exception ignore)
                    {
                    }
                }

                if (targetTypeDefinition != null)
                {
                    foreach (MethodDefinition methodDefinition in targetTypeDefinition.Methods)
                    {
                        if (IsMethodEquals(methodDefinition, method, ignoreAssemblyName))
                        {
                            return methodDefinition;
                        }
                    }
                }
            }

            return null;
        }

        // e.g.:
        // qualifiedName = AutoResolution+LastType, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null
        // return AutoResolution+LastType
        private static string StripAssemblyName(string qualifiedName)
        {
            if (qualifiedName != null)
            {
                int index = qualifiedName.IndexOf(",");
                if (index != -1)
                {
                    return qualifiedName.Substring(0, index);
                }
            }
            return qualifiedName;
        }

        private bool IsMethodEquals(MethodDefinition methodDefinition, IFixExternMethod externMethod, bool ignoreAssemblyName = false)
        {
            if (methodDefinition != null && externMethod != null)
            {
                if (methodDefinition.Name == externMethod.methodName
                    //&& methodDefinition.IsGenericInstance == externMethod.isGenericInstance // TODO: System.Array.Empty() methodDefinition.IsGenericInstance = false
                    && methodDefinition.GenericParameters.Count == (externMethod.genericArgs != null ? externMethod.genericArgs.Length : 0)
                    && methodDefinition.Parameters.Count == (externMethod.parameters != null ? externMethod.parameters.Length : 0)) {
                    //if (!IsTypeEquals(methodDefinition.DeclaringType, externMethod.declaringType, ignoreAssemblyName)) {
                    //    return false;
                    //}

                    for (int i = 0; i < methodDefinition.GenericParameters.Count; i++)
                    {
                        GenericParameter genericParameter = methodDefinition.GenericParameters[i];
                        string genericArg = externMethod.genericArgs[i];
                        if (!IsTypeEquals(genericParameter, genericArg))
                        {
                            return false;
                        }
                    }

                    for (int i = 0; i < methodDefinition.Parameters.Count; i++)
                    {
                        ParameterDefinition parameterDefinition = methodDefinition.Parameters[i];
                        IFIxParameter parameter = externMethod.parameters[i];
                        if (!IsTypeEquals(parameterDefinition.ParameterType, parameter.declaringType))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region Field

        public bool HasField(IFixFieldInfo field, bool skipAssemblyQualified = false)
        {
            return GetField(field, skipAssemblyQualified) != null;
        }

        private FieldDefinition GetField(IFixFieldInfo field, bool skipAssemblyQualified = false)
        {
            var typeDefinition = GetType(field.declaringType, skipAssemblyQualified);
            if (typeDefinition != null && typeDefinition.IsDefinition)
            {
                foreach (FieldDefinition fieldDefinition in ((TypeDefinition)typeDefinition).Fields)
                {
                    if (IsFieldEquals(fieldDefinition, field, skipAssemblyQualified)) {
                        return fieldDefinition;
                    }
                }
            }

            return null;
        }

        private bool IsFieldEquals(FieldDefinition fieldDefinition, IFixFieldInfo fieldInfo, bool skipAssemblyQualified = false)
        {
            if (fieldDefinition != null && fieldInfo != null)
            {
                return IsTypeEquals(fieldDefinition.DeclaringType, fieldInfo.declaringType, skipAssemblyQualified)
                    && fieldDefinition.Name == fieldInfo.fieldName; // NOTE: no need to check the fieldType
            }
            return false;
        }

        #endregion

        private AssemblyDefinition assembly;
        private BaseAssemblyResolver resolver;
        private bool readSymbols = false;
    }
}
