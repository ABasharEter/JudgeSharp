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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using JudgeSharp.Core;
using System.IO;
using System.Diagnostics;

namespace JudgeSharp.Compilers
{
    public class GCCCPPCompiler : Compiler
    {

        public override string Name
        {
            get
            {
                return "GCC C++ Compiler";
            }
        }

        public GCCCPPCompiler()
        {
            try
            {

                string code = ReadDummyFile();
                string tempsource = Path.Combine(Path.GetTempPath(), "dummy.cpp");
                File.WriteAllText(tempsource, code);
                string tempexe = Path.Combine(Path.GetTempPath(), "dummy.exe");
                string gccout = Compile(new string[] { tempsource }, tempexe);
                if (gccout != null)
                    throw new Exception("Can't compile dummy code.");
                Tester t = new Tester();
                string output;
                dummyResource = t.TestRun(tempexe, "99",out output);
                if (gccout != null)
                    throw new Exception("Can't test dummy code.");
            }
            catch(Exception e)
            {
                throw new Exception("Can't initalize gcc compiler!: " + e.Message);
            }
        }


        public override TestResourceResult ReletiveUsage(TestResourceResult usage)
        {
            usage.MemoryUsage  = (long)(usage.MemoryUsage * dummyMemory/ (double)(dummyResource.MemoryUsage) );
            usage.TimeUsage *= dummyTime / dummyResource.TimeUsage ;
            return usage;
        }

        public override string SourceExtentions
        {
            get
            {
                return "*.cpp;*.c;*.h;*.hpp";
            }
        }

        public override string Compile(string[] sourceFiles, string exeFile)
        {
            try
            {
                Process p = new Process();
                string workingDire = Path.GetDirectoryName(exeFile);
                p.StartInfo.FileName = "g++";
                p.StartInfo.Arguments = "-Wall -std=c++11 -o \"" + Path.GetFileName(exeFile) + "\" ";
                foreach (var source in sourceFiles)
                {
                    p.StartInfo.Arguments += string.Format("\"{0}\"", source);
                }
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.WorkingDirectory = workingDire;
                p.Start();
                for (int i = 0; i < 500; i++)
                    if (p.WaitForExit(100))
                        break;
                if(p.HasExited)
                {
                    if (p.ExitCode == 0 && File.Exists(exeFile))
                        return null;
                    else
                        return p.StandardOutput.ReadToEnd() + "\n" + p.StandardError.ReadToEnd();
                }
                else
                {
                    p.Kill();
                    return "Error: g++ compile timeout";
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }

        private static string ReadDummyFile()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "JudgeSharp.Compilers.dummy.cpp";
            var n = assembly.GetManifestResourceNames();
            using (System.IO.Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
        
        private const long dummyMemory = 50*1024*1024L;
        private const double dummyTime = 500;
        private TestResourceResult dummyResource;

    }
}
