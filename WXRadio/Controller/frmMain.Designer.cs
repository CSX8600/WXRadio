namespace WXRadio.WXController
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tblPlugins = new System.Windows.Forms.TableLayoutPanel();
            this.cmdExit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tblPlugins
            // 
            this.tblPlugins.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tblPlugins.AutoScroll = true;
            this.tblPlugins.ColumnCount = 1;
            this.tblPlugins.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblPlugins.Location = new System.Drawing.Point(0, 0);
            this.tblPlugins.Name = "tblPlugins";
            this.tblPlugins.RowCount = 1;
            this.tblPlugins.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblPlugins.Size = new System.Drawing.Size(318, 398);
            this.tblPlugins.TabIndex = 0;
            // 
            // cmdExit
            // 
            this.cmdExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdExit.Location = new System.Drawing.Point(324, 12);
            this.cmdExit.Name = "cmdExit";
            this.cmdExit.Size = new System.Drawing.Size(75, 23);
            this.cmdExit.TabIndex = 1;
            this.cmdExit.Text = "Exit";
            this.cmdExit.UseVisualStyleBackColor = true;
            this.cmdExit.Click += new System.EventHandler(this.cmdExit_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(411, 399);
            this.Controls.Add(this.cmdExit);
            this.Controls.Add(this.tblPlugins);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WX Radio Receiver";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tblPlugins;
        private System.Windows.Forms.Button cmdExit;
    }
}

