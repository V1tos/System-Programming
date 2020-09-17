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
        Thread encryptThread;
        Thread decryptThread;
        Thread refreshThread;
        int crypt;
        
        public enum Crypt
        {
            Encrypt,
            Decrypt
        }
        public MainWindow()
        {
            InitializeComponent();
            crypt = 0;
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
            switch (crypt)
            {
                case (int)Crypt.Encrypt:
                    encryptThread = new Thread(EncryptText);
                    encryptThread.Start(textFromFile);
                    refreshThread = new Thread(RefreshPB);
                    refreshThread.Start(pbMax);
                    break;
                case (int)Crypt.Decrypt:
                    decryptThread = new Thread(DecryptText);
                    decryptThread.Start(textFromFile);
                    refreshThread = new Thread(RefreshPB);
                    refreshThread.Start(pbMax);
                    break;
                default:
                    break;
            }
        }

        private int SetTypeOfCrypt()
        {
            int crypt = 0;
            if (rbEncrypt.IsChecked == true)
                crypt = (int)Crypt.Encrypt;
            else if (rbDecrypt.IsChecked == true)
                crypt = (int)Crypt.Decrypt;

            return crypt;
        }

        private void EncryptText(object textFromFile)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] textToBytes = ascii.GetBytes(textFromFile.ToString());
            byte keyValue = 0;
            Dispatcher.Invoke(() => { keyValue = Byte.Parse(tbKey.Text); });

            for (int i = 0; i < textToBytes.Length; i++)
            {
                textToBytes[i] += keyValue;
            }

            Dispatcher.Invoke(() =>
            {
                textResult = ascii.GetString(textToBytes);
            });
        }

        private void DecryptText(object textFromFile)
        {
            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] textToBytes = ascii.GetBytes(textFromFile.ToString());
            byte keyValue = 0;
            Dispatcher.Invoke(() => { keyValue = Byte.Parse(tbKey.Text); });

            for (int i = 0; i < textToBytes.Length; i++)
            {
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
            switch (crypt)
            {
                case (int)Crypt.Encrypt:
                    SaveEncryptFile(textResult);
                    break;
                case (int)Crypt.Decrypt:
                    SaveDecryptFile(textResult);
                    break;
                default:
                    break;
            }
        }


        private void SaveEncryptFile(string textResult)
        {
            string path = $@"..\EncryptFiles\{fileName}-encrypt.txt";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(textResult);
                fs.Write(array, 0, array.Length);
                MessageBox.Show("Saved", "Encrypt", MessageBoxButton.OK);
            }

        }

        private void SaveDecryptFile(string textResult)
        {
            string path = $@"..\DecryptFiles\{fileName}-decrypt.txt";

            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                byte[] array = Encoding.Default.GetBytes(textResult);
                fs.Write(array, 0, array.Length);
                MessageBox.Show("Saved", "Decrypt", MessageBoxButton.OK);
            }

        }
    }
}
