# FixIFix

The missing tool for [InjectFix](https://github.com/Tencent/InjectFix) patch file.

​          

## Build

* VS 2019

​    

## Install

Download the latest version of zip file from [Releases](../../releases), unzip it.

​          

## Usage

### Check Patch File

```bash
FixIFix.exe checkpatch <patch_file> <assemblies_path>  
```

* `patch_file`: e.g.: `Assembly-CSharp.patch.bytes`
* `assemblies_path`: e.g.: `./Temp/StagingArea/Il2Cpp/Managed/`

Output:

```bash
Error: can not found type: `UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null`.
Error: can not found type: `UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null`.
Error: can not found method: `Get` in type: UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
Error: can not found method: `FromJson` in type: UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
Error: can not found field: `var2` in type: NewBehaviourScript, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
Found 5 errors in patch file `..\..\..\Test\data\Assembly-CSharp.patch.bytes`
```

​        

### Merge Patch files  

TODO

​    

### Dump Patch File

```bash
FixIFix.exe dumppatch <patch_file>
```

* `patch_file`: e.g.: `Assembly-CSharp.patch.bytes`

Output:

```bash
instructionMagic: 3910370343942684686
interfaceBridgeTypeName: IFix.ILFixInterfaceBridge, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externTypes(13):
        IFix.ILFixInterfaceBridge, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
        System.Void, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        UnityEngine.Debug, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
        UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
        System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        NewBehaviourScript, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
        System.Single, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
        UnusedClassA, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
        UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null

methods(2):
        Method #0:
                instructions(19):
                        [Instruction code: 19, operand: 196611]
                        [Instruction code: 160, operand: 0]
                        [Instruction code: 76, operand: 0]
                        [Instruction code: 8, operand: 0]
                        [Instruction code: 88, operand: 0]
                        [Instruction code: 88, operand: 100]
                        [Instruction code: 57, operand: 196609]
                        [Instruction code: 76, operand: 1]
                        [Instruction code: 80, operand: 0]
                        [Instruction code: 80, operand: 1]
                        [Instruction code: 38, operand: 3]
                        [Instruction code: 57, operand: 131074]
                        [Instruction code: 57, operand: 65539]
                        [Instruction code: 160, operand: 1]
                        [Instruction code: 57, operand: 65540]
                        [Instruction code: 76, operand: 2]
                        [Instruction code: 17, operand: 0]
                        [Instruction code: 34, operand: 65537]
                        [Instruction code: 96, operand: 0]
                exceptionHandlers(0):

        Method #1:
                instructions(37):
                        [Instruction code: 19, operand: 327683]
                        [Instruction code: 160, operand: 2]
                        [Instruction code: 76, operand: 0]
                        [Instruction code: 8, operand: 0]
                        [Instruction code: 88, operand: 0]
                        [Instruction code: 88, operand: 100]
                        [Instruction code: 57, operand: 196609]
                        [Instruction code: 76, operand: 1]
                        [Instruction code: 88, operand: 0]
                        [Instruction code: 76, operand: 2]
                        [Instruction code: 80, operand: 0]
                        [Instruction code: 160, operand: 3]
                        [Instruction code: 57, operand: 131077]
                        [Instruction code: 76, operand: 4]
                        [Instruction code: 80, operand: 4]
                        [Instruction code: 81, operand: 3]
                        [Instruction code: 88, operand: 1]
                        [Instruction code: 76, operand: 2]
                        [Instruction code: 80, operand: 0]
                        [Instruction code: 80, operand: 1]
                        [Instruction code: 38, operand: 3]
                        [Instruction code: 17, operand: 0]
                        [Instruction code: 85, operand: 0]
                        [Instruction code: 38, operand: 10]
                        [Instruction code: 57, operand: 196614]
                        [Instruction code: 57, operand: 65539]
                        [Instruction code: 160, operand: 4]
                        [Instruction code: 57, operand: 65543]
                        [Instruction code: 76, operand: 3]
                        [Instruction code: 160, operand: 5]
                        [Instruction code: 80, operand: 3]
                        [Instruction code: 85, operand: 1]
                        [Instruction code: 57, operand: 131080]
                        [Instruction code: 57, operand: 65539]
                        [Instruction code: 160, operand: 6]
                        [Instruction code: 57, operand: 65539]
                        [Instruction code: 96, operand: 0]
                exceptionHandlers(0):


externMethods(9):
        System.Random..ctor:
                declaringType: System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(0):

        System.Random.Next:
                declaringType: System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(2): System.Int32 System.Int32

        System.String.Concat:
                declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(2): System.Object System.Object

        UnityEngine.Debug.Log:
                declaringType: UnityEngine.Debug, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
                parameters(1): System.Object

        UnityEngine.Networking.UnityWebRequest.Get:
                declaringType: UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
                parameters(1): System.String

        System.String.Contains:
                declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(1): System.String

        System.String.Concat:
                declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(3): System.Object System.Object System.Object

        UnityEngine.JsonUtility.FromJson:
                declaringType: UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
                parameters(1): System.String
                isGenericInstance: True
                genericArgs: System.String[]

        System.String.Concat:
                declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
                parameters(2): System.String System.String


internStrings(7):
        FuncA invoked
        http://www.baidu.com
        FuncB invoked
        FuncB
        {"name": "lds"}
        objA, name=
        UNITY_EDITOR only

fieldInfos(2):
        [FieldInfo isNewField: False, declaringType: NewBehaviourScript, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, fieldName: var2, fieldType: , methodId: 0]
        [FieldInfo isNewField: False, declaringType: UnusedClassA, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, fieldName: name, fieldType: , methodId: 0]

staticFieldTypes(0):

cctors(0):
```

​        

### Dump Assembly File

```bash
FixIFix.exe dumpdll <assembly_file>
```

* `assembly_file`: e.g.: `Assembly-CSharp.dll`

Output:

```
Type: <Module>
Type: NewBehaviourScript
        Method: System.Void NewBehaviourScript::Start()
        Method: System.Void NewBehaviourScript::Update()
        Method: System.Void NewBehaviourScript::FuncA()
        Method: System.Void NewBehaviourScript::FuncB()
        Method: System.Void NewBehaviourScript::UnusedMethod1()
        Method: System.Void NewBehaviourScript::.ctor()
        Field: System.Double unusedField1

Type: UnusedBehaviourScript
        Method: System.Void UnusedBehaviourScript::Start()
        Method: System.Void UnusedBehaviourScript::Update()
        Method: System.Void UnusedBehaviourScript::.ctor()

Type: UnusedClassA
        Method: System.Void UnusedClassA::.ctor()
        Field: System.String name

Type: IFix.ILFixDynamicMethodWrapper
        Method: System.Void IFix.ILFixDynamicMethodWrapper::.ctor(IFix.Core.VirtualMachine,System.Int32,System.Object)
        Method: System.Void IFix.ILFixDynamicMethodWrapper::__Gen_Wrap_0(System.Object)
        Method: System.Void IFix.ILFixDynamicMethodWrapper::.cctor()
        Field: IFix.Core.VirtualMachine virtualMachine
        Field: System.Int32 methodId
        Field: System.Object anonObj
        Field: IFix.ILFixDynamicMethodWrapper[] wrapperArray

Type: IFix.ILFixInterfaceBridge
        Method: System.Void IFix.ILFixInterfaceBridge::.ctor(System.Int32,System.Int32[],System.Int32,System.Int32[],System.Int32[],IFix.Core.VirtualMachine)
        Method: System.Void IFix.ILFixInterfaceBridge::RefAsyncBuilderStartMethod()

Type: IFix.WrappersManagerImpl
        Method: System.Void IFix.WrappersManagerImpl::.ctor(IFix.Core.VirtualMachine)
        Method: IFix.ILFixDynamicMethodWrapper IFix.WrappersManagerImpl::GetPatch(System.Int32)
        Method: System.Boolean IFix.WrappersManagerImpl::IsPatched(System.Int32)
        Method: System.Delegate IFix.WrappersManagerImpl::CreateDelegate(System.Type,System.Int32,System.Object)
        Method: System.Object IFix.WrappersManagerImpl::CreateWrapper(System.Int32)
        Method: System.Object IFix.WrappersManagerImpl::InitWrapperArray(System.Int32)
        Method: IFix.Core.AnonymousStorey IFix.WrappersManagerImpl::CreateBridge(System.Int32,System.Int32[],System.Int32,System.Int32[],System.Int32[],IFix.Core.VirtualMachine)
        Field: IFix.Core.VirtualMachine virtualMachine

Type: IFix.IDMAP0
        Field: System.Int32 value__
        Field: IFix.IDMAP0 NewBehaviourScript-Start0
        Field: IFix.IDMAP0 NewBehaviourScript-FuncB0
        Field: IFix.IDMAP0 NewBehaviourScript-FuncA0
        Field: IFix.IDMAP0 NewBehaviourScript-Update0
        Field: IFix.IDMAP0 NewBehaviourScript-UnusedMethod10
```

​    