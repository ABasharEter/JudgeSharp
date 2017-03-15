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
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using JudgeSharp.Core;

namespace JudgeSharp.ViewModels
{
    public class ProblemSetViewModel : ViewModelBase
    {
        public ObservableCollection<ProblemViewModel> Problems
        {
            get { return (ObservableCollection<ProblemViewModel>)GetValue(ProblemsProperty); }
            private set { SetValue(ProblemsKey, value); }
        }

        private static readonly DependencyPropertyKey ProblemsKey =
            DependencyProperty.RegisterReadOnly("Problems", typeof(ObservableCollection<ProblemViewModel>), typeof(ProblemSetViewModel), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for Problems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProblemsProperty = ProblemsKey.DependencyProperty;
        
        public ProblemViewModel SelectedProblem
        {
            get { return (ProblemViewModel)GetValue(SelectedProblemProperty); }
            set { SetValue(SelectedProblemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedProblem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedProblemProperty =
            DependencyProperty.Register("SelectedProblem", typeof(ProblemViewModel), typeof(ProblemSetViewModel), new PropertyMetadata(null));
        
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Path.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PathProperty =
            DependencyProperty.Register("Path", typeof(string), typeof(ProblemSetViewModel), new PropertyMetadata(null));


        bool isDirty = false;
        public bool IsDirty
        {
            get
            {
                return isDirty || Problems.Any(p => p.IsDirty);
            }
            set
            {
                isDirty = value;
            }
        }
        public bool IsLocked
        {
            get
            {
                return Problems.Any(p => p.IsLocked);
            }
        }


        //TODO: If you implements another type of problems then add a command to ad it to the problem set here
        
        public RelayCommand AddEqualityProblemCommand { get; private set; }
        public RelayCommand RemoveProblemCommand { get; private set; }



        public ProblemSetViewModel()
        {
            //Don't put the initialization in the dependency property default value it will be the same for all instances of this class
            Problems = new ObservableCollection<ProblemViewModel>();
            AddEqualityProblemCommand = new RelayCommand(p => OnAddEqualityProblem(), p => !IsLocked);
            RemoveProblemCommand = new RelayCommand(p => OnRemoveProblem(), p => !IsLocked);
            Name = "New problem set";
        }

        private void OnRemoveProblem()
        {
            var r = MessageBox.Show("Are you sure?","Removing problem",MessageBoxButton.OKCancel,MessageBoxImage.Question);
            if(r == MessageBoxResult.OK)
            {
                Problems.Remove(SelectedProblem);
                SelectedProblem = null;
                IsDirty = true;
            }
        }

        private void OnAddEqualityProblem()
        {
            SelectedProblem = new EqualityCompareProblemViewModel();
            Problems.Add(SelectedProblem);
            IsDirty = true;
        }

        public async Task<bool> SaveAsync()
        {
            if (string.IsNullOrEmpty(Path))
                return false;
            try
            {
                byte[] data = null;
                ProblemViewModel[] problems = Problems.ToArray();
                string path = Path;
                if (!await Task.Factory.StartNew(() =>
                 {
                     MemoryStream ms = new MemoryStream();
                     foreach (var problem in problems)
                     {
                         ProblemViewModel.WriteProblemHeader(ms, problem);
                         if (!problem.Save(ms)) 
                            return false;
                     }
                     ms.Close();
                     data = DocumentHelper.Pack(ms.ToArray());
                     return true;
                 }).ConfigureAwait(false))
                    return false;
                if (data == null)
                    return false;
                FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
                await fs.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
                IsDirty = false;
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> LoadAsync()
        {
            if (string.IsNullOrEmpty(Path))
                return false;
            try
            {
                FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read);
                byte[] data = new byte[fs.Length];
                await fs.ReadAsync(data, 0, data.Length).ConfigureAwait(false);
                List<ProblemViewModel> ps = new List<ProblemViewModel>();
                data = DocumentHelper.UnpackData(data);
                if (data == null)
                    return false;
                MemoryStream ms = new MemoryStream(data);
                while (ms.Position < ms.Length)
                {
                    ProblemViewModel problem = null;
                    Dispatcher.BeginInvoke((Action)(() => {
                        problem = ProblemViewModel.CreateFromStream(ms);
                    })).Wait();
                    if (problem == null)
                        return false;
                    if (!await Task.Factory.StartNew(() =>
                     {
                         if (problem == null || !problem.Load(ms))
                             return false;
                         return true;
                     }).ConfigureAwait(false))
                        return false;
                    ps.Add(problem);
                }
                ms.Close();
                Dispatcher.BeginInvoke((Action)(() => { 
                Problems.Clear();
                foreach (var problem in ps)
                {
                    Problems.Add(problem);
                    }
                }));
                IsDirty = false;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
