using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using FixIFix;

namespace FixIFix.Test
{
    [TestFixture]
     public class PatchReaderTest
    {
        [SetUp]
        public static void SetUp()
        {
        }

        [TearDown]
        public static void TearDown()
        {
        }

        [Test]
        public void SimpleTest()
        {
            Console.WriteLine("SimpleTest");
            Assert.True(true);
        }


        [Test]
        public void TypeNameTest()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("c:\\Users\\liudingsan\\Downloads\\Il2CppDump\\DummyDll\\Assembly-CSharp.dll");
            Assert.True(reader.HasType("AutoResolution+LastType, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null", false));
            Assert.True(reader.HasType("FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null", false));
            Assert.True(reader.HasType("System.Collections.Generic.List`1[[FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false));
            Assert.True(reader.HasType("FurRenderer", true));
            Assert.True(reader.HasType("System.Collections.Generic.List`1[[FurRenderer", true));
        }

        [Test]
        public void GetMethodTest1()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("c:\\Users\\liudingsan\\Downloads\\Il2CppDump\\DummyDll\\Assembly-CSharp.dll");

            //Assert.True(reader.HasType("System.Collections.Generic.List`1[[FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", true));

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Collections.Generic.List`1[[FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = ".ctor";
            Assert.True(reader.HasMethod(method));
        }

        [Test]
        public void GetMethodTest2()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("c:\\Users\\liudingsan\\Downloads\\Il2CppDump\\DummyDll\\mscorlib.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Array, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = "Empty";
            Assert.True(reader.HasMethod(method));
        }


        [Test]
        public void ReaderTest()
        {
            PatchReader patchReader = new PatchReader();
            IFixPatch fixPatch = patchReader.Read("D:\\desktop\\fix_test\\android\\Assembly-CSharp.patch.bytes");
            //IFixPatch fixPatch = patchReader.Read("D:\\desktop\\fix_test\\windows\\Assembly-CSharp.patch.bytes");
            if (fixPatch == null)
            {
                Console.WriteLine("patchReader.Read failed.");
            }

            //  instructionMagic
            Assert.AreEqual(fixPatch.instructionMagic, (ulong)1987565531538694905);

            //  interfaceBridgeTypeName
            Assert.IsNotNull(fixPatch.interfaceBridgeTypeName);
            Assert.IsTrue(fixPatch.interfaceBridgeTypeName.Contains("IFix.ILFixInterfaceBridge, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"));

            //  externTypes
            Assert.IsTrue(fixPatch.interfaceBridgeTypeName.Length != 0);

            //  methods
            Assert.IsTrue(fixPatch.methods.Length != 0);

            //  externMethods
            Assert.IsNotNull(fixPatch.externMethods.Length);
            Assert.IsTrue(fixPatch.externMethods.Length != 0);

            //  internStrings
            Assert.IsNotNull(fixPatch.internStrings);
            Assert.IsTrue(fixPatch.internStrings[0].Contains("FuncA invoked"));

            //  fieldInfos
            Assert.IsNotNull(fixPatch.fieldInfos);
            Assert.IsTrue(fixPatch.fieldInfos.Length == 3);

            //  staticFieldTypes
            Assert.IsNotNull(fixPatch.staticFieldTypes);
            Assert.IsTrue(fixPatch.staticFieldTypes.Length == 4);

            //  cctors
            Assert.IsNotNull(fixPatch.cctors);
            Assert.IsTrue(fixPatch.cctors.Length == 4);

            //  anonymousStoreyInfos
            Assert.IsNotNull(fixPatch.anonymousStoreyInfos);

            //  wrappersManagerImplName
            Assert.IsNotNull(fixPatch.wrappersManagerImplName);

            //  assemblyStr
            Assert.IsNotNull(fixPatch.assemblyStr);

            //  fixMethods
            Assert.IsNotNull(fixPatch.fixMethod);
            Assert.AreEqual(fixPatch.fixMethod.fixCount, 3);

            //  newClasses
            Assert.IsNotNull(fixPatch.newClass);
            Assert.AreEqual(fixPatch.newClass.newClassCount, 2);
            Assert.IsTrue(fixPatch.newClass.newClassFullName[0].Contains("NewClass"));
        }

    }
}
