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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace eJay.View
{
    /// <summary>
    /// Interaktionslogik für ScreenshotView.xaml
    /// </summary>
    public partial class ScreenshotView : Window
    {
        public ScreenshotView()
        {
            InitializeComponent();

            var h = new WindowInteropHelper(this);
            h.EnsureHandle();
        }
    }
}
