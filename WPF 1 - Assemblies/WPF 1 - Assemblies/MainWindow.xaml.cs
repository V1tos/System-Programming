using Microsoft.Win32;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
        AppDomain test;
        Assembly assembly;
        MethodInfo selectedMethod;
        string assemblyName;
        object obj;

        public MainWindow()
        {
            InitializeComponent();
            test = AppDomain.CreateDomain("Test");
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                int deleteIndex = openFileDialog.SafeFileName.IndexOf('.');
                assemblyName = openFileDialog.SafeFileName.Remove(deleteIndex);
                lblResult.Content = assemblyName;
                assembly = test.Load(assemblyName);
                var classes = assembly.GetTypes();
                lbAssemblies.ItemsSource = classes.Select(x=>x.Name);
            }
        }

        private void lbAssemblies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbMethods.SelectionChanged -= lbMethods_SelectionChanged;
            lbMethods.Items.Clear();
            spParameters.Children.Clear();
            var selectedClass = lbAssemblies.SelectedItem.ToString();
            lblResult.Content = selectedClass;
            string path = $"{assemblyName}.{selectedClass}";
            var clas = assembly.GetType(path);
            obj = Activator.CreateInstance(clas);
            
            var methods = clas.GetMethods();
            foreach (var item in methods)
            {
                lbMethods.Items.Add(item);
            }

            lbMethods.SelectionChanged += lbMethods_SelectionChanged;  
        }

        private void lbMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedMethod = lbMethods.SelectedItem as MethodInfo;
            var parametersCount = selectedMethod.GetParameters().Count();

            Label label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            TextBox textBox = new TextBox();
            textBox.Height = 30;
            textBox.Width = 50;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            spParameters.Children.Clear();
            for (int i = 0; i < parametersCount; i++)
            {
                label.Content = $"Parameter {i + 1}";
                spParameters.Children.Add(label);
                spParameters.Children.Add(textBox);
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {          
            var userParameters = new List<object>();
            var spParContent = spParameters.Children;

            List<TextBox> tbList = new List<TextBox>();

            var methodParameters = selectedMethod.GetParameters().ToArray();
            
            for (int i = 0; i < spParContent.Count; i++)
            {
                if (spParContent[i] is TextBox)
                    tbList.Add(spParContent[i] as TextBox);
            }

            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType == typeof(int))
                {
                    userParameters.Add(int.Parse(tbList[i].Text));
                }
                else
                {
                    userParameters.Add(tbList[i].Text);
                }
            }

            object result = selectedMethod.Invoke(obj, userParameters.ToArray());
            lblResult.Content = result.ToString();
        }
    }
}
