using System;
using System.Collections.Generic;
using System.Diagnostics;
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

using System.IO;
using System.IO.Pipes;
using System.Threading;
using System.IO.MemoryMappedFiles;
using System.Runtime.Serialization.Formatters.Binary;

namespace Sender
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const int MMF_MAX_SIZE = 1024;  // allocated memory for this memory mapped file (bytes)
        const int MMF_VIEW_SIZE = 1024; // how many bytes of the allocated memory can this process access

        // creates the memory mapped file which allows 'Reading' and 'Writing'
        MemoryMappedFile mmf;
        // creates a stream for this process, which allows it to write data from offset 0 to 1024 (whole memory)
        MemoryMappedViewStream mmvStream;

        string processPath;


        public MainWindow()
        {
            InitializeComponent();


            Thread t3 = new Thread(() => InitSharedMemory());
            t3.Start();
        }

 

        public void InitSharedMemory()
        {
            // creates the memory mapped file which allows 'Reading' and 'Writing'
            mmf = MemoryMappedFile.CreateOrOpen("mmf1", MMF_MAX_SIZE, MemoryMappedFileAccess.ReadWrite);
            // creates a stream for this process, which allows it to write data from offset 0 to 1024 (whole memory)
            mmvStream = mmf.CreateViewStream(0, MMF_VIEW_SIZE);

            UpdateSharedMemory();
         
        }

        public void UpdateSharedMemory()
        {
            // this is what we want to write to the memory mapped file
            String message1 = "";

            this.Dispatcher.Invoke(() =>
            {
                message1 = txtCommand.Text;
            });

            // serialize the variable 'message1' and write it to the memory mapped file
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(mmvStream, message1);
            mmvStream.Seek(0, SeekOrigin.Begin); // sets the current position back to the beginning of the stream
        }



        private void Command_Sent(object sender, RoutedEventArgs e)
        {

            UpdateSharedMemory();

            Debug.WriteLine("command:" + txtCommand.Text);

            //if (IsProcessOpen("Receiver"))
            //{
            //    //processPath = "C:\\SingletonApp.exe";
            //    System.Diagnostics.Process.Start(processPath, " -color MYINFO");

            //}


        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    // Get process full path
                    Console.WriteLine(clsProcess.MainModule.FileName);

                    if (name == "Receiver")
                    {
                        processPath = clsProcess.MainModule.FileName;

                    }
                    return true;
                }
            }

            return false;
        }


        public void TestSend()
        {
            Process[] NewProcessList2 =
                      Process.GetProcessesByName("Receiver");
            foreach (Process TempProcess in NewProcessList2)
            {
                // BUG :
                TempProcess.MainModule.GetType().GetMethod("TestFun").Invoke(TempProcess.MainModule, null);
            }
        }



    }
}
