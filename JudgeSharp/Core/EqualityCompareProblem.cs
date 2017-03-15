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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSharp.Core
{
    public class EqualityCompareProblem : ProblemSpecification
    {
        public string[] OutputHash { get; private set; }
        public string[] OutputCipher { get; private set; }

        public EqualityCompareProblem()
        {

        }

        public bool Initialize(string[] answers, string[] input, string password)
        {
            PasswordHash = EncryptionHelper.Hash(password);
            string totalanswers = "";
            if (input.Length != answers.Length)
                return false;
            TestInput = input.Clone() as string[];
            OutputHash = new string[answers.Length];
            OutputCipher = new string[answers.Length];
            for (int i = 0; i < answers.Length; i++)
            {
                string answer = FormatOutput(answers[i]);
                totalanswers += answer;
                OutputHash[i] = EncryptionHelper.Hash(answer);
                OutputCipher[i] = EncryptionHelper.Encrypt(answer, password);
            }
            PasswordCipher = EncryptionHelper.Encrypt(password, totalanswers);
            return true;
        }

        public override string[] GetCorrectOutput(string password)
        {
            if (EncryptionHelper.ValidateHash(password, PasswordHash))
            {
                List<string> answers = new List<string>();
                for (int i = 0; i < OutputCipher.Length; i++)
                {
                    answers.Add(EncryptionHelper.Decrypt(OutputCipher[i], password));
                }
                return answers.ToArray();
            }
            return null;
        }

        public override bool IsCorrect(string output, int index)
        {
            return EncryptionHelper.ValidateHash(FormatOutput(output), OutputHash[index]);
        }

        public override bool Load(Stream stream)
        {
            try
            {
                BinaryReader br = new BinaryReader(stream);
                Name = br.ReadString();
                TimeLimit = br.ReadDouble();
                MemoryLimit = br.ReadInt64();
                PasswordCipher = br.ReadString();
                PasswordHash = br.ReadString();
                int length = br.ReadInt32();
                TestInput = new string[length];
                OutputCipher = new string[length];
                OutputHash = new string[length];
                for (int i = 0; i < length; i++)
                {
                    TestInput[i] = br.ReadString();
                    OutputCipher[i] = br.ReadString();
                    OutputHash[i] = br.ReadString();
                }
                length = br.ReadInt32();
                DocumentData = (length == 0) ? null : br.ReadBytes(length);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override bool Save(Stream stream)
        {
            try
            {
                BinaryWriter bw = new BinaryWriter(stream);
                bw.Write(Name);
                bw.Write(TimeLimit);
                bw.Write(MemoryLimit);
                bw.Write(PasswordCipher);
                bw.Write(PasswordHash);
                bw.Write(TestInput.Length);
                for (int i = 0; i < TestInput.Length; i++)
                {
                    bw.Write(TestInput[i]);
                    bw.Write(OutputCipher[i]);
                    bw.Write(OutputHash[i]);
                }
                if (DocumentData == null || DocumentData.Length == 0)
                    bw.Write(0);
                else
                {
                    bw.Write(DocumentData.Length);
                    bw.Write(DocumentData);
                }
                bw.Flush();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override string GetPassword(string[] correctOutput)
        {
            string totalanswers = "";

            for (int i = 0; i < correctOutput.Length; i++)
            {
                string answer = FormatOutput(correctOutput[i]);
                if (!IsCorrect(answer, i))
                    return null;
                totalanswers += answer;
            }
            return EncryptionHelper.Decrypt(PasswordCipher, totalanswers);
        }
    }
}
