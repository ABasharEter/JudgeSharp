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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JudgeSharp.Views
{
    /// <summary>
    /// Interaction logic for ColorBar.xaml
    /// </summary>
    public partial class ColorBar : UserControl
    {
        public int DotsCount
        {
            get { return (int)GetValue(DotsCountProperty); }
            set { SetValue(DotsCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DotsCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DotsCountProperty =
            DependencyProperty.Register("DotsCount", typeof(int), typeof(ColorBar), new PropertyMetadata(100, (s, e) => (s as ColorBar).UpdateDotsPosition()));
        

        public Color Color
        {
            get { return (Color)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Color.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Color), typeof(ColorBar), new PropertyMetadata(Colors.AliceBlue));



        public ColorBar()
        {
            InitializeComponent();
            UpdateDotsPosition();
            c1.SizeChanged += delegate { UpdateDotsPosition(); };
            c2.SizeChanged += delegate { UpdateDotsPosition(); };
        }

        private void UpdateDotsPosition()
        {
            c1.Children.Clear();
            c2.Children.Clear();
            Random r = new Random();
            Style s = FindResource("DotStyle") as Style;
            for (int i = 0; i < DotsCount; i++)
            {
                var xpos = c1.Width * r.NextDouble();
                var ypos = c1.Height * r.NextDouble();
                Ellipse e = new Ellipse();
                e.Style = s;
                e.Opacity = 0.3 + r.NextDouble();
                double rate = 1 + (r.NextDouble() - 0.5) * 0.2;
                double h = r.NextDouble() * 4;
                e.Height = 3 + h;
                e.Width = 3 + rate * h;
                e.SetValue(Canvas.LeftProperty, xpos);
                e.SetValue(Canvas.TopProperty, ypos);
                c1.Children.Add(e);

                xpos = c2.Width * r.NextDouble();
                ypos = c2.Height * r.NextDouble();
                e = new Ellipse();
                e.Style = s;
                e.Opacity = 0.3 + r.NextDouble();
                rate = 1 + (r.NextDouble() - 0.5) * 0.2;
                h = r.NextDouble() * 4;
                e.Height = 3 + h;
                e.Width = 3 + rate * h;
                e.SetValue(Canvas.LeftProperty, xpos);
                e.SetValue(Canvas.TopProperty, ypos);
                c2.Children.Add(e);
            }
        }
    }
}
