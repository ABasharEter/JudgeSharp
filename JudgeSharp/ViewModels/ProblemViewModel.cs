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

using JudgeSharp.Core;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace JudgeSharp.ViewModels
{
    public enum ProblemState
    {
        NotTested,Solved,FailedToSolve
    }
    public abstract class ProblemViewModel : ViewModelBase
    {
        public static ProblemViewModel CreateFromStream(Stream s)
        {
            //TODO: add the specifications of other type of problems here
            BinaryReader br = new BinaryReader(s);
            Guid id = new Guid(br.ReadBytes(16));
            if( id == typeof(EqualityCompareProblemViewModel).GUID)
            {
                return new EqualityCompareProblemViewModel();
            }
            else
            {
                return null;
            }
        }


        public static void WriteProblemHeader(Stream s,ProblemViewModel problem)
        {
            BinaryWriter br = new BinaryWriter(s);
            Guid id = problem.GetType().GUID;
            br.Write(id.ToByteArray());
            br.Flush();
        }


        public long MemoryLimit
        {
            get { return (long)GetValue(MemoryLimitProperty); }
            set { SetValue(MemoryLimitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MemoryLimit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MemoryLimitProperty =
            DependencyProperty.Register("MemoryLimit", typeof(long), typeof(ProblemViewModel), new PropertyMetadata(64L));
        
        public double TimeLimit
        {
            get { return (double)GetValue(TimeLimitProperty); }
            set { SetValue(TimeLimitProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeLimit.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeLimitProperty =
            DependencyProperty.Register("TimeLimit", typeof(double), typeof(ProblemViewModel), new PropertyMetadata(500.0));
        

        public DateTime? SolutionTime
        {
            get { return (DateTime?)GetValue(SolutionTimeProperty); }
            private set { SetValue(SolutionTimeKey, value); }
        }

        private static readonly DependencyPropertyKey SolutionTimeKey =
            DependencyProperty.RegisterReadOnly("SolutionTime", typeof(DateTime?), typeof(ProblemViewModel), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for SolutionTime.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SolutionTimeProperty = SolutionTimeKey.DependencyProperty;
        

        public int Points
        {
            get { return (int)GetValue(PointsProperty); }
            set { SetValue(PointsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Points.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PointsProperty =
            DependencyProperty.Register("Points", typeof(int), typeof(ProblemViewModel), new PropertyMetadata(10));
        
        

        public ProblemState State
        {
            get { return (ProblemState)GetValue(StateProperty); }
            protected set { SetValue(StateKey, value); }
        }
        protected static readonly DependencyPropertyKey StateKey =
            DependencyProperty.RegisterReadOnly("State", typeof(ProblemState), typeof(ProblemViewModel), new PropertyMetadata(ProblemState.NotTested));

        // Using a DependencyProperty as the backing store for State.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty = StateKey.DependencyProperty;
        

        public string StateText
        {
            get { return (string)GetValue(StateTextProperty); }
            private set { SetValue(StateTextKey, value); }
        }

        private static readonly DependencyPropertyKey StateTextKey =
            DependencyProperty.RegisterReadOnly("StateText", typeof(string), typeof(ProblemViewModel), new PropertyMetadata("Not Tested"));

        // Using a DependencyProperty as the backing store for StateText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateTextProperty = StateTextKey.DependencyProperty;



        public bool IsEditing
        {
            get { return (bool)GetValue(IsEditingProperty); }
            protected set { SetValue(IsEditingKey, value); }
        }
        protected static readonly DependencyPropertyKey IsEditingKey =
            DependencyProperty.RegisterReadOnly("IsEditing", typeof(bool), typeof(ProblemViewModel), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for IsEditing.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsEditingProperty = IsEditingKey.DependencyProperty;

        

        public bool IsNew
        {
            get { return (bool)GetValue(IsNewProperty); }
            protected set { SetValue(IsNewKey, value); }
        }

        protected static readonly DependencyPropertyKey IsNewKey =
            DependencyProperty.RegisterReadOnly("IsNew", typeof(bool), typeof(ProblemViewModel), new PropertyMetadata(true));

        // Using a DependencyProperty as the backing store for IsNew.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsNewProperty = IsNewKey.DependencyProperty;


        public bool IsDirty
        {
            get { return (bool)GetValue(IsDirtyProperty); }
            protected set { SetValue(IsDirtyKey, value); }
        }


        protected static readonly DependencyPropertyKey IsDirtyKey =
            DependencyProperty.RegisterReadOnly("IsDirty", typeof(bool), typeof(ProblemViewModel), new PropertyMetadata(false));

        // Using a DependencyProperty as the backing store for IsDirty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsDirtyProperty = IsDirtyKey.DependencyProperty;

        
        public FixedDocumentSequence Document
        {
            get { return (FixedDocumentSequence)GetValue(DocumentProperty); }
            private set { SetValue(DocumentKey, value); }
        }

        private static readonly DependencyPropertyKey DocumentKey =
            DependencyProperty.RegisterReadOnly("Document", typeof(FixedDocumentSequence), typeof(ProblemViewModel), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Document.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DocumentProperty = DocumentKey.DependencyProperty;

        
        public ObservableCollection<TestCaseViewModel> TestCases
        {
            get { return (ObservableCollection<TestCaseViewModel>)GetValue(TestCasesProperty); }
            private set { SetValue(TestCasesKey, value); }
        }

        private static readonly DependencyPropertyKey TestCasesKey =
            DependencyProperty.RegisterReadOnly("TestCases", typeof(ObservableCollection<TestCaseViewModel>), typeof(ProblemViewModel), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for TestCases.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TestCasesProperty = TestCasesKey.DependencyProperty;



        public TestCaseViewModel SelectedTestCase
        {
            get { return (TestCaseViewModel)GetValue(SelectedTestCaseProperty); }
            set { SetValue(SelectedTestCaseProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTestCase.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTestCaseProperty =
            DependencyProperty.Register("SelectedTestCase", typeof(TestCaseViewModel), typeof(ProblemViewModel), new PropertyMetadata(null));



        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Password.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(ProblemViewModel), new PropertyMetadata(""));


        public bool IsLocked
        {
            get
            {
                return !IsNew && State != ProblemState.Solved;
            }
        }
        
        public RelayCommand ToggelEditCommand { get; protected set; }
        public RelayCommand ToggelUnlockCommand { get; protected set; }
        public RelayCommand BrowseDocumentCommand { get; protected set; }

        public ProblemSpecification ProblemSpecification { get; protected set; }

        public ProblemViewModel()
        {
            Name = "New Problem";
            TestCases = new ObservableCollection<TestCaseViewModel>();
            ToggelEditCommand = new RelayCommand(p => OnToggelEdit(), p=> !IsLocked);
            BrowseDocumentCommand = new RelayCommand(p => OnBrowseDocument(), p => IsEditing);
            ToggelUnlockCommand = new RelayCommand(p => OnToggelUnlock(), p => !IsEditing);
        }

        protected virtual void OnBrowseDocument()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Xps Documents (*.xps)|*.xps";
            var r = ofd.ShowDialog();
            if(r.HasValue && r.Value)
            {
                FixedDocumentSequence doc = DocumentHelper.LoadXpsData(ofd.FileName);
                if (doc == null)
                    MessageBox.Show("Could not open the " + ofd.FileName + " Xps Document.");
                else
                    Document = doc;
            }
        }

        protected virtual void OnToggelUnlock()
        {
            if(IsLocked)
            {
                var output = ProblemSpecification.GetCorrectOutput(Password);
                if(output == null)
                {
                    StateText = "Failed to unlock!";
                    State = ProblemState.FailedToSolve;
                }
                else
                {
                    StateText = "Problem unlocked!";
                    State = ProblemState.Solved;
                    TestCases.Clear();
                    for (int i = 0; i < ProblemSpecification.TestInput.Length; i++)
                    {
                        var tc = new TestCaseViewModel();
                        tc.Input = ProblemSpecification.TestInput[i];
                        tc.Output = output[i];
                        TestCases.Add(tc);
                    }
                    
                }
            }
            else
            {
                StateText = "Problem locked!";
                TestCases.Clear();
                Password = "";
                State = ProblemState.NotTested;
                IsNew = false;
            }
        }

        protected virtual void OnToggelEdit()
        {
            if(IsEditing)
            {
                CommitChanges();
                IsDirty = true;
            }
            IsEditing = !IsEditing;
        }

        protected abstract void CommitChanges();
        public void SetSolution(TestResult result)
        {
            switch (result.Result)
            {
                case TestResultType.AC:
                    string password = ProblemSpecification.GetPassword(result.Output);
                    if (password == null)
                    {
                        StateText = string.Format("Error in problem file or the probelem have no test cases. Must Be accepted. Time:{0} Memory:{1}", result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                        State = ProblemState.FailedToSolve;
                    }
                    else
                    {
                        TestCases.Clear();
                        for (int i = 0; i < ProblemSpecification.TestInput.Length; i++)
                        {
                            var tc = new TestCaseViewModel();
                            tc.Input = ProblemSpecification.TestInput[i];
                            tc.Output = result.Output[i];
                            TestCases.Add(tc);
                        }
                        Password = password;
                        State = ProblemState.Solved;
                        StateText = string.Format("Accepted!! Time:{0} Memory:{1}", result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                    }
                    break;
                case TestResultType.WA:
                    State = ProblemState.FailedToSolve;
                    StateText = string.Format("Wrong answer on testcase {0}!! Time:{1} Memory:{2}", result.Output.Length, result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                    break;
                case TestResultType.CTE:
                    State = ProblemState.FailedToSolve;
                    StateText = string.Format("Compile time error!! compiler output:{0}", result.CompilerOutput);
                    break;
                case TestResultType.RTE:
                    State = ProblemState.FailedToSolve;
                    StateText = string.Format("Run-time error on testcase {0}!! Time:{1} Memory:{2}", result.Output.Length, result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                    break;
                case TestResultType.TLE:
                    State = ProblemState.FailedToSolve;
                    StateText = string.Format("Time limit ecceded on testcase {0}!! Time:{1} Memory:{2}", result.Output.Length, result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                    break;
                case TestResultType.MLE:
                    State = ProblemState.FailedToSolve;
                    StateText = string.Format("Memory limit ecceded on testcase {0}!! Time:{1} Memory:{2}", result.Output.Length, result.ResourceUsage.TimeUsage, result.ResourceUsage.MemoryUsage);
                    break;
                default:
                    break;
            }
        }

        public virtual bool Load(Stream stream)
        {
            if(!ProblemSpecification.Load(stream))
            {
                return false;
            }
            Dispatcher.BeginInvoke((Action)(() =>
            {
                if (ProblemSpecification.DocumentData != null)
                    Document = DocumentHelper.BytesToXpsDocument(ProblemSpecification.DocumentData);
                else
                    Document = null;
                Name = ProblemSpecification.Name;
                State = ProblemState.NotTested;
                StateText = "Not Tested";
                MemoryLimit = ProblemSpecification.MemoryLimit / 1024 / 1024;
                TimeLimit = ProblemSpecification.TimeLimit;
                IsDirty = false;
                IsEditing = false;
                IsNew = false;
            }));
            return true;
        }
        public virtual bool Save(Stream stream)
        {
            if (!ProblemSpecification.Save(stream))
            {
                return false;
            }
            Dispatcher.BeginInvoke((Action)(() =>
            {
                IsDirty = false;
            }));
            return true;
        }

    }
}
