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
            lblResult.Content = obj.ToString();
            var methods = clas.GetMethods();
            foreach (var item in methods)
            {
                lbMethods.Items.Add(item);
            }

            lbMethods.SelectionChanged += lbMethods_SelectionChanged;  
        }

        private void lbMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spParameters.Children.Clear();
            selectedMethod = lbMethods.SelectedItem as MethodInfo;
            var parametersCount = selectedMethod.GetParameters().Count();
            lblResult.Content = "";

            if (selectedMethod.ReturnType == typeof(void)&& parametersCount==0)
                return;

            var parametersTypes = selectedMethod.GetParameters().Select(x=>x.ParameterType.Name).ToList();
            var parametersNames = selectedMethod.GetParameters().Select(x=>x.Name).ToList();
            
            Label label = new Label();
            label.VerticalAlignment = VerticalAlignment.Center;
            TextBox textBox = new TextBox();
            textBox.Height = 30;
            textBox.Width = 50;
            textBox.VerticalContentAlignment = VerticalAlignment.Center;

            for (int i = 0; i < parametersCount; i++)
            {
                label.Content = $"({parametersTypes[i]} { parametersNames[i]})";
                spParameters.Children.Add(label);
                spParameters.Children.Add(textBox);
            }
        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            var parametersCount = selectedMethod.GetParameters().Count();
            if (parametersCount == 0 && selectedMethod.ReturnType == typeof(void))
            {
                selectedMethod.Invoke(obj, null);
                return;
            }
            else if (parametersCount == 0 && selectedMethod.ReturnType != typeof(void))
            {
                object result = selectedMethod.Invoke(obj, null);
                if (result == null)
                {
                    lblResult.Content += "null";
                    return;
                }
                lblResult.Content += $"{result.ToString()}";
                return;
            }
            else
            {
                var panelUiElements = spParameters.Children;
                var textBoxes = GetTextBoxes(panelUiElements);
                foreach (var item in textBoxes)
                {
                    if (item.Text=="")
                    {
                        return;
                    }
                }
                var inputParameters = GetParameters(textBoxes);

                if (selectedMethod.ReturnType == typeof(void) && inputParameters != null)
                {
                    selectedMethod.Invoke(obj, inputParameters);
                    
                    lblResult.Content = $"{selectedMethod.Name} (";
                    foreach (var item in inputParameters)
                    {
                        if (inputParameters.Length>1)
                        {
                            lblResult.Content += $"{item.ToString()}, ";
                        }
                        else if (item == inputParameters[inputParameters.Length-1])
                        {
                        lblResult.Content += $"{item.ToString()})";
                        }
                    }
                    return;
                }

                object result = selectedMethod.Invoke(obj, inputParameters);
                if (result == null)
                {
                    lblResult.Content += ":Null";
                    return;
                }

                lblResult.Content = result.ToString();
            }
        }

        private object[] GetParameters(TextBox[] textBoxes)
        {
            var methodParameters = selectedMethod.GetParameters().ToArray();
            var inputValues = new List<object>();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType == typeof(int))
                {
                    if (int.TryParse(textBoxes[i].Text, out int temp))
                        inputValues.Add(temp);
                    else
                        inputValues.Add(0);
                }
                else
                {
                    inputValues.Add(textBoxes[i].Text);
                }
            }

            return inputValues.ToArray();
        }

        private TextBox[] GetTextBoxes(UIElementCollection panelElements)
        {
            var textBoxes = new List<TextBox>();

            foreach (var item in panelElements)
            {
                if (item is TextBox)
                    textBoxes.Add(item as TextBox);
            }

            return textBoxes.ToArray();
        }
    }
}
