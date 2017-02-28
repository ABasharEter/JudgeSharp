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


namespace JudgeSharpTest
{
    [TestClass]
    public class EncryptionHelperTest
    {
        [TestMethod]
        public void NormalHashTest()
        {
            string text1 = "some text to hash";
            string text2 = "$0me text t0 h@$h";
            string h1 = EncryptionHelper.Hash(text1);
            string h2 = EncryptionHelper.Hash(text1);
            string h3 = EncryptionHelper.Hash(text2);

            Assert.IsTrue(h1 != h2);
            Assert.IsTrue(h1 != h3);
            Assert.IsTrue(h2 != h3);

            Assert.IsTrue(EncryptionHelper.ValidateHash(text1, h1));
            Assert.IsTrue(EncryptionHelper.ValidateHash(text1, h2));
            Assert.IsTrue(EncryptionHelper.ValidateHash(text2, h3));
            Assert.IsTrue(!EncryptionHelper.ValidateHash(text1, h3));

            string salat = EncryptionHelper.ExtractSalat(h1);
            string h4 = EncryptionHelper.Hash(text1, salat);
            Assert.IsTrue(h1 == h4);
        }

        [TestMethod]
        public void FastHashTest()
        {
            byte[] data1 = new byte[1000];
            byte[] data2 = new byte[1000];
            Random r = new Random();
            r.NextBytes(data1);
            for (int i = 0; i < 1000; i++)
            {
                data2[i] = data1[i];
            }
            data2[500]++;

            string h1 = EncryptionHelper.FastHash(data1);
            string h2 = EncryptionHelper.FastHash(data1);
            string h3 = EncryptionHelper.FastHash(data2);

            Assert.IsTrue(h1 != h2);
            Assert.IsTrue(h1 != h3);
            Assert.IsTrue(h2 != h3);

            Assert.IsTrue(EncryptionHelper.FastHashValidation(data1, h1));
            Assert.IsTrue(EncryptionHelper.FastHashValidation(data1, h2));
            Assert.IsTrue(EncryptionHelper.FastHashValidation(data2, h3));
            Assert.IsTrue(!EncryptionHelper.FastHashValidation(data1, h3));
        }


        [TestMethod]
        public void EncryptionTest()
        {
            string text = "some text to encrypt";
            string pass1 = "the password";
            string pass2 = "the p@$$w0rd";

            string e1 = EncryptionHelper.Encrypt(text, pass1);
            string e2 = EncryptionHelper.Encrypt(text, pass1);
            string e3 = EncryptionHelper.Encrypt(text, pass2);

            string d1 = EncryptionHelper.Decrypt(e1, pass1);
            string d2 = EncryptionHelper.Decrypt(e2, pass1);
            string d3 = EncryptionHelper.Decrypt(e3, pass2);
            string d4 = EncryptionHelper.Decrypt(e3, pass1);

            Assert.IsTrue(e1 != e2);
            Assert.IsTrue(e2 != e3);
            Assert.IsTrue(e3 != e1);


            Assert.IsTrue(text == d1);
            Assert.IsTrue(text == d2);
            Assert.IsTrue(text == d3);
            Assert.IsTrue(text != d4);

        }
    }
}
