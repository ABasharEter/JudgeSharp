//╔═════════════════════════════════════════════════════════════════════════════════╗
//║                                                                                 ║
//║   ╔╗      ╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗╔╗╔╗╔╗     ╔╗      ╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗           ║
//║   ╚╝    ╔╗╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝╚╝╚╝╚╝╔╗   ╚╝╔╗    ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝           ║
//║   ╔╗  ╔╗╚╝     ╔╗           ╔╗      ╚╝   ╔╗╚╝    ╔╗   ╔╗           ╔╗           ║
//║   ╚╝╔╗╚╝       ╚╝╔╗╔╗╔╗╔╗   ╚╝╔╗╔╗╔╗     ╚╝  ╔╗  ╚╝   ╚╝╔╗╔╗╔╗╔╗   ╚╝           ║
//║   ╔╗╚╝╔╗       ╔╗╚╝╚╝╚╝╚╝   ╔╗╚╝╚╝╚╝╔╗   ╔╗  ╚╝  ╔╗   ╔╗╚╝╚╝╚╝╚╝   ╔╗           ║
//║   ╚╝  ╚╝╔╗     ╚╝           ╚╝      ╚╝   ╚╝    ╔╗╚╝   ╚╝           ╚╝           ║
//║   ╔╗    ╚╝╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗      ╔╗   ╔╗    ╚╝╔╗   ╔╗╔╗╔╗╔╗╔╗   ╔╗╔╗╔╗╔╗╔╗   ║
//║   ╚╝      ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝      ╚╝   ╚╝      ╚╝   ╚╝╚╝╚╝╚╝╚╝   ╚╝╚╝╚╝╚╝╚╝   ║
//║                                                                                 ║
//║   This file is a part of the project Judge Sharp done by Ahmad Bashar Eter.     ║
//║   This program is free software: you can redistribute it and/or modify          ║
//║   it under the terms of the GNU General Public License version 3.               ║
//║   This program is distributed in the hope that it will be useful, but WITHOUT   ║
//║   ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS ║
//║   FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details ║
//║   GNU General Public: http://www.gnu.org/licenses.                              ║
//║   For usage not under GPL please request my approval for commercial license.    ║
//║   Copyright(C) 2017 Ahmad Bashar Eter.                                          ║
//║   KernelGD@Hotmail.com                                                          ║
//║                                                                                 ║
//╚═════════════════════════════════════════════════════════════════════════════════╝

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JudgeSharp.Core;
using System.IO;

namespace JudgeSharpTest
{
    [TestClass]
    public class EqualityCompareProblemTest
    {

        [TestMethod]
        public void EqualityCompareProblemEncryptionDecryptionTest()
        {

            EqualityCompareProblem ep = new EqualityCompareProblem();

            string[] testInput = { "first input", "second input", "third input" };
            string[] testOutput = { "first output", "second output", "third output" };
            string[] wrongOutput = { "first\noutput", "second\noutput", "third\noutput" };
            string password1 = "password";
            string password2 = "p@$$w0rd";

            Assert.IsTrue(ep.Initialize(testOutput, testInput, password1));


            string[] o1 = ep.GetCorrectOutput(password1);
            string[] o2 = ep.GetCorrectOutput(password2);
            Assert.IsNotNull(o1);
            Assert.IsTrue(testOutput.Length == o1.Length);
            for (int i = 0; i < testOutput.Length; i++)
            {
                Assert.IsTrue(testOutput[i] == o1[i]);
            }
            Assert.IsNull(o2);

            string pass1 = ep.GetPassword(testOutput);
            string pass2 = ep.GetPassword(wrongOutput);

            Assert.AreEqual(pass1, password1);
            Assert.IsNull(pass2);
        }

        [TestMethod]
        public void EqualityCompareProblemLoadingSavingTest()
        {

            EqualityCompareProblem ep = new EqualityCompareProblem();

            string[] testInput = { "first input", "second input", "third input" };
            string[] testOutput = { "first output", "second output", "third output" };
            string password = "password";
            ep.Name = "Test Problem";
            var doc = DocumentHelper.LoadXpsData("Dummy Doucment.xps");
            ep.DocumentData = DocumentHelper.XpsDocumentToBytes(doc);
            Assert.IsTrue(ep.Initialize(testOutput, testInput, password));

            MemoryStream ms = new MemoryStream();
            Assert.IsTrue(ep.Save(ms));
            ms.Flush();
            ms.Close();
            ep = new EqualityCompareProblem();
            ms = new MemoryStream(ms.ToArray());
            Assert.IsTrue(ep.Load(ms));
            string[] o1 = ep.GetCorrectOutput(password);
            Assert.IsNotNull(o1);
            Assert.IsTrue(testOutput.Length == o1.Length);
            DocumentHelperTest.ShowDoucment(DocumentHelper.BytesToXpsDocument(ep.DocumentData));
            for (int i = 0; i < testOutput.Length; i++)
            {
                Assert.IsTrue(testOutput[i] == o1[i]);
            }

        }
    }
}
