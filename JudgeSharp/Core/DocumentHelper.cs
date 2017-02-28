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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Xps.Packaging;
using System.Windows.Documents;
using System.IO.Packaging;
using System.IO;
using System.Windows.Xps;

namespace JudgeSharp.Core
{
    public static class DocumentHelper
    {

        public static byte[] XpsDocumentToBytes(FixedDocumentSequence fds)
        {
            string uriString = "pack://" + Guid.NewGuid().ToString() + ".xps";
            Uri uri = new Uri(uriString);
            MemoryStream stream = new MemoryStream();
            Package package = Package.Open(stream,FileMode.Create);
            PackageStore.AddPackage(uri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Maximum, uriString);
            XpsDocumentWriter writer = XpsDocument.CreateXpsDocumentWriter(xpsDocument);
            writer.Write(fds);
            xpsDocument.Close();
            package.Close();
            stream.Close();
            PackageStore.RemovePackage(uri);
            MemoryStream mainstream = new MemoryStream();
            BinaryWriter br = new BinaryWriter(mainstream, Encoding.Default, true);
            byte[] data = stream.ToArray();
            br.Write(uriString);
            br.Write(data.Length);
            br.Write(data);
            mainstream.Close();
            return mainstream.ToArray();
        }

        public static FixedDocumentSequence BytesToXpsDocument(byte[] bytes)
        {
            Package package;
            Stream stream = new MemoryStream(bytes);
            BinaryReader br = new BinaryReader(stream, Encoding.Default, true);
            string uriString = br.ReadString();
            int length = br.ReadInt32();
            byte[] docData = br.ReadBytes(length);
            stream.Close();
            stream = new MemoryStream(docData);
            package = Package.Open(stream);
            Uri uri = new Uri(uriString);
            PackageStore.AddPackage(uri, package);
            XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.Maximum, uriString);
            return xpsDocument.GetFixedDocumentSequence();
        }

        public static FixedDocumentSequence LoadXpsData(string fileName)
        {
            try
            {
                XpsDocument xpsDocument = new XpsDocument(fileName, FileAccess.ReadWrite);
                var fds = xpsDocument.GetFixedDocumentSequence();
                xpsDocument.Close();
                return fds;
            }
            catch
            {
                return null;
            }
        }

        public static byte[] Pack(byte[] data)
        {
            string hash = EncryptionHelper.FastHash(data);
            MemoryStream memoryStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(memoryStream,Encoding.Default,true);
            bw.Write(hash);
            bw.Write(data.Length);
            bw.Write(data);
            memoryStream.Close();
            return memoryStream.ToArray();
        }
        public static byte[] UnpackData(byte[] data)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(data);
                BinaryReader br = new BinaryReader(memoryStream, Encoding.Default, true);
                string hash = br.ReadString();
                int length = br.ReadInt32();
                byte[] unpackdata = br.ReadBytes(length);
                if (EncryptionHelper.FastHashValidation(unpackdata, hash))
                    return unpackdata;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }

    }
}
