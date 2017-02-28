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

namespace JudgeSharp.ViewModels
{
    public enum MainWindowStatus
    {
        Running,Ready,Error
    }
    public class MainWindowViewModel : ViewModelBase,ICloseable
    {
        //This application uses SDI architecture for simplicity. However you can upgrade this application to use MDI instead.
        
        public ProblemSetViewModel ProblemSet
        {
            get { return (ProblemSetViewModel)GetValue(ProblemSetProperty); }
            set { SetValue(ProblemSetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProblemSet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProblemSetProperty =
            DependencyProperty.Register("ProblemSet", typeof(ProblemSetViewModel), typeof(MainWindowViewModel), new PropertyMetadata(null));

        
        public ObservableCollection<Compiler> Compilers
        {
            get { return (ObservableCollection<Compiler>)GetValue(CompilersProperty); }
            set { SetValue(CompilersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Compilers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompilersProperty =
            DependencyProperty.Register("Compilers", typeof(ObservableCollection<Compiler>), typeof(MainWindowViewModel), new PropertyMetadata(ToObservableCollection(CompilersManager.DefaultCompilers)));

        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> enumerable)
        {
            ObservableCollection<T> col = new ObservableCollection<T>(); 
            foreach (var item in enumerable)
            {
                col.Add(item);
            }
            return col;
        }
        
        public Compiler SelectedCompiler
        {
            get { return (Compiler)GetValue(SelectedCompilerProperty); }
            set { SetValue(SelectedCompilerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedCompiler.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedCompilerProperty =
            DependencyProperty.Register("SelectedCompiler", typeof(Compiler), typeof(MainWindowViewModel), new PropertyMetadata(null));



        public string SourceFile
        {
            get { return (string)GetValue(SourceFileProperty); }
            set { SetValue(SourceFileProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SourceFile.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceFileProperty =
            DependencyProperty.Register("SourceFile", typeof(string), typeof(MainWindowViewModel), new PropertyMetadata(null));




        public string StateText
        {
            get { return (string)GetValue(StateTextProperty); }
            private set { SetValue(StateTextKey, value); }
        }

        private static readonly DependencyPropertyKey StateTextKey =
            DependencyProperty.RegisterReadOnly("StateText", typeof(string), typeof(MainWindowViewModel), new PropertyMetadata("Ready"));

        // Using a DependencyProperty as the backing store for StatusText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateTextProperty = StateTextKey.DependencyProperty;
        

        public MainWindowStatus State
        {
            get { return (MainWindowStatus)GetValue(StateProperty); }
            private set { SetValue(StateKey, value); }
        }

        private static readonly DependencyPropertyKey StateKey =
            DependencyProperty.RegisterReadOnly("State", typeof(MainWindowStatus), typeof(MainWindowViewModel), new PropertyMetadata(MainWindowStatus.Ready));

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StateProperty = StateKey.DependencyProperty;




        public RelayCommand NewCommand { get; private set; }
        public RelayCommand OpenCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand SaveAsCommand { get; private set; }
        public RelayCommand CloseCommand { get; private set; }

        public RelayCommand BrowseSourceFileCommand { get; private set; }
        public RelayCommand SolveSelectedProblemCommand { get; private set; }

        public MainWindowViewModel()
        {
            Name = "Judge Sharp";

            NewCommand = new RelayCommand(p => OnNew());
            OpenCommand = new RelayCommand(p => OnOpen());
            SaveCommand = new RelayCommand(p => OnSave(), p => ProblemSet != null && ProblemSet.IsDirty);
            SaveAsCommand = new RelayCommand(p => OnSaveAs(), p => ProblemSet != null);
            CloseCommand = new RelayCommand(p => OnClose(), p => ProblemSet != null);
            
            BrowseSourceFileCommand = new RelayCommand(p => OnBrowseSourceFile(), p => ProblemSet != null && ProblemSet.SelectedProblem != null && SelectedCompiler != null);
            SolveSelectedProblemCommand = new RelayCommand(p => OnSolveSelectedProblem(), p => ProblemSet != null && SelectedCompiler != null
                                        && ProblemSet.SelectedProblem != null && ProblemSet.SelectedProblem.IsLocked && SourceFile != null && File.Exists(SourceFile));
        }

        private void OnBrowseSourceFile()
        {
            if (SelectedCompiler == null)
                return;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = string.Format("{0} file {1}|{1}", SelectedCompiler.Name, SelectedCompiler.SourceExtentions);
            ofd.Multiselect = true;
            var r = ofd.ShowDialog();
            if(r.HasValue && r.Value)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < ofd.FileNames.Length; i++)
                {
                    if (i != 0)
                        sb.Append(";");
                    sb.Append(ofd.FileNames[i]);
                }
                SourceFile = sb.ToString();
            }
        }

        private void OnSolveSelectedProblem()
        {
            string[] sourceFiles = SourceFile.Split(';');
            StateText = string.Format("Testing {0} problem on {1} compiler", ProblemSet.SelectedProblem.Name, SelectedCompiler.Name);
            State = MainWindowStatus.Running;
            ProblemSpecification ps = ProblemSet.SelectedProblem.ProblemSpecification;
            Compiler c = SelectedCompiler;
            var t = Task.Run(() =>
             tester.TestFiles(sourceFiles, ps, c));
            t.ContinueWith((task) =>
            {
                if (t.IsFaulted)
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        StateText = string.Format("Error while testing problem {0}: {1}", ProblemSet.SelectedProblem.Name,t.Exception.Message);
                        State = MainWindowStatus.Error;
                    }));
                }
                else
                {
                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ProblemSet.SelectedProblem.SetSolution(t.Result);
                        StateText = string.Format("Problem {0} tested", ProblemSet.SelectedProblem.Name);
                        State = MainWindowStatus.Ready;
                    }));
                }
            });
        }

        private async Task<bool> OnClose()
        {
            if(ProblemSet != null && ProblemSet.IsDirty)
            {
                var r = MessageBox.Show("Do you want to save before close?", "Closing", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (r == MessageBoxResult.Cancel)
                    return false;
                else if(r == MessageBoxResult.Yes)
                {
                    if (! await OnSave().ConfigureAwait(false))
                        return false;
                }
            }
            ProblemSet = null;
            return true;
        }

        private async Task<bool> OnSaveAs()
        { 
            if (ProblemSet == null)
                return true;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Judge Sharp Problem Set (*.jsps)|*.jsps";
            var r = sfd.ShowDialog();
            if(r.HasValue && r.Value)
            {
                ProblemSet.Path = sfd.FileName;
                State = MainWindowStatus.Running;
                StateText = "Saving Problem Set";
                var result = await ProblemSet.SaveAsync().ConfigureAwait(false);
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (result)
                    {
                        State = MainWindowStatus.Ready;
                        StateText = "Problem Set Saved";
                    }
                    else
                    {
                        State = MainWindowStatus.Error;
                        StateText = "Failed to Save Problem Set";
                    }
                }));
                return result;
            }
            return false;
        }

        private async Task<bool> OnSave()
        {
            if (ProblemSet == null || !ProblemSet.IsDirty)
                return true;
            if(File.Exists(ProblemSet.Path))
            {
                State = MainWindowStatus.Running;
                StateText = "Saving Problem Set";
                var result = await ProblemSet.SaveAsync().ConfigureAwait(false);
                await Dispatcher.BeginInvoke((Action)(() =>
                {
                    if (result)
                    {
                        State = MainWindowStatus.Ready;
                        StateText = "Problem Set Saved";
                    }
                    else
                    {
                        State = MainWindowStatus.Error;
                        StateText = "Failed to Save Problem Set";
                    }
                }));
                return result;
            }
            else
            {
                return await OnSaveAs().ConfigureAwait(false);
            }
        }

        private async void OnOpen()
        {
            if(await OnNew().ConfigureAwait(false))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Judge Sharp Problem Set (*.jsps)|*.jsps";
                var r = ofd.ShowDialog();
                if(r.HasValue && r.Value)
                {
                    ProblemSet.Path = ofd.FileName;

                    State = MainWindowStatus.Running;
                    StateText = "Opening Problem Set";
                    var result = await ProblemSet.LoadAsync().ConfigureAwait(false);
                    await Dispatcher.BeginInvoke((Action)(() =>
                    {
                        if (result)
                        {
                            State = MainWindowStatus.Ready;
                            StateText = "Problem Set Opened";
                        }
                        else
                        {
                            State = MainWindowStatus.Error;
                            StateText = "Failed to Open Problem Set";
                        }

                    }));
                }
            }
        }

        private async Task<bool> OnNew()
        {
            if (! await OnClose())
                return false;
            ProblemSet = new ProblemSetViewModel();
            State = MainWindowStatus.Ready;
            StateText = "Ready";
            return true;
        }

        public async Task<bool> Close()
        {
            return await OnClose();
        }

        private Tester tester = new Tester();
    }
}
