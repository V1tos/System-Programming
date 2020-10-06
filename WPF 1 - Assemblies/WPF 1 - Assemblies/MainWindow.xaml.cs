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
                lbAssemblies.ItemsSource = classes.Select(x => x.Name);
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
            var methods = type.GetMethods();

            lblResult.Content = obj.ToString();
         
            foreach (var item in methods)
            {
                lbMethods.Items.Add(item);
            }

            lbMethods.SelectionChanged += lbMethods_SelectionChanged;
        }

        private void lbMethods_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            spParameters.Children.Clear();
            btnRunMethod.IsEnabled = true;

            selectedMethod = lbMethods.SelectedItem as MethodInfo;
            var methodParameters = selectedMethod.GetParameters();

            lblResult.Content = "";

            if (selectedMethod.ReturnType == typeof(void) && methodParameters == null)
                return;
            UpdatePanel(spParameters, methodParameters);
        }

        private void UpdatePanel(Panel panel, params ParameterInfo[] parameters)
        {
            foreach (var parameter in parameters)
            {
                Label label = new Label();
                TextBox textBox = new TextBox();

                label.VerticalAlignment = VerticalAlignment.Center;
                label.Content = $"({parameter.ParameterType.Name} {parameter.Name})";
                textBox.Height = 30;
                textBox.Width = 60;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;

                panel.Children.Add(label);
                panel.Children.Add(textBox);
            }

        }

        private void btnRun_Click(object sender, RoutedEventArgs e)
        {
            object[] inputedParameters = null;

            if (spParameters.Children.Count > 0)
            {
                var parameterTextBoxes = SelectTextBoxesFromPanel(spParameters.Children);
                var tbValues = SelectValuesFromTextBoxes(parameterTextBoxes);
                inputedParameters = GetParsedParameters(tbValues, selectedMethod);
            }

            InvokeMethod(selectedMethod, inputedParameters);
            (sender as Button).IsEnabled = false;
        }

        private void InvokeMethod(MethodInfo method, object[] parameters)
        {
            object result = method.Invoke(obj, parameters);
            lblResult.Content = MethodResult(method, parameters, result).ToString();
        }

        private object[] GetParsedParameters(object[] tbValues, MethodInfo method)
        {
            var methodParameters = method.GetParameters();
            var inputValues = new List<object>();

            for (int i = 0; i < methodParameters.Length; i++)
            {
                if (methodParameters[i].ParameterType == typeof(int))
                {
                    if (int.TryParse(tbValues[i].ToString(), out int temp))
                        inputValues.Add(temp);
                    else
                        inputValues.Add(0);
                }
                else
                {
                    inputValues.Add(tbValues[i].ToString());
                }
            }
            return inputValues.ToArray();
        }



        private object MethodResult(MethodInfo method, object[] parameters, object result)
        {
            string methodDescription = $"{method.Name} (";

            if (parameters == null && result == null)
            {
                if (method.ReturnType == typeof(void))
                    methodDescription += ") executed";
                else if (method.ReturnType != typeof(void) && parameters == null)
                    methodDescription += ") = null";
            }
            else if (parameters == null && result != null)
            {
                methodDescription += $") = {result.ToString()}";
            }
            else
            {
                foreach (var item in parameters)
                {
                    if (item != parameters[parameters.Length - 1])
                    {
                        methodDescription += $"{item.ToString()}, ";
                        continue;
                    }
                    methodDescription += $"{item.ToString()})";
                }
                if (result == null)
                {
                    if (method.ReturnType == typeof(void))
                        methodDescription += " executed";
                    else
                        methodDescription += " = null";
                }
                else
                {
                    methodDescription += $" = {result.ToString()}";
                }
            }
            return methodDescription as object;
        }

        private TextBox[] SelectTextBoxesFromPanel(UIElementCollection panelElements)
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

        private object[] SelectValuesFromTextBoxes(TextBox[] textBoxes)
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
        private object[] SelectValueFromTextBoxes1(TextBox[] textBoxes)
        {
            return (textBoxes.Select(x => x.Text) as object[]);
        }
    }
}
