using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FixIFix
{
    class PatchReader
    {

        public PatchReader()
        {

        }

        public bool Read(string patchfile)
        {
            using (FileStream fs = File.Open(patchfile, FileMode.Open))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    var instructionMagic = reader.ReadUInt64();
                    Console.WriteLine("instructionMagic: " + instructionMagic);
                    var interfaceBridgeTypeName = reader.ReadString();
                    Console.WriteLine("interfaceBridgeTypeName: " + interfaceBridgeTypeName);
                    int externTypeCount = reader.ReadInt32();
                    string[] externTypes = new string[externTypeCount];
                    Console.WriteLine("externTypeCount: " + externTypeCount);
                    for (int i = 0; i < externTypeCount; i++)
                    {
                        var assemblyQualifiedName = reader.ReadString();
                        externTypes[i] = assemblyQualifiedName;
                        Console.WriteLine("assemblyQualifiedName: " + assemblyQualifiedName);
                    }

                    int methodCount = reader.ReadInt32();
                    Console.WriteLine("methodCount: " + methodCount);
                    for (int j = 0; j < methodCount; j++)
                    {
                        Console.WriteLine("method: " + j);
                        int codeSize = reader.ReadInt32();
                        for (int i = 0; i < codeSize; i++)
                        {
                            int code = reader.ReadInt32();
                            int operand = reader.ReadInt32();
                            Console.WriteLine(i + " Code=" + code + " Operand=" + operand);
                        }
                        int ehsOfMethodCount = reader.ReadInt32();
                        Console.WriteLine("ehsOfMethodCount: " + ehsOfMethodCount);
                        for (int i = 0; i < ehsOfMethodCount; i++)
                        {
                            int handlerType = reader.ReadInt32();
                            int catchTypeId = reader.ReadInt32();
                            int tryStart = reader.ReadInt32();
                            int tryEnd = reader.ReadInt32();
                            int handlerStart = reader.ReadInt32();
                            int handlerEnd = reader.ReadInt32();
                        }
                    }

                    int externMethodCount = reader.ReadInt32();
                    Console.WriteLine("externMethodCount: " + externMethodCount);
                    for (int i = 0; i < externMethodCount; i++)
                    {
                        bool isGenericInstance = reader.ReadBoolean();
                        Console.WriteLine("isGenericInstance: " + isGenericInstance);
                        if (isGenericInstance)
                        {
                            string declaringType = externTypes[reader.ReadInt32()];
                            Console.WriteLine("declaringType: " + declaringType);
                            string methodName = reader.ReadString();
                            Console.WriteLine("methodName: " + methodName);
                            int genericArgCount = reader.ReadInt32();
                            string[] genericArgs = new string[genericArgCount];
                            for (int j = 0; j < genericArgCount; j++)
                            {
                                genericArgs[j] = externTypes[reader.ReadInt32()];
                                Console.WriteLine(j + " ga:" + genericArgs[j]);
                            }
                            int paramCount = reader.ReadInt32();
                            string[] paramMatchInfo = new string[paramCount];
                            for (int j = 0; j < paramCount; j++)
                            {
                                bool isGeneric = reader.ReadBoolean();
                                paramMatchInfo[j] = isGeneric ? reader.ReadString() : externTypes[reader.ReadInt32()];
                                Console.WriteLine("paramMatchInfo: " + j + " "+ paramMatchInfo[j]);
                            }
                        }
                        else
                        {
                            string declaringType = externTypes[reader.ReadInt32()];
                            string methodName = reader.ReadString();
                            int paramCount = reader.ReadInt32();
                            string[] paramTypes = new string[paramCount];
                            for (int j = 0; j < paramCount; j++)
                            {
                                paramTypes[j] = externTypes[reader.ReadInt32()];
                            }
                        }
                    }

                    int internStringsCount = reader.ReadInt32();
                    string[] internStrings = new string[internStringsCount];
                    for (int i = 0; i < internStringsCount; i++)
                    {
                        internStrings[i] = reader.ReadString();
                    }

                    int fieldInfoCount = reader.ReadInt32();
                    for (int i = 0; i < fieldInfoCount; i++)
                    {
                        var isNewField = reader.ReadBoolean();
                        var declaringType = externTypes[reader.ReadInt32()];
                        var fieldName = reader.ReadString();

                        if (!isNewField)
                        {
                            // oldField  pass
                        }
                        else
                        {
                            var fieldType = externTypes[reader.ReadInt32()];
                            Console.WriteLine("fieldType: " + fieldType);
                            var methodId = reader.ReadInt32();
                            Console.WriteLine("methodId: " + methodId);
                        }
                    }

                    string[] staticFieldTypes = new string[reader.ReadInt32()];
                    int[] cctors = new int[staticFieldTypes.Length];
                    for (int i = 0; i < staticFieldTypes.Length; i++)
                    {
                        staticFieldTypes[i] = externTypes[reader.ReadInt32()];
                        cctors[i] = reader.ReadInt32();
                    }

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








                }
            }

            return false; // TODO
        }


        private BinaryReader reader;

    }
}
