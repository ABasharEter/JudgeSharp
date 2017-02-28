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
using System.Windows;
using System.Windows.Controls;
using System.Text;

namespace JudgeSharpTest
{
    [TestClass]
    public class DocumentHelperTest
    {
        public static void ShowDoucment(System.Windows.Documents.FixedDocumentSequence doc)
        {
            Window w = new Window();
            DocumentViewer dv = new DocumentViewer();
            w.Content = dv;
            dv.Document = doc;
            w.ShowDialog();
        }
        [TestMethod]
        public void XpsDoucmentToolsTest()
        {
            string s = Environment.CurrentDirectory;
            var doc = DocumentHelper.LoadXpsData("Dummy Doucment.xps");
            Assert.IsNotNull(doc);
            ShowDoucment(doc);
            var docdata = DocumentHelper.XpsDocumentToBytes(doc);
            doc = DocumentHelper.BytesToXpsDocument(docdata);
            ShowDoucment(doc);
        }

        [TestMethod]
        public void PackagingToolsTest()
        {
            string someText = "some text data to work with";
            var data = Encoding.UTF8.GetBytes(someText);
            var packetData = DocumentHelper.Pack(data);
            var croptedData = packetData.Clone() as byte[];
            croptedData[0]--;
            var unpackedData = DocumentHelper.UnpackData(packetData);
            var unpackedCroptedData = DocumentHelper.UnpackData(croptedData);
            Assert.IsTrue(unpackedCroptedData == null);
            string orginalText = Encoding.UTF8.GetString(unpackedData);
            Assert.IsTrue(orginalText == someText);
        }
    }
}