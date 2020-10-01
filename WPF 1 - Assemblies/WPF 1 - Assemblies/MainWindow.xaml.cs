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
            string path = $"{assemblyName}.{selectedClass}";
            var type = assembly.GetType(path);
            obj = Activator.CreateInstance(type);
            lblResult.Content = obj.ToString();
            var methods = type.GetMethods();
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
            FillStackPanel(spParameters);
        }

        private void FillStackPanel(StackPanel spParameters)
        {
            var methodParameters = selectedMethod.GetParameters();

            foreach (var item in methodParameters)
            {
                Label label = new Label();
                TextBox textBox = new TextBox();

                label.VerticalAlignment = VerticalAlignment.Center;
                label.Content = $"({item.ParameterType.Name} {item.Name})";
                textBox.Height = 30;
                textBox.Width = 60;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;

                spParameters.Children.Add(label);
                spParameters.Children.Add(textBox);
            }

        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            var parametersCount = selectedMethod.GetParameters().Count();

            if (parametersCount == 0)
            {
                InvokeWithoutParams(selectedMethod);
            }
            else
            {              
                var inputParameters = GetParametersFromPanel(spParameters.Children);
                InvokeWithParams(selectedMethod, inputParameters);
            }
        }

        private void InvokeWithoutParams(MethodInfo selectedMethod)
        {
            lblResult.Content = $"{ selectedMethod.Name}()";
            if (selectedMethod.ReturnType == typeof(void))
            {
                selectedMethod.Invoke(obj, null);
                return;
            }

            object result = selectedMethod.Invoke(obj, null);
            if (result == null)
            {
                lblResult.Content += $" = null";
                return;
            }
            lblResult.Content += $" = {result.ToString()}";
        }

        private void InvokeWithParams(MethodInfo selectedMethod, object[] inputParameters)
        {
            lblResult.Content = $"{selectedMethod.Name} (";
            foreach (var item in inputParameters)
            {
                if (inputParameters.Length > 1)
                {
                    lblResult.Content += $"{item.ToString()}, ";
                }
                else if (item == inputParameters[inputParameters.Length - 1])
                {
                    lblResult.Content += $"{item.ToString()})";
                }
            }

            if (selectedMethod.ReturnType == typeof(void))
            {
                selectedMethod.Invoke(obj, inputParameters);
            }
            else
            {
                object result = selectedMethod.Invoke(obj, inputParameters);
                if (result == null)
                {
                    lblResult.Content += " = null";
                    return;
                }
                lblResult.Content += $" = {result.ToString()}";
            }        
        }


        private object[] GetParametersFromPanel(UIElementCollection panelElements)
        {
            var textBoxes = GetTextBoxes(panelElements);
            return GetValuesFromTextBoxes(textBoxes);
        }

        private TextBox[] GetTextBoxes(UIElementCollection panelElements)
        {
            var textBoxes = new List<TextBox>();

            foreach (var item in panelElements)
            {
                if (item is TextBox)
                    textBoxes.Add(item as TextBox);
            }

            foreach (var item in textBoxes)
            {
                if (item.Text == "")
                    item.Text = "null";
            }

            return textBoxes.ToArray();
        }

        private object[] GetValuesFromTextBoxes(TextBox[] textBoxes)
        {
            var methodParameters = selectedMethod.GetParameters();
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
    }
}
