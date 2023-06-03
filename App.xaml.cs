using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AutoFix
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string AdminPosition = "Директор";
        public static Employee? LoggedInEmployee { get; set; }
    }
}
