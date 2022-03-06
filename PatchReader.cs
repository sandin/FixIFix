using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FixIFix
{
    namespace IFix
    {
        public class BinaryPatch
        {
            public ulong instructionMagic;
            public string interfaceBridgeTypeName;
            public string[] externTypes;
            public Method[] methods;
            public ExternMethod[] externMethods;
            public string[] internStrings;
            public FieldInfo[] fieldInfos;
            public string[] staticFieldTypes;
            public int[] cctors;
            public AnonymousStoreyInfo[] anonymousStoreyInfos;
            public string wrappersManagerImplName;

            public override string ToString()
            {
                return "[BinaryPatch " 
                        + "instructionMagic: " + instructionMagic
                        + ", interfaceBridgeTypeName: " + interfaceBridgeTypeName
                        + ", externTypes: " + externTypes
                        + ", methods: " + methods
                        + ", externMethods: " + externMethods
                        + ", internStrings: " + internStrings
                        + ", fieldInfos: " + fieldInfos
                        + ", staticFieldTypes: " + staticFieldTypes
                        + ", cctors: " + cctors
                        + ", anonymousStoreyInfos: " + anonymousStoreyInfos
                        + ", wrappersManagerImplName: " + wrappersManagerImplName
                        + "]";
            }
        }

        public class Method
        {
            public Instruction[] instructions;
            public ExceptionHandler[] exceptionHandlers;

            public override string ToString()
            {
                return "[Method "
                        + "instructions: " + instructions
                        + ", exceptionHandlers: " + exceptionHandlers
                        + "]";
            }
        }

        public class Instruction
        {
            public int code;
            public int operand;

            public override string ToString()
            {
                return "[Instruction "
                        + "code: " + code
                        + ", operand: " + operand
                        + "]";
            }
        }

        public class ExceptionHandler
        {
            public int handlerType;
            public int catchTypeId;
            public int tryStart;
            public int tryEnd;
            public int handlerStart;
            public int handlerEnd;

            public override string ToString()
            {
                return "[ExceptionHandler "
                        + "handlerType: " + handlerType
                        + ", catchTypeId: " + catchTypeId
                        + ", tryStart: " + tryStart
                        + ", tryEnd: " + tryEnd
                        + ", handlerStart: " + handlerStart
                        + ", handlerEnd: " + handlerEnd
                        + "]";
            }
        }

        public class ExternMethod
        {
            public bool isGenericInstance;
            public string declaringType;
            public string methodName;
            public string[] genericArgs; // if isGenericInstance = true
            public string[] paramTypes;

            public override string ToString()
            {
                return "[ExternMethod "
                        + "isGenericInstance: " + isGenericInstance
                        + ", declaringType: " + declaringType
                        + ", methodName: " + methodName
                        + ", genericArgs: " + genericArgs
                        + ", paramTypes: " + paramTypes
                        + "]";
            }
        }

        public class FieldInfo
        {
            public bool isNewField;
            public string declaringType;
            public string fieldName;
            public string fieldType;
            public int methodId;

            public override string ToString()
            {
                return "[FieldInfo "
                        + "isNewField: " + isNewField
                        + ", declaringType: " + declaringType
                        + ", fieldName: " + fieldName
                        + ", fieldType: " + fieldType
                        + ", methodId: " + methodId
                        + "]";
            }
        }

        public class AnonymousStoreyInfo
        {
            public int fieldNum;
            public int[] fieldTypes;
            public int ctorId;
            public int[] slots;
            public int[] vTable;

            public override string ToString()
            {
                return "[AnonymousStoreyInfo "
                        + "fieldNum: " + fieldNum
                        + ", fieldTypes: " + fieldTypes
                        + ", ctorId: " + ctorId
                        + ", slots: " + slots
                        + ", vTable: " + vTable
                        + "]";
            }
        }
    }

    class PatchReader
    {
        public PatchReader()
        {
        }


        public IFix.BinaryPatch Read(string patchfile)
        {
            Console.WriteLine("patch file: " + patchfile);
            IFix.BinaryPatch patch = new IFix.BinaryPatch();
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
                    patch.methods = new IFix.Method[methodCount];
                    for (int j = 0; j < methodCount; j++)
                    {
                        IFix.Method method = new IFix.Method();
                        int codeSize = reader.ReadInt32();
                        method.instructions = new IFix.Instruction[codeSize];
                        for (int i = 0; i < codeSize; i++)
                        {
                            IFix.Instruction inst = new IFix.Instruction();
                            inst.code = reader.ReadInt32();
                            inst.operand = reader.ReadInt32();
                            method.instructions[i] = inst;
                        }
                        int ehsOfMethodCount = reader.ReadInt32();
                        method.exceptionHandlers = new IFix.ExceptionHandler[ehsOfMethodCount];
                        for (int i = 0; i < ehsOfMethodCount; i++)
                        {
                            IFix.ExceptionHandler eh = new IFix.ExceptionHandler();
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
                    patch.externMethods = new IFix.ExternMethod[externMethodCount];
                    for (int i = 0; i < externMethodCount; i++)
                    {
                        IFix.ExternMethod externMethod = new IFix.ExternMethod();
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
                            externMethod.paramTypes = new string[paramCount];
                            for (int j = 0; j < paramCount; j++)
                            {
                                bool isGeneric = reader.ReadBoolean();
                                externMethod.paramTypes[j] = isGeneric ? reader.ReadString() : patch.externTypes[reader.ReadInt32()];
                            }
                        }
                        else
                        {
                            externMethod.declaringType = patch.externTypes[reader.ReadInt32()];
                            externMethod.methodName = reader.ReadString();
                            int paramCount = reader.ReadInt32();
                            externMethod.paramTypes = new string[paramCount];
                            for (int j = 0; j < paramCount; j++)
                            {
                                externMethod.paramTypes[j] = patch.externTypes[reader.ReadInt32()];
                            }
                        }
                        patch.externMethods[i] = externMethod;
                    }

                    int internStringsCount = reader.ReadInt32();
                    patch.internStrings = new string[internStringsCount];
                    for (int i = 0; i < internStringsCount; i++)
                    {
                        patch.internStrings[i] = reader.ReadString();
                    }

                    int fieldInfoCount = reader.ReadInt32();
                    patch.fieldInfos = new IFix.FieldInfo[fieldInfoCount];
                    for (int i = 0; i < fieldInfoCount; i++)
                    {
                        IFix.FieldInfo fieldInfo = new IFix.FieldInfo();
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

                    // TODO

                    /*
                    anonymousStoreyInfos = new AnonymousStoreyInfo[reader.ReadInt32()];
                    for (int i = 0; i < anonymousStoreyInfos.Length; i++)
                    {
                        int fieldNum = reader.ReadInt32();
                        int[] fieldTypes = new int[fieldNum];
                        for (int fieldIdx = 0; fieldIdx < fieldNum; ++fieldIdx)
                        {
                            fieldTypes[fieldIdx] = reader.ReadInt32();
                        }
                        int ctorId = reader.ReadInt32();
                        int ctorParamNum = reader.ReadInt32();
                        var slots = readSlotInfo(reader, itfMethodToId, externTypes, maxId);

                        int virtualMethodNum = reader.ReadInt32();
                        int[] vTable = new int[virtualMethodNum];
                        for (int vm = 0; vm < virtualMethodNum; vm++)
                        {
                            vTable[vm] = reader.ReadInt32();
                        }
                        anonymousStoreyInfos[i] = new AnonymousStoreyInfo()
                        {
                            CtorId = ctorId,
                            FieldNum = fieldNum,
                            FieldTypes = fieldTypes,
                            CtorParamNum = ctorParamNum,
                            Slots = slots,
                            VTable = vTable
                        };
                    }
                    */

                    // TODO
                }
            }

            return patch; 
        }

        public static void Dump(IFix.BinaryPatch patch)
        {
            Console.WriteLine("instructionMagic: " + patch.instructionMagic);
            Console.WriteLine("interfaceBridgeTypeName: " + patch.interfaceBridgeTypeName);
            Console.WriteLine("externTypeCount: " + patch.externTypes.Length);
            foreach (string externType in patch.externTypes)
            {
                Console.WriteLine("externType: " + externType);
            }
            Console.WriteLine("methodCount: " + patch.methods.Length);
            foreach (IFix.Method method in patch.methods)
            {
                Console.WriteLine("Method:");
                Console.WriteLine("instructionCount: " + method.instructions.Length);
                foreach (IFix.Instruction inst in method.instructions)
                {
                    Console.WriteLine(inst.ToString());
                }
                Console.WriteLine("ehsOfMethodCount: " + method.exceptionHandlers.Length);
                foreach (IFix.ExceptionHandler eh in method.exceptionHandlers)
                {
                    Console.WriteLine(eh.ToString());
                }
            }
            Console.WriteLine("externMethodCount: " + patch.externMethods.Length);
            foreach (IFix.ExternMethod externMethod in patch.externMethods)
            {
                Console.WriteLine(externMethod.ToString());
            }
            foreach (string internString in patch.internStrings)
            {
                Console.WriteLine("internString: " + internString);
            }
            Console.WriteLine("fieldInfoCount: " + patch.fieldInfos.Length);
            foreach (IFix.FieldInfo fieldInfo in patch.fieldInfos)
            {
                Console.WriteLine(fieldInfo.ToString());
            }
            foreach (string staticFieldType in patch.staticFieldTypes)
            {
                Console.WriteLine("staticFieldType: " + staticFieldType);
            }
            foreach (int cctor in patch.cctors)
            {
                Console.WriteLine("cctor: " + cctor);
            }
        }

    }
}
