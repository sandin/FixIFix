﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace FixIFix
{
    public class IFixPatch
    {
        public ulong instructionMagic;
        public string interfaceBridgeTypeName;
        public string[] externTypes;
        public IFixMethod[] methods;
        public IFixExternMethod[] externMethods;
        public string[] internStrings;
        public IFixFieldInfo[] fieldInfos;
        public string[] staticFieldTypes;
        public int[] cctors;
        public IFixAnonymousStoreyInfo[] anonymousStoreyInfos;
        public string wrappersManagerImplName;
        public string assemblyStr;
        public IFixFixMethod fixMethod;
        public IFixNewClass newClass;

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
                    + ", assemblyStr: " + assemblyStr
                    + ", assemblyStr: " + fixMethod
                    + ", assemblyStr: " + newClass
                    + "]";
        }
    }

    public class IFixMethod
    {
        public IFixInstruction[] instructions;
        public IFixExceptionHandler[] exceptionHandlers;

        public override string ToString()
        {
            return "[Method "
                    + "instructions: " + instructions
                    + ", exceptionHandlers: " + exceptionHandlers
                    + "]";
        }
    }

    public class IFixInstruction
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

    public class IFixExceptionHandler
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

    public class IFixExternMethod
    {
        public bool isGenericInstance;
        public string declaringType;
        public string methodName;
        public string[] genericArgs; // if isGenericInstance = true
        public IFIxParameter[] parameters;

        public override string ToString()
        {
            return "[ExternMethod "
                    + "isGenericInstance: " + isGenericInstance
                    + ", declaringType: " + declaringType
                    + ", methodName: " + methodName
                    + ", genericArgs: " + genericArgs
                    + ", parameters: " + parameters
                    + "]";
        }
    }

    public class IFIxParameter
    {
        public bool isGeneric;
        public string declaringType;

        public override string ToString()
        {
            return "[IFIxParameter "
                    + "isGeneric: " + isGeneric
                    + ", declaringType: " + declaringType
                    + "]";
        }
    }

    public class IFixFieldInfo
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

    public class IFixAnonymousStoreyInfo
    {
        public int fieldNum;
        public int[] fieldTypes;
        public int ctorId;
        public int ctorParamNum;
        public int[] slots;
        public int[] vTable;

        public override string ToString()
        {
            return "[AnonymousStoreyInfo "
                    + "fieldNum: " + fieldNum
                    + ", fieldTypes: " + fieldTypes
                    + ", ctorId: " + ctorId
                    + ", ctorParamNum: " + ctorParamNum
                    + ", slots: " + slots
                    + ", vTable: " + vTable
                    + "]";
        }
    }

    public class IFixFixMethod
    {
        public IFixExternMethod[] fixMethods;
        public int[] fixMethodIds;
        public int fixCount;

        public override string ToString()
        {
            return "[fixMethods "
                    + "fixCount: " + fixCount
                    + ", fixMethodIds: " + fixMethodIds
                    + "]";
        }
    }

    public class IFixNewClass
    {
        public int newClassCount;
        public string[] newClassFullName;

        public override string ToString()
        {
            return "[NewClass "
                    + "newClassCount: " + newClassCount
                    + ", newClassFullName: " + newClassFullName
                    + "]";
        }
    }

    public class IFixIDTagAttribute : Attribute
    {
        public int ID;

        public IFixIDTagAttribute(int id)
        {
            ID = id;
        }
    }
}
