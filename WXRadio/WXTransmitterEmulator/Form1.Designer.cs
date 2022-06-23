namespace WXTransmitterEmulator
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.frmMap = new System.Windows.Forms.Button();
            this.cmdAddStorm = new System.Windows.Forms.Button();
            this.lstStorms = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.txtSpeed = new System.Windows.Forms.NumericUpDown();
            this.txtPosZ = new System.Windows.Forms.NumericUpDown();
            this.txtMovementZ = new System.Windows.Forms.NumericUpDown();
            this.txtMovementX = new System.Windows.Forms.NumericUpDown();
            this.txtStrength = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPosX = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cboStrength = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmdStartStopStreaming = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.tmrMovement = new System.Windows.Forms.Timer(this.components);
            this.tmrDataBurst = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMovementZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMovementX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStrength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosX)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.frmMap);
            this.groupBox1.Controls.Add(this.cmdAddStorm);
            this.groupBox1.Controls.Add(this.lstStorms);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 259);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Storms";
            // 
            // frmMap
            // 
            this.frmMap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frmMap.Location = new System.Drawing.Point(6, 201);
            this.frmMap.Name = "frmMap";
            this.frmMap.Size = new System.Drawing.Size(226, 23);
            this.frmMap.TabIndex = 1;
            this.frmMap.Text = "Map";
            this.frmMap.UseVisualStyleBackColor = true;
            this.frmMap.Click += new System.EventHandler(this.frmMap_Click);
            // 
            // cmdAddStorm
            // 
            this.cmdAddStorm.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAddStorm.Location = new System.Drawing.Point(6, 230);
            this.cmdAddStorm.Name = "cmdAddStorm";
            this.cmdAddStorm.Size = new System.Drawing.Size(226, 23);
            this.cmdAddStorm.TabIndex = 2;
            this.cmdAddStorm.Text = "Add";
            this.cmdAddStorm.UseVisualStyleBackColor = true;
            this.cmdAddStorm.Click += new System.EventHandler(this.cmdAddStorm_Click);
            // 
            // lstStorms
            // 
            this.lstStorms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstStorms.FormattingEnabled = true;
            this.lstStorms.Location = new System.Drawing.Point(6, 19);
            this.lstStorms.Name = "lstStorms";
            this.lstStorms.Size = new System.Drawing.Size(226, 173);
            this.lstStorms.TabIndex = 0;
            this.lstStorms.SelectedIndexChanged += new System.EventHandler(this.lstStorms_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.cmdSave);
            this.groupBox2.Controls.Add(this.txtSpeed);
            this.groupBox2.Controls.Add(this.txtPosZ);
            this.groupBox2.Controls.Add(this.txtMovementZ);
            this.groupBox2.Controls.Add(this.txtMovementX);
            this.groupBox2.Controls.Add(this.txtStrength);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtPosX);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.cboStrength);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtDisplayName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(256, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(351, 259);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Storm Details";
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Location = new System.Drawing.Point(270, 230);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 8;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // txtSpeed
            // 
            this.txtSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSpeed.DecimalPlaces = 2;
            this.txtSpeed.Location = new System.Drawing.Point(87, 202);
            this.txtSpeed.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(258, 20);
            this.txtSpeed.TabIndex = 7;
            // 
            // txtPosZ
            // 
            this.txtPosZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPosZ.DecimalPlaces = 2;
            this.txtPosZ.Location = new System.Drawing.Point(87, 124);
            this.txtPosZ.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.txtPosZ.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.txtPosZ.Name = "txtPosZ";
            this.txtPosZ.Size = new System.Drawing.Size(258, 20);
            this.txtPosZ.TabIndex = 4;
            // 
            // txtMovementZ
            // 
            this.txtMovementZ.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMovementZ.DecimalPlaces = 2;
            this.txtMovementZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.txtMovementZ.Location = new System.Drawing.Point(87, 176);
            this.txtMovementZ.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMovementZ.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.txtMovementZ.Name = "txtMovementZ";
            this.txtMovementZ.Size = new System.Drawing.Size(258, 20);
            this.txtMovementZ.TabIndex = 6;
            // 
            // txtMovementX
            // 
            this.txtMovementX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMovementX.DecimalPlaces = 2;
            this.txtMovementX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.txtMovementX.Location = new System.Drawing.Point(87, 150);
            this.txtMovementX.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtMovementX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.txtMovementX.Name = "txtMovementX";
            this.txtMovementX.Size = new System.Drawing.Size(258, 20);
            this.txtMovementX.TabIndex = 5;
            // 
            // txtStrength
            // 
            this.txtStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStrength.Location = new System.Drawing.Point(87, 72);
            this.txtStrength.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.txtStrength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtStrength.Name = "txtStrength";
            this.txtStrength.Size = new System.Drawing.Size(258, 20);
            this.txtStrength.TabIndex = 2;
            this.txtStrength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtStrength.ValueChanged += new System.EventHandler(this.txtStrength_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 204);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Speed/t:";
            // 
            // txtPosX
            // 
            this.txtPosX.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPosX.DecimalPlaces = 2;
            this.txtPosX.Location = new System.Drawing.Point(87, 98);
            this.txtPosX.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.txtPosX.Minimum = new decimal(new int[] {
            999999,
            0,
            0,
            -2147483648});
            this.txtPosX.Name = "txtPosX";
            this.txtPosX.Size = new System.Drawing.Size(258, 20);
            this.txtPosX.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 178);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Motion Z:";
            // 
            // cboStrength
            // 
            this.cboStrength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboStrength.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStrength.FormattingEnabled = true;
            this.cboStrength.Items.AddRange(new object[] {
            "Rain",
            "Thunderstorm",
            "Severe Thunderstorm",
            "Severe Thunderstorm w/ Hail",
            "Tornado F0",
            "Tornado F1",
            "Tornado F2",
            "Tornado F3",
            "Tornado F4",
            "Tornado F5"});
            this.cboStrength.Location = new System.Drawing.Point(87, 45);
            this.cboStrength.Name = "cboStrength";
            this.cboStrength.Size = new System.Drawing.Size(258, 21);
            this.cboStrength.TabIndex = 1;
            this.cboStrength.SelectedIndexChanged += new System.EventHandler(this.cboStrength_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 152);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Motion X:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Pos Z:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Pos X:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Strength:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.Location = new System.Drawing.Point(87, 19);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(258, 20);
            this.txtDisplayName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Display Name:";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.cmdStartStopStreaming);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtPort);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtAddress);
            this.groupBox3.Location = new System.Drawing.Point(613, 12);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(175, 259);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Streaming Details";
            // 
            // cmdStartStopStreaming
            // 
            this.cmdStartStopStreaming.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStartStopStreaming.Location = new System.Drawing.Point(6, 97);
            this.cmdStartStopStreaming.Name = "cmdStartStopStreaming";
            this.cmdStartStopStreaming.Size = new System.Drawing.Size(163, 23);
            this.cmdStartStopStreaming.TabIndex = 2;
            this.cmdStartStopStreaming.Text = "Start Streaming";
            this.cmdStartStopStreaming.UseVisualStyleBackColor = true;
            this.cmdStartStopStreaming.Click += new System.EventHandler(this.cmdStartStopStreaming_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Streaming Port";
            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(6, 71);
            this.txtPort.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(163, 20);
            this.txtPort.TabIndex = 1;
            this.txtPort.Value = new decimal(new int[] {
            25566,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Streaming Address";
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(6, 32);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(163, 20);
            this.txtAddress.TabIndex = 0;
            this.txtAddress.Text = "127.0.0.1";
            // 
            // tmrMovement
            // 
            this.tmrMovement.Enabled = true;
            this.tmrMovement.Interval = 50;
            this.tmrMovement.Tick += new System.EventHandler(this.tmrMovement_Tick);
            // 
            // tmrDataBurst
            // 
            this.tmrDataBurst.Enabled = true;
            this.tmrDataBurst.Interval = 1000;
            this.tmrDataBurst.Tick += new System.EventHandler(this.tmrDataBurst_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 283);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stream Emulator";
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMovementZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMovementX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtStrength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPosX)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdAddStorm;
        private System.Windows.Forms.ListBox lstStorms;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cboStrength;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtPosZ;
        private System.Windows.Forms.NumericUpDown txtPosX;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button cmdStartStopStreaming;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown txtPort;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.NumericUpDown txtStrength;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button frmMap;
        private System.Windows.Forms.NumericUpDown txtSpeed;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown txtMovementZ;
        private System.Windows.Forms.NumericUpDown txtMovementX;
        private System.Windows.Forms.Timer tmrMovement;
        private System.Windows.Forms.Timer tmrDataBurst;
    }
}

