using IFix;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public void Dump() { 
            //Gets all types which are declared in the Main Module of "MyLibrary.dll"
            foreach (TypeDefinition type in assembly.MainModule.Types)
            {
                //Writes the full name of a type
                Console.WriteLine("Type: " + type.GetAssemblyQualifiedName());
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

        public bool HasType(string typeFullName, bool skipAssemblyQualified = false)
        {
            return GetType(typeFullName, skipAssemblyQualified) != null;
        }

        private TypeDefinition GetType(string typeFullName, bool skipAssemblyQualified = false)
        {
            if (assembly != null)
            {
                List<TypeDefinition> types = assembly.GetAllType();
                foreach (TypeDefinition type in types)
                {
                    if (IsTypeEquals(type, typeFullName, skipAssemblyQualified))
                    {
                        return type; // found
                    }
                }
            }
            return null;
        }

        private static bool IsTypeEquals(TypeDefinition typeDefinition, string typeFullName, bool skipAssemblyQualified = false)
        {
            if (skipAssemblyQualified)
            {
                return typeDefinition.FullName == StripAssemblyName(typeFullName);
            }
            else
            {
                return typeDefinition.GetAssemblyQualifiedName(null, false) == typeFullName;
            }
        }

        #endregion

        #region Method

        public bool HasMethod(IFixExternMethod method, bool skipAssemblyQualified = false)
        {
            return GetMethod(method, skipAssemblyQualified) != null;
        }
 
        private MethodDefinition GetMethod(IFixExternMethod method, bool skipAssemblyQualified = false)
        {
            TypeDefinition typeDefinition = GetType(method.declaringType, skipAssemblyQualified);
            if (typeDefinition != null)
            {
                foreach (MethodDefinition methodDefinition in typeDefinition.Methods)
                {
                    if (IsMethodEquals(methodDefinition, method, skipAssemblyQualified)) {
                        return methodDefinition;
                    }
                }
            }

            return null;
        }

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

        private static bool IsMethodEquals(MethodDefinition methodDefinition, IFixExternMethod externMethod, bool skipAssemblyQualified = false)
        {
            if (methodDefinition != null && externMethod != null)
            {
                if (IsTypeEquals(methodDefinition.DeclaringType, externMethod.declaringType, skipAssemblyQualified)
                    && methodDefinition.Name == externMethod.methodName
                    && methodDefinition.IsGenericInstance == externMethod.isGenericInstance
                    && methodDefinition.GenericParameters.Count == (externMethod.genericArgs != null ? externMethod.genericArgs.Length : 0)
                    && methodDefinition.Parameters.Count == (externMethod.parameters != null ? externMethod.parameters.Length : 0))
                {
                    for (int i = 0; i < methodDefinition.GenericParameters.Count; i++)
                    {
                        GenericParameter genericParameter = methodDefinition.GenericParameters[i];
                        string genericArg = StripAssemblyName(externMethod.genericArgs[i]);
                        if (genericParameter.FullName != genericArg) // TODO: IsTypeEquals?
                        {
                            return false; 
                        }
                    }
                    for (int i = 0; i < methodDefinition.Parameters.Count; i++)
                    {
                        ParameterDefinition parameterDefinition = methodDefinition.Parameters[i];
                        IFIxParameter parameter = externMethod.parameters[i];
                        if (skipAssemblyQualified)
                        {
                            if (parameterDefinition.ParameterType.FullName != StripAssemblyName(parameter.declaringType)) {
                                return false; 
                            }
                        }
                        else
                        {
                            string parameterTypeName;
                            if (parameter.isGeneric)
                            {
                                if (parameterDefinition.ParameterType.IsGenericParameter)
                                {
                                    parameterTypeName = parameterDefinition.ParameterType.Name;
                                }
                                else
                                {
                                    parameterTypeName = parameterDefinition.ParameterType.GetAssemblyQualifiedName(methodDefinition.DeclaringType, true);
                                }
                            }
                            else
                            {
                                if (parameterDefinition.ParameterType.IsGenericParameter)
                                {
                                    parameterTypeName = (parameterDefinition.ParameterType as GenericParameter).ResolveGenericArgument(methodDefinition.DeclaringType).GetAssemblyQualifiedName(); // TODO:?
                                }
                                else
                                {
                                    parameterTypeName = parameterDefinition.ParameterType.GetAssemblyQualifiedName(); // TODO:?
                                }
                            }

                            if (parameterTypeName != parameter.declaringType)
                            {
                                return false;
                            }
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
            TypeDefinition typeDefinition = GetType(field.declaringType, skipAssemblyQualified);
            if (typeDefinition != null)
            {
                foreach (FieldDefinition fieldDefinition in typeDefinition.Fields)
                {
                    if (IsFieldEquals(fieldDefinition, field, skipAssemblyQualified)) {
                        return fieldDefinition;
                    }
                }
            }

            return null;
        }

        private static bool IsFieldEquals(FieldDefinition fieldDefinition, IFixFieldInfo fieldInfo, bool skipAssemblyQualified = false)
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
