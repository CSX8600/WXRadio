using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WXTransmitterEmulator
{
    public partial class Form1 : Form
    {
        private bool isStreaming = false;
        private Thread streamThread = null;
        TcpListener listener;
        public Form1()
        {
            InitializeComponent();
        }

        public class Storm
        {
            public static int StormCount = 0;
            public int ID { get; set; }
            public string DisplayName { get; set; }
            public int Strength { get; set; } = 1;
            public decimal PosX { get; set; }
            public decimal PosZ { get; set; }
            public decimal MovementX { get; set; }
            public decimal MovementZ { get; set; }
            public decimal Speed { get; set; }

            public override string ToString()
            {
                return DisplayName;
            }
        }

        private void cboStrength_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtStrength.Value = cboStrength.SelectedIndex + 1;
        }

        private void txtStrength_ValueChanged(object sender, EventArgs e)
        {
            cboStrength.SelectedIndex = (int)txtStrength.Value - 1;
        }

        private void cmdAddStorm_Click(object sender, EventArgs e)
        {
            Storm.StormCount++;
            Storm storm = new Storm();
            storm.ID = Storm.StormCount;
            storm.DisplayName = "storm" + Storm.StormCount;
            lstStorms.Items.Add(storm);
            lstStorms.SelectedItem = storm;

            SendString(JsonConvert.SerializeObject(new
            {
                @event = "new",
                id = storm.ID,
                strength = storm.Strength,
                posx = storm.PosX,
                posz = storm.PosZ
            }));
        }

        private void lstStorms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstStorms.SelectedItem == null)
            {
                return;
            }

            Storm storm = (Storm)lstStorms.SelectedItem;
            txtDisplayName.Text = storm.DisplayName;
            txtStrength.Value = storm.Strength;
            txtPosX.Value = storm.PosX;
            txtPosZ.Value = storm.PosZ;
            txtMovementX.Value = storm.MovementX;
            txtMovementZ.Value = storm.MovementZ;
            txtSpeed.Value = storm.Speed;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Storm storm = (Storm)lstStorms.SelectedItem;
            storm.DisplayName = txtDisplayName.Text;
            storm.Strength = (int)txtStrength.Value;
            storm.PosX = txtPosX.Value;
            storm.PosZ = txtPosZ.Value;
            storm.MovementX = txtMovementX.Value;
            storm.MovementZ = txtMovementZ.Value;
            storm.Speed = txtSpeed.Value;

            int currentIndex = lstStorms.SelectedIndex;
            lstStorms.Items.Remove(storm);
            lstStorms.Items.Insert(currentIndex, storm);
            lstStorms.SelectedIndex = currentIndex;
        }

        private void cmdStartStopStreaming_Click(object sender, EventArgs e)
        {
            cmdStartStopStreaming.Enabled = false;
            txtAddress.Enabled = false;
            txtPort.Enabled = false;

            if (isStreaming)
            {
                if (streamThread != null && streamThread.ThreadState == ThreadState.Running)
                {
                    listener.Stop();
                }

                cmdStartStopStreaming.Enabled = true;
                txtAddress.Enabled = true;
                txtPort.Enabled = true;

                cmdStartStopStreaming.Text = "Start Streaming";
            }
            else
            {
                listener = new TcpListener(IPAddress.Parse(txtAddress.Text), (int)txtPort.Value);
                streamThread = new Thread(new ThreadStart(() => Stream(listener)));
                streamThread.Start();

                cmdStartStopStreaming.Enabled = true;
                cmdStartStopStreaming.Text = "Stop Streaming";
            }

            isStreaming = !isStreaming;
        }

        private HashSet<TcpClient> tcpClients = new HashSet<TcpClient>();
        private object clientLock = new object();
        private void Stream(TcpListener listener)
        {
            try
            {
                while (true)
                {
                    listener.Start();
                    TcpClient client = listener.AcceptTcpClient();

                    var initialMessage = new
                    {
                        @event = "initial",
                        storms = lstStorms.Items.Cast<Storm>()
                    };
                    string stormMessage = JsonConvert.SerializeObject(initialMessage);
                    byte[] stormMessageBytes = Encoding.UTF8.GetBytes(stormMessage);
                    int stormMessageByteCount = stormMessageBytes.Length;
                    string stormMessageByteCountString = stormMessageByteCount.ToString().PadLeft(8, '0');
                    byte[] stormMessageByteCountStringBytes = Encoding.UTF8.GetBytes(stormMessageByteCountString);

                    client.GetStream().Write(stormMessageByteCountStringBytes, 0, stormMessageByteCountStringBytes.Length);
                    client.GetStream().Write(stormMessageBytes, 0, stormMessageByteCount);

                    lock (clientLock)
                    {
                        tcpClients.Add(client);
                    }
                }
            }
            catch (SocketException)
            {
                // We gucci
            }
            finally
            {
                lock (clientLock)
                {
                    foreach(TcpClient client in tcpClients)
                    {
                        client.Close();
                    }

                    tcpClients.Clear();
                }
            }
        }

        private void SendString(string json)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            int byteCount = bytes.Length;
            string byteCountString = byteCount.ToString().PadLeft(8, '0');
            byte[] byteCountStringBytes = Encoding.UTF8.GetBytes(byteCountString);
            
            lock(clientLock)
            {
                foreach(TcpClient client in tcpClients)
                {
                    client.GetStream().Write(byteCountStringBytes, 0, byteCountStringBytes.Length);
                    client.GetStream().Write(bytes, 0, byteCount);
                }
            }
        }

        private void tmrMovement_Tick(object sender, EventArgs e)
        {
            foreach(Storm storm in lstStorms.Items.Cast<Storm>())
            {
                storm.PosX += storm.MovementX * storm.Speed;
                storm.PosZ += storm.MovementZ * storm.Speed;
            }

            if (lstStorms.SelectedItem == null)
            {
                return;
            }

            Storm currentStorm = (Storm)lstStorms.SelectedItem;
            if (currentStorm.Speed == 0)
            {
                return;
            }

            txtPosX.Value = currentStorm.PosX;
            txtPosZ.Value = currentStorm.PosZ;
        }

        private void frmMap_Click(object sender, EventArgs e)
        {
            frmMap map = new frmMap();
            map.SetGetStormsCallback(new frmMap.GetStormsDelegate(() =>
            {
                return new List<Storm>(lstStorms.Items.Cast<Storm>());
            }));
            map.Show();
        }

        private void tmrDataBurst_Tick(object sender, EventArgs e)
        {
            SendString(JsonConvert.SerializeObject(new
            {
                @event = "update",
                storms = lstStorms.Items.Cast<Storm>()
            }));
        }
    }
}
