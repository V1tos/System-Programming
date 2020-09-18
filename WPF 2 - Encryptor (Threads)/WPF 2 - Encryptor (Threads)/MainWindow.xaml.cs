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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_2___Encryptor__Threads_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string textFromFile;
        string textResult;
        string fileName;
        Thread cryptThread;
        Thread refreshThread;
        Crypt crypt;
        
        public enum Crypt
        {
            Encrypt,
            Decrypt
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "text files (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                string path = tbFileWay.Text = dialog.FileName;


                int deleteIndex = dialog.SafeFileName.IndexOf('.');
                fileName = dialog.SafeFileName.Remove(deleteIndex);

                using (FileStream fs = File.OpenRead(path))
                {
                    byte[] array = new byte[fs.Length];
                    fs.Read(array, 0, array.Length);
                    tbLoad.Text = textFromFile = Encoding.Default.GetString(array);
                }
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            crypt = SetTypeOfCrypt();
            string pbMax = textFromFile.Length.ToString();

            cryptThread = new Thread(CryptText);
            cryptThread.Start(textFromFile);
            refreshThread = new Thread(RefreshPB);
            refreshThread.Start(pbMax);
        }

        private Crypt SetTypeOfCrypt()
        {
            if (rbEncrypt.IsChecked == true)
                return Crypt.Encrypt;
            else
                return Crypt.Decrypt;
        }

        private void CryptText(object textFromFile)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] textToBytes = ascii.GetBytes(textFromFile.ToString());
            byte keyValue = 0;
            Dispatcher.Invoke(() => { keyValue = Byte.Parse(tbKey.Text); });

            for (int i = 0; i < textToBytes.Length; i++)
            {
                if (crypt==Crypt.Encrypt)
                    textToBytes[i] += keyValue;
                else
                    textToBytes[i] -= keyValue;
            }

            Dispatcher.Invoke(() =>
            {
                textResult = ascii.GetString(textToBytes);
            });
        }

        private void RefreshPB(object pbMax)
        {
            int max = int.Parse(pbMax.ToString());
            Dispatcher.Invoke(() => { pbCrypt.Minimum = 0; });
            Dispatcher.Invoke(() => { pbCrypt.Maximum = max; });
            for (int i = 0; i <= max; i++)
            {
                Dispatcher.Invoke(() => { pbCrypt.Value = i; });
                Thread.Sleep(10);
            }

            Dispatcher.Invoke(() =>
            {
                tbResult.Text = textResult;
            });
        }


        private void tbKey_MouseDown(object sender, MouseButtonEventArgs e)
        {
            tbKey.Clear();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Save(textResult);
        }

        private void Save(string textResult)
        {
            string path;
            if (crypt == Crypt.Encrypt)
                path = $@"..\EncryptFiles\{fileName}-encrypt.txt";
            else
                path = $@"..\DecryptFiles\{fileName}-decrypt.txt";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(textResult);
                fs.Write(array, 0, array.Length);
                if (crypt == Crypt.Encrypt)
                    MessageBox.Show("Saved", "Encrypt", MessageBoxButton.OK);
                else
                    MessageBox.Show("Saved", "Decrypt", MessageBoxButton.OK);
            }
        }
    }
}
