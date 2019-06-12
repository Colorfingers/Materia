﻿using System;
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
using D = System.Drawing;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using OpenTK;
using Materia.MathHelpers;

namespace Materia.UI.Components
{
    /// <summary>
    /// Interaction logic for ColorSelect.xaml
    /// </summary>
    public partial class ColorSelect : UserControl
    {
        MVector current;
        D.Color c;
        PropertyInfo property;
        object propertyOwner;

        public ColorSelect()
        {
            InitializeComponent();
            SelectColor.Background = new SolidColorBrush(Colors.Black);
        }

        public ColorSelect(PropertyInfo p, object owner)
        {
            InitializeComponent();
            property = p;
            propertyOwner = owner;

            MVector m = (MVector)p.GetValue(owner);
            current = m;

            c = D.Color.FromArgb((int)(current.W * 255), (int)(current.X * 255), (int)(current.Y * 255), (int)(current.Z * 255));
            SelectColor.Background = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));
        }

        private void SelectColor_Click(object sender, RoutedEventArgs e)
        {
            ColorPicker cp = new ColorPicker(c);
            cp.Owner = MainWindow.Instance;
            cp.ShowDialog();

            if(cp.DialogResult == true)
            {
                var r = cp.Selected;
                c = r;

                current.X = r.R / 255.0f;
                current.Y = r.G / 255.0f;
                current.Z = r.B / 255.0f;
                current.W = r.A / 255.0f;

                SelectColor.Background = new SolidColorBrush(Color.FromArgb(c.A, c.R, c.G, c.B));

                property.SetValue(propertyOwner, current);
            }
        }
    }
}
