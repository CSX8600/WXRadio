using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WXRadio.WeatherManager.Product;
using WXRadio.WeatherManager;
using WXRadio.WeatherManager.Utility;
using System.Drawing.Drawing2D;

namespace AdvisoryDisplay
{
    public partial class frmMap : Form
    {
        double horizontalScale = 1;
        double verticalScale = 1;
        Dictionary<Storm, Label> labelsByStorm = new Dictionary<Storm, Label>();
        private int centerX;
        private int centerZ;

        public delegate void StormDelegate(Storm storm);

        public frmMap(int centerX, int centerZ) : this()
        {
            this.centerX = centerX;
            this.centerZ = centerZ;
        }

        protected frmMap()
        {
            InitializeComponent();
        }

        private void DrawGrid()
        {
            float horizontalIncrement = this.Width / 3;
            float verticalIncrement = this.Height / 3;

            Graphics g = CreateGraphics();
            g.Clear(BackColor);

            g.DrawLine(Pens.White, horizontalIncrement, 0, horizontalIncrement, this.Height);
            g.DrawLine(Pens.White, horizontalIncrement * 2, 0, horizontalIncrement * 2, this.Height);

            g.DrawLine(Pens.White, 0, verticalIncrement, this.Width, verticalIncrement);
            g.DrawLine(Pens.White, 0, verticalIncrement * 2, this.Width, verticalIncrement * 2);

            horizontalScale = Width / (double)2000;
            verticalScale = Height / (double)2000;

            foreach(BaseProduct product in ProductManager.INSTANCE.GetProducts())
            {
                Coordinate[] drawingCoords = product.GetPolygonCoordinates();

                if (drawingCoords.Length < 3)
                {
                    continue;
                }

                Brush fillBrush = null;
                Pen outlinePen = null;

                switch(product.GetType().Name)
                {
                    case "SevereThunderstormWatch":
                        outlinePen = new Pen(Color.Yellow, 5F);
                        break;
                    case "SevereThunderstormWarning":
                        outlinePen = new Pen(Color.Yellow, 5F);
                        fillBrush = new HatchBrush(HatchStyle.ForwardDiagonal, Color.Yellow);
                        break;
                    case "TornadoWatch":
                        outlinePen = new Pen(Color.Red, 5F);
                        break;
                    case "TornadoWarning":
                        outlinePen = new Pen(Color.Red, 5F);
                        fillBrush = new HatchBrush(HatchStyle.ForwardDiagonal, Color.Red);
                        break;
                }

                Point[] points = product.GetPolygonCoordinates().Select(c => new Point((int)((c.X + 1000 - centerX) * horizontalScale), (int)((c.Z + 1000 - centerZ) * verticalScale))).ToArray();
                if (fillBrush != null)
                {
                    g.FillPolygon(fillBrush, points);
                }

                if (outlinePen != null)
                {
                    g.DrawPolygon(outlinePen, points);
                }
            }
        }

        private void frmMap_Shown(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void frmMap_Resize(object sender, EventArgs e)
        {
            DrawGrid();
        }

        private void UpdateStorm(Storm storm)
        {
            if (storm.Strength == Storm.Strengths.Rain)
            {
                if (labelsByStorm.ContainsKey(storm))
                {
                    Controls.Remove(labelsByStorm[storm]);
                    labelsByStorm.Remove(storm);
                }

                return;
            }
            int mapX = (int)((storm.Coordinate.X + 1000 - centerX) * horizontalScale);
            int mapY = (int)((storm.Coordinate.Z + 1000 - centerZ) * verticalScale);

            Label label;
            if (!labelsByStorm.ContainsKey(storm))
            {
                label = new Label();
                label.Text = "Storm";
                label.AutoSize = true;
                label.BackColor = Color.Transparent;
                label.ForeColor = Color.White;
                Controls.Add(label);
                labelsByStorm[storm] = label;
            }

            label = labelsByStorm[storm];

            mapX -= label.Width / 2;
            mapY -= label.Height / 2;

            label.Location = new Point(mapX, mapY);

            DrawGrid();
        }

        private void DeleteStorm(Storm storm)
        {
            if (labelsByStorm.ContainsKey(storm))
            {
                Controls.Remove(labelsByStorm[storm]);
                labelsByStorm.Remove(storm);
            }
        }

        public StormDelegate GetUpdateStormDelegate()
        {
            return (storm) =>
            {
                if (IsHandleCreated)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        UpdateStorm(storm);
                    });
                }
            };
        }

        public StormDelegate GetDeleteStormDelegate()
        {
            return (storm) =>
            {
                if (IsHandleCreated)
                {
                    Invoke((MethodInvoker)delegate
                    {
                        DeleteStorm(storm);
                    });
                }
            };
        }
    }
}
