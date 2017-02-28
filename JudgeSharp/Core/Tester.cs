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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JudgeSharp.Core
{
    public class Tester
    {
        public const double MaxTimeLimit = 10000;

        public const long MaxMemoryLimit = 1024 * 1024 * 1024;
        public TestResult TestFiles(string[] sources,ProblemSpecification problem,Compiler compiler)
        {
            TestResult r1 = new TestResult();
            string exefileName = Path.Combine(Path.GetTempPath(), problem.Name +".exe");

            r1.CompilerOutput = compiler.Compile(sources, exefileName);
            if(r1.CompilerOutput != null)
            {
                r1.Result = TestResultType.CTE;
                return r1;
            }
            List<string> outputs = new List<string>();
            int i = 0;
            foreach (var input in problem.TestInput)
            {
                string output;
                TestResourceResult r2 = TestRun(exefileName, input, out output);
                r2 = compiler.ReletiveUsage(r2);
                r2.MemoryUsage = Math.Max(r2.MemoryUsage, r1.ResourceUsage.MemoryUsage);
                if (Double.IsNaN(r2.TimeUsage))
                {
                    r1.Result = TestResultType.RTE;
                    r1.Output = outputs.ToArray();
                    return r1;
                }
                r2.TimeUsage = Math.Max(r2.TimeUsage, r1.ResourceUsage.TimeUsage);
                r1.ResourceUsage = r2;
                if (r2.TimeUsage > problem.TimeLimit)
                {
                    r1.Result = TestResultType.TLE;
                    r1.Output = outputs.ToArray();
                    return r1;
                }
                if (r2.MemoryUsage > problem.MemoryLimit)
                {
                    r1.Result = TestResultType.MLE;
                    r1.Output = outputs.ToArray();
                    return r1;
                }
                if (!problem.IsCorrect(output,i))
                {
                    r1.Result = TestResultType.WA;
                    r1.Output = outputs.ToArray();
                    return r1;
                }
                outputs.Add(output);
                i++;
            }
            r1.Output = outputs.ToArray();
            return r1;
        }

        public TestResourceResult TestRun(string exeFile,string input,out string output)
        {
            try
            {
                TestResourceResult r = new TestResourceResult();
                Process p = new Process();
                p.StartInfo.FileName = exeFile;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.Start();
                p.PriorityClass = ProcessPriorityClass.AboveNormal;
                p.Refresh();
                r.MemoryUsage = p.PeakWorkingSet64;
                p.WaitForExit(1);
                p.Refresh();
                r.MemoryUsage = p.PeakWorkingSet64;
                p.StandardInput.WriteLine(input);
                p.StandardInput.Close();
                do
                {
                    if (!p.HasExited)
                    {
                        p.Refresh();
                        r.MemoryUsage = Math.Max(r.MemoryUsage, p.PeakWorkingSet64);
                        if (r.MemoryUsage > MaxMemoryLimit)
                            break;
                    }
                } while ( (DateTime.Now - p.StartTime).TotalMilliseconds < MaxTimeLimit && !p.WaitForExit(10));
                if(p.HasExited)
                {
                    output = p.StandardOutput.ReadToEnd();
                    if (p.ExitCode == 0)
                    {
                        r.TimeUsage = (p.ExitTime - p.StartTime).Milliseconds;
                    }
                    else
                    {
                        r.TimeUsage = double.NaN;
                    }
                }
                else
                {
                    p.Kill();
                    output = "";
                    r.TimeUsage = double.PositiveInfinity;
                }
                return r;
            }
            catch
            {
                TestResourceResult r = new TestResourceResult();
                r.MemoryUsage = 0;
                r.TimeUsage = double.NaN;
                output = "";
                return r;
            }
        }
    }
}
