namespace WXRadio.WXSynthesizer
{
    partial class frmRadioInterface
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
            this.components = new System.ComponentModel.Container();
            this.lblMenu = new System.Windows.Forms.Label();
            this.lblSelect = new System.Windows.Forms.Label();
            this.lblUp = new System.Windows.Forms.Label();
            this.lblLeft = new System.Windows.Forms.Label();
            this.lblRight = new System.Windows.Forms.Label();
            this.lblDown = new System.Windows.Forms.Label();
            this.lblWeatherSnooze = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.cmdClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMenu
            // 
            this.lblMenu.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblMenu.BackColor = System.Drawing.Color.Transparent;
            this.lblMenu.Location = new System.Drawing.Point(80, 155);
            this.lblMenu.Name = "lblMenu";
            this.lblMenu.Size = new System.Drawing.Size(63, 19);
            this.lblMenu.TabIndex = 0;
            this.lblMenu.Click += new System.EventHandler(this.lblMenu_Click);
            // 
            // lblSelect
            // 
            this.lblSelect.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblSelect.BackColor = System.Drawing.Color.Transparent;
            this.lblSelect.Location = new System.Drawing.Point(80, 199);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(63, 19);
            this.lblSelect.TabIndex = 0;
            this.lblSelect.Click += new System.EventHandler(this.lblSelect_Click);
            // 
            // lblUp
            // 
            this.lblUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblUp.BackColor = System.Drawing.Color.Transparent;
            this.lblUp.Location = new System.Drawing.Point(237, 159);
            this.lblUp.Name = "lblUp";
            this.lblUp.Size = new System.Drawing.Size(47, 19);
            this.lblUp.TabIndex = 0;
            this.lblUp.Click += new System.EventHandler(this.lblUp_Click);
            // 
            // lblLeft
            // 
            this.lblLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblLeft.Location = new System.Drawing.Point(169, 178);
            this.lblLeft.Name = "lblLeft";
            this.lblLeft.Size = new System.Drawing.Size(47, 19);
            this.lblLeft.TabIndex = 0;
            this.lblLeft.Click += new System.EventHandler(this.lblLeft_Click);
            // 
            // lblRight
            // 
            this.lblRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblRight.BackColor = System.Drawing.Color.Transparent;
            this.lblRight.Location = new System.Drawing.Point(306, 178);
            this.lblRight.Name = "lblRight";
            this.lblRight.Size = new System.Drawing.Size(47, 19);
            this.lblRight.TabIndex = 0;
            this.lblRight.Click += new System.EventHandler(this.lblRight_Click);
            // 
            // lblDown
            // 
            this.lblDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblDown.BackColor = System.Drawing.Color.Transparent;
            this.lblDown.Location = new System.Drawing.Point(237, 196);
            this.lblDown.Name = "lblDown";
            this.lblDown.Size = new System.Drawing.Size(47, 19);
            this.lblDown.TabIndex = 0;
            this.lblDown.Click += new System.EventHandler(this.lblDown_Click);
            // 
            // lblWeatherSnooze
            // 
            this.lblWeatherSnooze.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblWeatherSnooze.BackColor = System.Drawing.Color.Transparent;
            this.lblWeatherSnooze.Location = new System.Drawing.Point(83, 228);
            this.lblWeatherSnooze.Name = "lblWeatherSnooze";
            this.lblWeatherSnooze.Size = new System.Drawing.Size(351, 47);
            this.lblWeatherSnooze.TabIndex = 0;
            this.lblWeatherSnooze.Click += new System.EventHandler(this.lblWeatherSnooze_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Font = new System.Drawing.Font("Digital-7", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.Location = new System.Drawing.Point(86, 60);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(349, 53);
            this.lblMessage.TabIndex = 1;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // cmdClose
            // 
            this.cmdClose.BackColor = System.Drawing.Color.White;
            this.cmdClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmdClose.Location = new System.Drawing.Point(485, 12);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(23, 23);
            this.cmdClose.TabIndex = 2;
            this.cmdClose.Text = "X";
            this.cmdClose.UseVisualStyleBackColor = false;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // frmRadioInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = global::WXRadio.WXSynthesizer.Properties.Resources.Radio;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(520, 447);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblDown);
            this.Controls.Add(this.lblWeatherSnooze);
            this.Controls.Add(this.lblRight);
            this.Controls.Add(this.lblLeft);
            this.Controls.Add(this.lblUp);
            this.Controls.Add(this.lblSelect);
            this.Controls.Add(this.lblMenu);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Name = "frmRadioInterface";
            this.Text = "WX Radio";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.frmRadioInterface_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.frmRadioInterface_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmRadioInterface_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMenu;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.Label lblUp;
        private System.Windows.Forms.Label lblLeft;
        private System.Windows.Forms.Label lblRight;
        private System.Windows.Forms.Label lblDown;
        private System.Windows.Forms.Label lblWeatherSnooze;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Button cmdClose;
    }
}