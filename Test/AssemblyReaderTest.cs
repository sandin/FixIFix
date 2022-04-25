using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using System.Reflection;
using FixIFix;
using Mono.Cecil;

namespace FixIFix.Test
{

    [TestFixture]
    public class AssemblyReaderTest
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
        public void TypeNameTest()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\Assembly-CSharp.dll");

            Assert.False(reader.HasType("Not.Exists.Class", true));
            Assert.True(reader.HasType("FurRenderer", true));
            Assert.True(reader.HasType("AutoResolution+LastType", true));
            Assert.True(reader.HasType("AutoResolution+LastType, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null", false));
            Assert.True(reader.HasType("FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null", false));
            Assert.True(reader.HasType("System.Collections.Generic.List`1[[FurRenderer, Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", false));
        }

        [Test]
        public void GetTypeNameTest()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\Assembly-CSharp.dll");
            TypeReference type;

            type = reader.GetType("System.Object", false);
            Assert.NotNull(type);
            Assert.AreEqual("System.Object", type.FullName);

/*            type = reader.GetType("System.Int32[,]", false);
            Assert.NotNull(type);
            Assert.AreEqual("System.Int32[,]", type.FullName);*/


/*            type = reader.GetType("System.Object[]", false);
            Assert.NotNull(type);
            Assert.AreEqual("System.Object[]", type.FullName);*/
        }

        [Test]
        public void GetMethodTest1()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\Assembly-CSharp.dll");

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
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\mscorlib.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Array, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = "Empty";
            method.isGenericInstance = true;
            method.genericArgs = new string[] { "System.String[]" };
            Assert.True(reader.HasMethod(method));
        }

        [Test]
        public void GetMethodTest3()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\Assembly-CSharp.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "UnityEngine.AndroidJavaObject, UnityEngine.AndroidJNIModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            method.methodName = "Call";
            method.parameters = new IFIxParameter[2];
            method.parameters[0] = new IFIxParameter();
            method.parameters[0].isGeneric = false;
            method.parameters[0].declaringType = "System.String";
            method.parameters[1] = new IFIxParameter();
            method.parameters[1].isGeneric = false;
            method.parameters[1].declaringType = "System.Object[]";
            Assert.True(reader.HasMethod(method, true));
        }

        [Test]
        public void GetMethodTest4()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\mscorlib.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Threading.Interlocked, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = "CompareExchange";
            method.isGenericInstance = true;
            method.genericArgs = new string[] { "System.String[]" };
            method.parameters = new IFIxParameter[3];
            method.parameters[0] = new IFIxParameter();
            method.parameters[0].isGeneric = true;
            method.parameters[0].declaringType = "!!0&";
            method.parameters[1] = new IFIxParameter();
            method.parameters[1].isGeneric = true;
            method.parameters[1].declaringType = "!!0";
            method.parameters[2] = new IFIxParameter();
            method.parameters[2].isGeneric = true;
            method.parameters[2].declaringType = "!!0";
            Assert.True(reader.HasMethod(method, true));
        }

        [Test]
        public void GetMethodTest5()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\Assembly-CSharp.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "SimpleDictionaryInt16`2[[System.UInt32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[UnityEngine.Texture2D, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null]], Assembly-CSharp, Version=2.0.0.668, Culture=neutral, PublicKeyToken=null";
            method.methodName = "TryGetValue";
            method.parameters = new IFIxParameter[2];
            method.parameters[0] = new IFIxParameter();
            method.parameters[0].isGeneric = false;
            method.parameters[0].declaringType = "System.UInt32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.parameters[1] = new IFIxParameter();
            method.parameters[1].isGeneric = false;
            method.parameters[1].declaringType = "UnityEngine.Texture2D&, UnityEngine.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            Assert.True(reader.HasMethod(method, true));
        }

        [Test]
        public void GetMethodTest6()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\mscorlib.dll");

            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Collections.Generic.List`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = ".ctor";
            method.parameters = new IFIxParameter[1];
            method.parameters[0] = new IFIxParameter();
            method.parameters[0].isGeneric = false;
            method.parameters[0].declaringType = "System.Collections.Generic.IEnumerable`1[[System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assert.True(reader.HasMethod(method, true));
        }

        [Test]
        public void GetMethodTest7()
        {
            AssemblyReader reader = new AssemblyReader();
            reader.Read("D:\\desktop\\fix_test\\android\\DummyDll\\mscorlib.dll");
            IFixExternMethod method = new IFixExternMethod();
            method.declaringType = "System.Int32[,], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.methodName = ".Get";
            method.isGenericInstance = true;
            method.parameters = new IFIxParameter[2];
            method.parameters[0] = new IFIxParameter();
            method.parameters[0].isGeneric = false;
            method.parameters[0].declaringType = "System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            method.parameters[1] = new IFIxParameter();
            method.parameters[1].isGeneric = false;
            method.parameters[1].declaringType = "System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";
            Assert.True(reader.HasMethod(method, true));
        }

        [Test]
        public void ResolveTypeNameWithGaTest()
        {
            AssemblyReader reader = new AssemblyReader();
            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.String");
                Assert.AreEqual("System.String&", reader.ResolveTypeNameWithGa("T&", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.String");
                Assert.AreEqual("System.String[]", reader.ResolveTypeNameWithGa("T[]", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.String");
                Assert.AreEqual("System.String", reader.ResolveTypeNameWithGa("T", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T1", "System.String");
                Assert.AreEqual("System.String", reader.ResolveTypeNameWithGa("T1", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.String");
                Assert.AreEqual("System.Collections.Generic.List`1<System.String>",
                    reader.ResolveTypeNameWithGa("System.Collections.Generic.List`1<T>", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T1", "System.String");
                gaMap.Add("T2", "System.Int32");
                Assert.AreEqual("System.Collections.Generic.Dictionary`2<System.String, System.Int32>",
                    reader.ResolveTypeNameWithGa("System.Collections.Generic.Dictionary`2<T1, T2>", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T1", "System.String");
                gaMap.Add("T2", "System.Int32");
                Assert.AreEqual("System.Collections.Generic.Dictionary`2<System.String, System.Collections.Generic.List`1<System.Int32>>",
                    reader.ResolveTypeNameWithGa("System.Collections.Generic.Dictionary`2<T1, System.Collections.Generic.List`1<T2>>", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T1", "System.String");
                gaMap.Add("T2", "System.Int32");
                Assert.AreEqual("System.Collections.Generic.Dictionary`2<System.Collections.Generic.List`1<System.String>, System.Collections.Generic.List`1<System.Int32>>",
                    reader.ResolveTypeNameWithGa("System.Collections.Generic.Dictionary`2<System.Collections.Generic.List`1<T1>, System.Collections.Generic.List`1<T2>>", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.Int32");
                gaMap.Add("TString", "System.String");
                Assert.AreEqual("System.Int32[,],System.String[,]", reader.ResolveTypeNameWithGa("T[,],TString[,]", gaMap));
            }

            {
                var gaMap = new Dictionary<string, string>();
                gaMap.Add("T", "System.Int32");
                Assert.AreEqual("System.Int32[,,]", reader.ResolveTypeNameWithGa("T[,,]", gaMap));
            }
        }
    }
}
