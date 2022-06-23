using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WXTransmitterEmulator
{
    public partial class frmMap : Form
    {
        public delegate IReadOnlyCollection<Form1.Storm> GetStormsDelegate();
        Dictionary<int, Label> stormLabelsByID = new Dictionary<int, Label>();
        private GetStormsDelegate _getStormsCallback;
        decimal horizontalScale = 1;
        decimal verticalScale = 1;
        public frmMap()
        {
            InitializeComponent();
        }

        public void SetGetStormsCallback(GetStormsDelegate getStormsDelegate)
        {
            _getStormsCallback = getStormsDelegate;
        }

        private void frmMap_Shown(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void frmMap_SizeChanged(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void DrawGrid()
        {
            float horizontalIncrement = this.Width / 3;
            float verticalIncrement = this.Height / 3;

            Graphics g = CreateGraphics();
            g.Clear(BackColor);

            g.DrawLine(Pens.Black, horizontalIncrement, 0, horizontalIncrement, this.Height);
            g.DrawLine(Pens.Black, horizontalIncrement * 2, 0, horizontalIncrement * 2, this.Height);

            g.DrawLine(Pens.Black, 0, verticalIncrement, this.Width, verticalIncrement);
            g.DrawLine(Pens.Black, 0, verticalIncrement * 2, this.Width, verticalIncrement * 2);

            horizontalScale = (decimal)Width / 2000;
            verticalScale = (decimal)Height / 2000;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            foreach(Form1.Storm storm in _getStormsCallback())
            {
                if (!stormLabelsByID.ContainsKey(storm.ID))
                {
                    Label newStormLabel = new Label();
                    stormLabelsByID[storm.ID] = newStormLabel;
                    newStormLabel.AutoSize = true;
                    newStormLabel.BorderStyle = BorderStyle.FixedSingle;
                    Controls.Add(newStormLabel);
                }

                Label stormLabel = stormLabelsByID[storm.ID];
                if (stormLabel.Text != storm.DisplayName)
                {
                    stormLabel.Text = storm.DisplayName;
                }

                stormLabel.Location = new Point((int)((Width / 2) + storm.PosX * horizontalScale - (stormLabel.Width / 2)),
                                                (int)((Height / 2) + storm.PosZ * verticalScale - (stormLabel.Height / 2)));
            }
        }
    }
}
