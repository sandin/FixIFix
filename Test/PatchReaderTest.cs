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
