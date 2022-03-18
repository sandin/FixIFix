using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Reflection;
using Mono.Cecil.Cil;

namespace FixIFix
{

    public class PatchReader
    {
        public PatchReader()
        {
        }

        public IFixPatch Read(string patchfile)
        {
            //Console.WriteLine("patch file: " + patchfile);
            patch = new IFixPatch();
            using (FileStream fs = File.Open(patchfile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    patch.instructionMagic = reader.ReadUInt64();
                    patch.interfaceBridgeTypeName = reader.ReadString();
                    int externTypeCount = reader.ReadInt32();
                    patch.externTypes = new string[externTypeCount];
                    for (int i = 0; i < externTypeCount; i++)
                    {
                        var assemblyQualifiedName = reader.ReadString();
                        patch.externTypes[i] = assemblyQualifiedName;
                    }

                    int methodCount = reader.ReadInt32();
                    patch.methods = new IFixMethod[methodCount];
                    for (int j = 0; j < methodCount; j++)
                    {
                        IFixMethod method = new IFixMethod();
                        int codeSize = reader.ReadInt32();
                        method.instructions = new IFixInstruction[codeSize];
                        for (int i = 0; i < codeSize; i++)
                        {
                            IFixInstruction inst = new IFixInstruction();
                            inst.code = reader.ReadInt32();
                            inst.operand = reader.ReadInt32();
                            method.instructions[i] = inst;
                        }
                        int ehsOfMethodCount = reader.ReadInt32();
                        method.exceptionHandlers = new IFixExceptionHandler[ehsOfMethodCount];
                        for (int i = 0; i < ehsOfMethodCount; i++)
                        {
                            IFixExceptionHandler eh = new IFixExceptionHandler();
                            eh.handlerType = reader.ReadInt32();
                            eh.catchTypeId = reader.ReadInt32();
                            eh.tryStart = reader.ReadInt32();
                            eh.tryEnd = reader.ReadInt32();
                            eh.handlerStart = reader.ReadInt32();
                            eh.handlerEnd = reader.ReadInt32();
                            method.exceptionHandlers[i] = eh;
                        }
                        patch.methods[j] = method;
                    }

                    int externMethodCount = reader.ReadInt32();
                    patch.externMethods = new IFixExternMethod[externMethodCount];
                    for (int i = 0; i < externMethodCount; i++)
                    {
                        patch.externMethods[i] = readMethod(reader, patch);
                    }

                    int internStringsCount = reader.ReadInt32();
                    patch.internStrings = new string[internStringsCount];
                    for (int i = 0; i < internStringsCount; i++)
                    {
                        patch.internStrings[i] = reader.ReadString();
                    }

                    int fieldInfoCount = reader.ReadInt32();
                    patch.fieldInfos = new IFixFieldInfo[fieldInfoCount];
                    for (int i = 0; i < fieldInfoCount; i++)
                    {
                        IFixFieldInfo fieldInfo = new IFixFieldInfo();
                        fieldInfo.isNewField = reader.ReadBoolean();
                        fieldInfo.declaringType = patch.externTypes[reader.ReadInt32()];
                        fieldInfo.fieldName = reader.ReadString();

                        if (!fieldInfo.isNewField)
                        {
                            // oldField  pass
                        }
                        else
                        {
                            fieldInfo.fieldType = patch.externTypes[reader.ReadInt32()];
                            fieldInfo.methodId = reader.ReadInt32();
                        }
                        patch.fieldInfos[i] = fieldInfo;
                    }

                    patch.staticFieldTypes = new string[reader.ReadInt32()];
                    patch.cctors = new int[patch.staticFieldTypes.Length];
                    for (int i = 0; i < patch.staticFieldTypes.Length; i++)
                    {
                        patch.staticFieldTypes[i] = patch.externTypes[reader.ReadInt32()];
                        patch.cctors[i] = reader.ReadInt32();
                    }

                    /*patch.anonymousStoreyInfos = new IFixAnonymousStoreyInfo[reader.ReadInt32()];
                    //  c# 中定义的anonymous函数(lambada, deleaget等), 在实现过程中都会实现一个对象, 每一个对象都继承自 System.Object 对象, 
                    //  每一个对象会生成两个方法: invoke 和 ctor, 注意，因此每一个对象都继承自 System.Object 对象, 因此 System.Object 这个父类也自带一个 ctor
                    for (int i = 0; i < patch.anonymousStoreyInfos.Length; i++)
                    {
                        int fieldNum = reader.ReadInt32();
                        int[] fieldTypes = new int[fieldNum];
                        for (int fieldIdx = 0; fieldIdx < fieldNum; ++fieldIdx)
                        {
                            fieldTypes[fieldIdx] = reader.ReadInt32();
                        }
                        int ctorId = reader.ReadInt32();
                        int ctorParamNum = reader.ReadInt32();
                        int[] slots = readSlotInfo(reader, patch.externTypes);       //  FixMe

                        int virtualMethodNum = reader.ReadInt32();
                        int[] vTable = new int[virtualMethodNum];
                        for (int vm = 0; vm < virtualMethodNum; vm++)
                        {
                            vTable[vm] = reader.ReadInt32();
                        }
                        patch.anonymousStoreyInfos[i] = new IFixAnonymousStoreyInfo()
                        {
                            ctorId = ctorId,
                            fieldNum = fieldNum,        //  添加的匿名函数的个数
                            fieldTypes = fieldTypes,
                            ctorParamNum = ctorParamNum,
                            slots = slots,
                            vTable = vTable
                        };
                    }
                    
                    patch.wrappersManagerImplName = reader.ReadString();

                    patch.assemblyStr = reader.ReadString();

                    patch.fixMethod = new IFixFixMethod();
                    patch.fixMethod.fixCount = reader.ReadInt32();
                    patch.fixMethod.fixMethods = new IFixExternMethod[patch.fixMethod.fixCount];
                    patch.fixMethod.fixMethodIds = new int[patch.fixMethod.fixCount];

                    for (int i = 0; i < patch.fixMethod.fixCount; i++)
                    {
                        patch.fixMethod.fixMethods[i] = readMethod(reader, patch);
                        patch.fixMethod.fixMethodIds[i] = reader.ReadInt32();
                    }

                    patch.newClass = new IFixNewClass();
                    int newClassCount = reader.ReadInt32();
                    patch.newClass.newClassCount = newClassCount;
                    patch.newClass.newClassFullName = new string[newClassCount];
                    for (int i = 0; i < newClassCount; i++)
                    {
                        var newClassFullName = reader.ReadString();
                        patch.newClass.newClassFullName[i] = newClassFullName;
                        //var newClassName = Type.GetType(newClassFullName);
                    }*/
                }
            }

            return patch; 
        }

        public void Dump()
        {
            Console.WriteLine("instructionMagic: " + patch.instructionMagic);
            Console.WriteLine("interfaceBridgeTypeName: " + patch.interfaceBridgeTypeName);

            Console.WriteLine("externTypes(" + patch.externTypes.Length + "):");
            foreach (string externType in patch.externTypes)
            {
                Console.WriteLine("\t" + externType);
            }
            Console.WriteLine("");

            Console.WriteLine("methods(" + patch.methods.Length + "):");
            int i = 0;
            foreach (IFixMethod method in patch.methods)
            {
                Console.WriteLine("\tMethod #" + i + ":");
                Console.WriteLine("\t\tinstructions(" + method.instructions.Length + "):");
                foreach (IFixInstruction inst in method.instructions)
                {
                    Console.WriteLine("\t\t\t" + inst.ToString());
                    //Instruction instruction = Instruction.Create(inst.code, inst.operand);
                }
                Console.WriteLine("\t\texceptionHandlers(" + method.exceptionHandlers.Length + "):");
                foreach (IFixExceptionHandler eh in method.exceptionHandlers)
                {
                    Console.WriteLine("\t\t\t" + eh.ToString());
                }
                i++;
                Console.WriteLine("");
            }
            Console.WriteLine("");

            Console.WriteLine("externMethods(" + patch.externMethods.Length + "):");
            foreach (IFixExternMethod externMethod in patch.externMethods)
            {
                Console.WriteLine("\t" + StripAssemblyName(externMethod.declaringType) + "." + externMethod.methodName + ":");
                Console.WriteLine("\t\tdeclaringType: " + externMethod.declaringType);
                StringBuilder sb = new StringBuilder();
                foreach (IFIxParameter p in externMethod.parameters)
                {
                    sb.Append(StripAssemblyName(p.declaringType));
                    if (p.isGeneric)
                    {
                        sb.Append("(isGeneric=");
                        sb.Append(p.isGeneric);
                        sb.Append(")");
                    }
                    sb.Append(" ");
                }
                Console.WriteLine("\t\tparameters(" + externMethod.parameters.Length + "): " + sb.ToString());
                if (externMethod.isGenericInstance)
                {
                    Console.WriteLine("\t\tisGenericInstance: " + externMethod.isGenericInstance);
                    Console.WriteLine("\t\tgenericArgs: " + externMethod.genericArgs);
                }
                Console.WriteLine("");
            }
            Console.WriteLine("");

            Console.WriteLine("internStrings(" + patch.internStrings.Length + "):");
            foreach (string internString in patch.internStrings)
            {
                Console.WriteLine("\t" + internString);
            }
            Console.WriteLine("");

            Console.WriteLine("fieldInfos(" + patch.fieldInfos.Length + "):");
            foreach (IFixFieldInfo fieldInfo in patch.fieldInfos)
            {
                Console.WriteLine("\t" + fieldInfo.ToString());
            }
            Console.WriteLine("");

            Console.WriteLine("staticFieldTypes(" + patch.staticFieldTypes.Length + "):");
            foreach (string staticFieldType in patch.staticFieldTypes)
            {
                Console.WriteLine("\t" + staticFieldType);
            }
            Console.WriteLine("");

            Console.WriteLine("cctors(" + patch.cctors.Length + "):");
            foreach (int cctor in patch.cctors)
            {
                Console.WriteLine("\t" + cctor);
            }
            Console.WriteLine("");

/*            Console.WriteLine("anonymousStoreyInfos(" + patch.anonymousStoreyInfos.Length + "):");
            foreach (IFixAnonymousStoreyInfo anonymousStoreyInfo in patch.anonymousStoreyInfos)
            {
                Console.WriteLine("\t" + anonymousStoreyInfo.ToString());
            }
            Console.WriteLine("");

            Console.WriteLine("wrappersManagerImplName:");
            Console.WriteLine("\t" + patch.wrappersManagerImplName);

            Console.WriteLine("assemblyStr:");
            Console.WriteLine("\t" + patch.assemblyStr);

            Console.WriteLine("fixMethods(" + patch.fixMethod.fixCount + "):");
            i = 0;
            for (; i < patch.fixMethod.fixCount; ++i)
            {
                Console.WriteLine("\t" + patch.fixMethod.fixMethodIds[i].ToString());
                Console.WriteLine("\t" + patch.fixMethod.fixMethods[i].ToString());
            }
            Console.WriteLine("");

            Console.WriteLine("newClasses(" + patch.newClass.newClassCount + "):");
            foreach (var fullClassName in patch.newClass.newClassFullName)
            {
                Console.WriteLine("\t" + fullClassName);
            }
            Console.WriteLine("");*/
        }
        private static string StripAssemblyName(string qualifiedTypeName)
        {
            if (qualifiedTypeName != null)
            {
                int index = qualifiedTypeName.IndexOf(",");
                if (index != -1)
                {
                    return qualifiedTypeName.Substring(0, index);
                }
            }
            return qualifiedTypeName;
        }

        private static IFixExternMethod readMethod(BinaryReader reader, IFixPatch patch)
        {
            IFixExternMethod externMethod = new IFixExternMethod();
            externMethod.isGenericInstance = reader.ReadBoolean();
            if (externMethod.isGenericInstance)
            {
                externMethod.declaringType = patch.externTypes[reader.ReadInt32()];
                externMethod.methodName = reader.ReadString();
                int genericArgCount = reader.ReadInt32();
                externMethod.genericArgs = new string[genericArgCount];
                for (int j = 0; j < genericArgCount; j++)
                {
                    externMethod.genericArgs[j] = patch.externTypes[reader.ReadInt32()];
                }
                int paramCount = reader.ReadInt32();
                externMethod.parameters = new IFIxParameter[paramCount];
                for (int j = 0; j < paramCount; j++)
                {
                    IFIxParameter param = new IFIxParameter();
                    param.isGeneric = reader.ReadBoolean();
                    param.declaringType = param.isGeneric ? reader.ReadString() : patch.externTypes[reader.ReadInt32()];
                    externMethod.parameters[j] = param;
                }
            }
            else
            {
                externMethod.declaringType = patch.externTypes[reader.ReadInt32()];
                externMethod.methodName = reader.ReadString();
                int paramCount = reader.ReadInt32();
                externMethod.parameters = new IFIxParameter[paramCount];

                for (int j = 0; j < paramCount; j++)
                {
                    IFIxParameter param = new IFIxParameter();
                    param.isGeneric = false;
                    param.declaringType = patch.externTypes[reader.ReadInt32()];
                    externMethod.parameters[j] = param;
                }
            }

            return externMethod;
        }

        private int[] readSlotInfo(BinaryReader reader, string[] externTypes)
        {
            int interfaceCount = reader.ReadInt32();

            if (interfaceCount == 0) return null;

            int[] slots = new int[interfaceCount + 1];
            for (int j = 0; j < slots.Length; j++)
            {
                slots[j] = -1;
            }

            //  BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance
            UInt32[] bitArr = { 2, 16, 4 };

            //VirtualMachine._Info(string.Format("-------{0}----------", interfaceCount));
            for (int i = 0; i < interfaceCount; i++)
            {
                var itfId = reader.ReadInt32();
                var itf = patch.externTypes[itfId];
                //VirtualMachine._Info(itf.ToString());
                foreach (var method in bitArr)
                {
                    int methodId = reader.ReadInt32();
                    // slots[method] = methodId;
                    //VirtualMachine._Info(string.Format("<<< {0} [{1}]", method, methodId));
                }
            }
            return slots;
        }

        private IFixPatch patch;
    }
}
