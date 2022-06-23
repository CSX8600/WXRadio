namespace AdvisoryDisplay
{
    partial class AdvisoryDisplayControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.treAdvisories = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuMap = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // treAdvisories
            // 
            this.treAdvisories.ContextMenuStrip = this.contextMenuStrip1;
            this.treAdvisories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treAdvisories.Location = new System.Drawing.Point(0, 0);
            this.treAdvisories.Name = "treAdvisories";
            this.treAdvisories.Size = new System.Drawing.Size(271, 160);
            this.treAdvisories.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuMap});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(181, 48);
            // 
            // mnuMap
            // 
            this.mnuMap.Name = "mnuMap";
            this.mnuMap.Size = new System.Drawing.Size(180, 22);
            this.mnuMap.Text = "View Map";
            this.mnuMap.Click += new System.EventHandler(this.mnuMap_Click);
            // 
            // AdvisoryDisplayControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treAdvisories);
            this.Name = "AdvisoryDisplayControl";
            this.Size = new System.Drawing.Size(271, 160);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treAdvisories;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnuMap;
    }
}
