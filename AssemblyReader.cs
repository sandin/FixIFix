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
                TypeReference typeRef = assembly.MainModule.GetType(typeFullName, true);
                try
                {
                    return typeRef.Resolve();
                }
                catch (Exception ignore)
                {
                    return null;
                }

                /*
                List<TypeDefinition> types = assembly.GetAllType();
                foreach (TypeDefinition type in types)
                {
                    if (IsTypeEquals(type, typeFullName, ignoreAssemblyName))
                    {
                        return type; // found
                    }
                }
                */
            }
            return null;
        }

        public bool IsTypeEquals(TypeReference typeDefinition, string typeFullName, bool ignoreAssemblyName = false)
        {
            /*
            TypeDefinition typeDef = GetType(typeFullName) as TypeDefinition;
            if (typeDef != null)
            {
                return ignoreAssemblyName ? typeDefinition.IsSameName(typeDef) : typeDefinition.IsSameType(typeDef);
            }
            return false;
            */
            if (ignoreAssemblyName)
            {
                //return StripAssemblyName(typeDefinition.GetAssemblyQualifiedName()) == StripAssemblyName(typeFullName);
                TypeReference typeRef = assembly.MainModule.GetType(typeFullName, true);
                return typeDefinition.FullName == typeRef.FullName;
            }
            else
            {
                return typeDefinition.GetAssemblyQualifiedName(null, false) == typeFullName;
            }
        }

        #endregion

        #region Method

        public bool HasMethod(IFixExternMethod method, bool ignoreAssemblyName = false)
        {
            return GetMethod(method, ignoreAssemblyName) != null;
        }
 
        private MethodDefinition GetMethod(IFixExternMethod method, bool ignoreAssemblyName = false)
        {
            TypeReference typeRef = assembly.MainModule.GetType(method.declaringType, true);
            TypeDefinition typeDef = null;
            try
            {
                typeDef = typeRef.Resolve();
            }
            catch (Exception ignore)
            {
            }

            if (typeDef != null)
            {
                foreach (MethodDefinition methodDefinition in typeDef.Methods)
                {
                    if (IsMethodEquals(typeRef, methodDefinition, method, ignoreAssemblyName))
                    {
                        return methodDefinition;
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

        private bool IsMethodEquals(TypeReference typeReference /* TODO */, MethodDefinition methodDefinition, IFixExternMethod externMethod, bool ignoreAssemblyName = false)
        {
            if (methodDefinition != null && externMethod != null)
            {
                int externMethodParametersCount = externMethod.parameters != null ? externMethod.parameters.Length : 0;
                int externMethodgenericArgsCount = externMethod.genericArgs != null ? externMethod.genericArgs.Length : 0;
                if (externMethod.isGenericInstance)
                {
                    bool matched = methodDefinition.Name == externMethod.methodName
                        && methodDefinition.Parameters.Count == externMethodParametersCount;
                    if (!matched)
                    {
                        return false;
                    }

                    // compare GenericParameters
                    if (externMethodgenericArgsCount > 0)
                    {
                        if (!methodDefinition.IsGeneric())
                        {
                            if (debug)
                            {
                                Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                    + "`, methodDefinition.IsGeneric=" + methodDefinition.IsGeneric()
                                    + ", externMethodgenericArgsCount=" + externMethodgenericArgsCount);
                            }
                            return false;
                        }
                        if (externMethodgenericArgsCount != methodDefinition.GenericParameters.Count)
                        {
                            if (debug)
                            {
                                Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                    + "`,methodDefinition.GenericParameters.Count=" + methodDefinition.GenericParameters.Count
                                    + ", externMethodgenericArgsCount=" + externMethodgenericArgsCount);
                            }
                            return false;
                        }
                    }

                    // compare Parameters
                    if (externMethod.parameters != null)
                    {
                        for (int i = 0; i < externMethodParametersCount; i++)
                        {
                            IFIxParameter externMethodParam = externMethod.parameters[i];
                            ParameterDefinition paramDefinition = methodDefinition.Parameters[i];

                            if (externMethodParam.isGeneric) // param is generic, generic from method
                            {
                                if (!methodDefinition.IsGeneric())
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                            + "`,methodDefinition.IsGeneric=" + methodDefinition.IsGeneric()
                                            + ", externMethodParam=" + externMethodParam);
                                    }
                                    return false;
                                }
                                string paramTypeName = System.Text.RegularExpressions.Regex
                                                            .Replace(externMethodParam.declaringType, @"!!\d+", m =>
                                                                 methodDefinition.GenericParameters[int.Parse(m.Value.Substring(2))].Name);
                                if (paramTypeName != paramDefinition.ParameterType.Name)
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                            + "`,paramDefinition.Name=" + paramDefinition.ParameterType.Name
                                            + ", externMethod.paramTypeName=" + paramTypeName);
                                    }
                                    return false;
                                }
                            }
                            else // param is not generic
                            {
                                if (!IsTypeEquals(paramDefinition.ParameterType, externMethodParam.declaringType, ignoreAssemblyName))
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                            + "`,paramDefinition.ParameterType.FullName=" + paramDefinition.ParameterType.FullName
                                            + ", externMethodParam.declaringType=" + externMethodParam.declaringType);
                                    }
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                }
                else // !isGenericInstance
                {
                    bool matched = methodDefinition.Name == externMethod.methodName
                      && methodDefinition.Parameters.Count == externMethodParametersCount;
                    if (!matched)
                    {
                        return false;
                    }

                    if (methodDefinition.IsGenericInstance)
                    {
                        if (debug)
                        {
                            Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                + "`,methodDefinition.IsGenericInstance=" + methodDefinition.IsGenericInstance
                                + ", externMethodParam.isGenericInstance=" + externMethod.isGenericInstance);
                        }
                        return false;
                    }

                    // compare Parameters
                    if (externMethod.parameters != null)
                    {
                        for (int i = 0; i < externMethodParametersCount; i++)
                        {
                            IFIxParameter externMethodParam = externMethod.parameters[i];
                            ParameterDefinition paramDefinition = methodDefinition.Parameters[i];
                            if (paramDefinition.ParameterType.IsGeneric()) // param is generic, generic from type
                            {
                                // map: GenericParameter(T) -> GenericArgument(Object)
                                Dictionary<string, string> gaMap = new Dictionary<string, string>();
                                TypeDefinition typeDefinition = methodDefinition.DeclaringType;
                                GenericInstanceType genericInstanceType = typeReference as GenericInstanceType;
                                if (genericInstanceType != null && genericInstanceType.HasGenericArguments && typeDefinition.HasGenericParameters 
                                    && genericInstanceType.GenericArguments.Count == typeDefinition.GenericParameters.Count)
                                {
                                    for (int l = 0; l < typeDefinition.GenericParameters.Count; l++)
                                    {
                                        GenericParameter gp = typeDefinition.GenericParameters[l];
                                        TypeReference ga = genericInstanceType.GenericArguments[l];
                                        gaMap.Add(gp.Name, ga.FullName);
                                    }
                                }

                                string genericParemeter = paramDefinition.ParameterType.Name.Replace("&", ""); // TODO: ref? array?
                                string genericArgument = gaMap.ContainsKey(genericParemeter) ? gaMap[genericParemeter] : null;
                                TypeReference typeRef = assembly.MainModule.GetType(externMethodParam.declaringType, true);
                                if (genericArgument != typeRef.FullName.Replace("&", ""))
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                            + "`,paramDefinition.ParameterType=" + paramDefinition.ParameterType.FullName
                                            + ", externMethodParam.declaringType=" + externMethodParam.declaringType);
                                    }
                                    return false;
                                }
                            }
                            else // param is not generci
                            {
                                if (!IsTypeEquals(paramDefinition.ParameterType, externMethodParam.declaringType, ignoreAssemblyName))
                                {
                                    if (debug)
                                    {
                                        Console.WriteLine("warning: not match `" + externMethod.declaringType + "` `" + externMethod.methodName
                                            + "`,paramDefinition.ParameterType.FullName=" + paramDefinition.ParameterType.FullName
                                            + ", externMethodParam.declaringType=" + externMethodParam.declaringType);
                                    }
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                } // endif(isGenericInstance)
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
        private bool debug = false;
    }
}
