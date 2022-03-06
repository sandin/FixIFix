# FixIFix

The missing tool for [InjectFix]([https://github.com/Tencent/InjectFix]) patch file.

​          

## Build

* VS 2019

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
externTypeCount: 13
externType: IFix.ILFixInterfaceBridge, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externType: System.Void, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: UnityEngine.Debug, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externType: UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externType: System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: NewBehaviourScript, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externType: System.Single, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
externType: UnusedClassA, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
externType: UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
methodCount: 2
Method:
instructionCount: 19
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
ehsOfMethodCount: 0
Method:
instructionCount: 37
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
ehsOfMethodCount: 0
externMethodCount: 9
[ExternMethod isGenericInstance: False, declaringType: System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: .ctor, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: System.Random, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: Next, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: Concat, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: UnityEngine.Debug, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, methodName: Log, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: UnityEngine.Networking.UnityWebRequest, UnityEngine.UnityWebRequestModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, methodName: Get, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: Contains, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: Concat, genericArgs: , parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: True, declaringType: UnityEngine.JsonUtility, UnityEngine.JSONSerializeModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, methodName: FromJson, genericArgs: System.String[], parameters: FixIFix.IFIxParameter[]]
[ExternMethod isGenericInstance: False, declaringType: System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089, methodName: Concat, genericArgs: , parameters: FixIFix.IFIxParameter[]]
internString: FuncA invoked
internString: http://www.baidu.com
internString: FuncB invoked
internString: FuncB
internString: {"name": "lds"}
internString: objA, name=
internString: UNITY_EDITOR only
fieldInfoCount: 2
[FieldInfo isNewField: False, declaringType: NewBehaviourScript, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, fieldName: var2, fieldType: , methodId: 0]
[FieldInfo isNewField: False, declaringType: UnusedClassA, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null, fieldName: name, fieldType: , methodId: 0]
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
System.Void NewBehaviourScript::Start()
System.Void NewBehaviourScript::Update()
System.Void NewBehaviourScript::FuncA()
System.Void NewBehaviourScript::FuncB()
System.Void NewBehaviourScript::UnusedMethod1()
System.Void NewBehaviourScript::.ctor()
System.Double unusedField1
Type: UnusedBehaviourScript
System.Void UnusedBehaviourScript::Start()
System.Void UnusedBehaviourScript::Update()
System.Void UnusedBehaviourScript::.ctor()
Type: UnusedClassA
System.Void UnusedClassA::.ctor()
System.String name
Type: IFix.ILFixDynamicMethodWrapper
System.Void IFix.ILFixDynamicMethodWrapper::.ctor(IFix.Core.VirtualMachine,System.Int32,System.Object)
System.Void IFix.ILFixDynamicMethodWrapper::__Gen_Wrap_0(System.Object)
System.Void IFix.ILFixDynamicMethodWrapper::.cctor()
IFix.Core.VirtualMachine virtualMachine
System.Int32 methodId
System.Object anonObj
IFix.ILFixDynamicMethodWrapper[] wrapperArray
Type: IFix.ILFixInterfaceBridge
System.Void IFix.ILFixInterfaceBridge::.ctor(System.Int32,System.Int32[],System.Int32,System.Int32[],System.Int32[],IFix.Core.VirtualMachine)
System.Void IFix.ILFixInterfaceBridge::RefAsyncBuilderStartMethod()
Type: IFix.WrappersManagerImpl
System.Void IFix.WrappersManagerImpl::.ctor(IFix.Core.VirtualMachine)
IFix.ILFixDynamicMethodWrapper IFix.WrappersManagerImpl::GetPatch(System.Int32)
System.Boolean IFix.WrappersManagerImpl::IsPatched(System.Int32)
System.Delegate IFix.WrappersManagerImpl::CreateDelegate(System.Type,System.Int32,System.Object)
System.Object IFix.WrappersManagerImpl::CreateWrapper(System.Int32)
System.Object IFix.WrappersManagerImpl::InitWrapperArray(System.Int32)
IFix.Core.AnonymousStorey IFix.WrappersManagerImpl::CreateBridge(System.Int32,System.Int32[],System.Int32,System.Int32[],System.Int32[],IFix.Core.VirtualMachine)
IFix.Core.VirtualMachine virtualMachine
Type: IFix.IDMAP0
System.Int32 value__
IFix.IDMAP0 NewBehaviourScript-Start0
IFix.IDMAP0 NewBehaviourScript-FuncB0
IFix.IDMAP0 NewBehaviourScript-FuncA0
IFix.IDMAP0 NewBehaviourScript-Update0
IFix.IDMAP0 NewBehaviourScript-UnusedMethod10
```

​    