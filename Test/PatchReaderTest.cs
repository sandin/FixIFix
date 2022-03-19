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
            // IFixPatch fixPatch = patchReader.Read("D:\\desktop\\fix_test\\android\\Assembly-CSharp.patch.bytes");
            //IFixPatch fixPatch = patchReader.Read("D:\\desktop\\fix_test\\windows\\Assembly-CSharp.patch.bytes");
            IFixPatch fixPatch = patchReader.Read("..\\..\\data\\Assembly-CSharp.patch.bytes");
            if (fixPatch == null)
            {
                Console.WriteLine("patchReader.Read failed.");
            }

            //  instructionMagic
            Assert.AreEqual(fixPatch.instructionMagic, (ulong)3910370343942684686);

            //  interfaceBridgeTypeName
            Assert.IsNotNull(fixPatch.interfaceBridgeTypeName);
            Assert.IsTrue(fixPatch.interfaceBridgeTypeName.Contains("IFix.ILFixInterfaceBridge, Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"));

            //  externTypes
            Assert.IsTrue(fixPatch.externTypes.Length != 0);
            bool inner_class_flags = false;
            bool inner_enum_flags = false;
            bool template_flags = false;
            for (int i = 0; i < fixPatch.externTypes.Length; ++i)
            {
                if (fixPatch.externTypes[i].Contains("NewBehaviourScript.InnerClass"))
                {
                    inner_class_flags = true;
                }
                if (fixPatch.externTypes[i].Contains("NewBehaviourScript.InnerEnum"))
                {
                    inner_enum_flags = true;
                }

                //  FIXME
                if (fixPatch.externTypes[i].Contains("System.Collections.Generic.List`1[[NewBehaviourScript+InnerClass"))
                {
                    template_flags = true;
                }
            }

            Assert.IsTrue(inner_class_flags);
            Assert.IsTrue(template_flags);
            Assert.IsTrue(inner_enum_flags);

            //  methods
            Assert.IsTrue(fixPatch.methods.Length != 0);

            //  externMethods
            Assert.IsNotNull(fixPatch.externMethods.Length);
            Assert.IsTrue(fixPatch.externMethods.Length != 0);

            //  internStrings
            Assert.IsNotNull(fixPatch.internStrings);

            //  fieldInfos
            Assert.IsNotNull(fixPatch.fieldInfos);

            //  staticFieldTypes
            Assert.IsNotNull(fixPatch.staticFieldTypes);

            //  cctors
            Assert.IsNotNull(fixPatch.cctors);

/*            //  anonymousStoreyInfos
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
            Assert.IsTrue(fixPatch.newClass.newClassFullName[0].Contains("NewClass"));*/
        }

    }
}
