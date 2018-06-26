using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CsharpAndNode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string PipeName = "salamander_pipe";

        private void StartListen()
        {
            while (true)
            {
                using (NamedPipeServerStream pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1))
                {
                    try
                    {
                        pipeServer.WaitForConnection();
                        pipeServer.ReadMode = PipeTransmissionMode.Byte;

                        using (StreamReader reader = new StreamReader(pipeServer))
                        {
                            string message = reader.ReadToEnd();

                            NodeMessage.Invoke(new EventHandler(delegate
                            {
                                NodeMessage.AppendText(message + "\n");
                            }));
                        }
                    }
                    catch (IOException ex)
                    {
                        MessageBox.Show("管道监听失败" + ex.Message);
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Factory.StartNew(StartListen);
        }
    }
}