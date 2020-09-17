using Microsoft.Win32;
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

namespace WPF_1___Assemblies
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            AppDomain test = AppDomain.CreateDomain("Test");
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                int deleteIndex = openFileDialog.SafeFileName.IndexOf('.');
                string assemblyName = openFileDialog.SafeFileName.Remove(deleteIndex);
                lblResult.Content = assemblyName;
                var assembly = test.Load(assemblyName);
                var classes = assembly.GetTypes();
                lbAssemblies.ItemsSource = classes.Select(x=>x.Name);
            }
            //var types = asm.GetTypes();
            //foreach (var item in types)
            //{
            //    Console.WriteLine(item.Name);
            //}

        }
    }
}
