using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_3___Synchronized_Threads
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string path = @"D:\FullText.txt";
        static string loadText="";
        static object locker = new object();
        Thread thread;
        List<Thread> threads = new List<Thread>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog dialog = new OpenFileDialog();
            
            //dialog.Filter = "text files (*.txt)|*.txt";
            //if (dialog.ShowDialog() == true)
            //{
            //    string path = tbFileWay.Text = dialog.FileName;


            //    int deleteIndex = dialog.SafeFileName.IndexOf('.');
            //    fileName = dialog.SafeFileName.Remove(deleteIndex);

            //    using (FileStream fs = File.OpenRead(path))
            //    {
            //        byte[] array = new byte[fs.Length];
            //        fs.Read(array, 0, array.Length);
            //        tbLoad.Text = textFromFile = Encoding.Default.GetString(array);
            //    }
            //}
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();
                
                if (result.ToString()=="OK" && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var filePathes = Directory.GetFiles(fbd.SelectedPath).Where(x=>x.Contains(".txt")||x.Contains(".cpp")||x.Contains(".cs"));
                    
                    lbFiles.ItemsSource = filePathes;
                }
            }
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in lbFiles.Items)
            {
                thread = new Thread(LoadContent);
                threads.Add(thread);
                thread.Start(item.ToString());
            }
          
            foreach (var item in threads)
            {
                item.Join();
            }

            tbTest.Text = loadText; 
            thread = new Thread(WriteContent);
            threads.Add(thread);
            thread.Start(loadText);
        }

        private static void WriteContent(object text)
        {
            lock (text)
            {
                using (StreamWriter fileStream = new StreamWriter(path, false, Encoding.Default))
                {
                    fileStream.WriteLine(text.ToString());
                }
            }
        }

        private static void LoadContent(object file)
        {
            lock (loadText)
            {
                var filePath = file.ToString();

                using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
                {
                    byte[] array = new byte[fileStream.Length];
                    fileStream.Read(array, 0, array.Length);
                    loadText += Encoding.Default.GetString(array);
                }
            }
        }
    }
}
