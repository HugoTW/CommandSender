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

        string processPath;
        string targetPorcess = "Receiver"; // change target process name if needed

        public MainWindow()
        {
            InitializeComponent();
        }

 

        private void Command_Sent(object sender, RoutedEventArgs e)
        {


            if (IsProcessOpen( targetPorcess ))
            {

                //processPath = "Y:\\Project\\02_Win\\ColorCommand\\Receiver\\Receiver\\Receiver\\bin\\Debug\\Receiver.exe";
                //processPath = "mozbii";

                Debug.WriteLine("Moz -processPath:" + processPath);
                Debug.WriteLine("Moz - sender command:" + txtCommand.Text);
                System.Diagnostics.Process.Start(processPath, txtCommand.Text);

            }

        }

        public bool IsProcessOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    // Get process full path
                    Console.WriteLine(clsProcess.MainModule.FileName);

                     processPath = clsProcess.MainModule.FileName;

                    return true;
                }
            }

            return false;
        }


    }
}
