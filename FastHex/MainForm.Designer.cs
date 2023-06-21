namespace FastHex
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            chkHashcat = new CheckBox();
            txtNumIn = new TextBox();
            splitPanelMain = new SplitContainer();
            splitContainer1 = new SplitContainer();
            txtHexIn = new TextBox();
            txtHexOut = new TextBox();
            splitContainer2 = new SplitContainer();
            txtNumOut = new TextBox();
            splitContainer3 = new SplitContainer();
            txtMmhIn = new TextBox();
            txtMmhOut = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitPanelMain).BeginInit();
            splitPanelMain.Panel1.SuspendLayout();
            splitPanelMain.Panel2.SuspendLayout();
            splitPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).BeginInit();
            splitContainer3.Panel1.SuspendLayout();
            splitContainer3.Panel2.SuspendLayout();
            splitContainer3.SuspendLayout();
            SuspendLayout();
            // 
            // chkHashcat
            // 
            chkHashcat.Location = new Point(333, 418);
            chkHashcat.Name = "chkHashcat";
            chkHashcat.Size = new Size(126, 24);
            chkHashcat.TabIndex = 0;
            chkHashcat.Text = "HashCat Output";
            // 
            // txtNumIn
            // 
            txtNumIn.Dock = DockStyle.Fill;
            txtNumIn.Location = new Point(0, 0);
            txtNumIn.Multiline = true;
            txtNumIn.Name = "txtNumIn";
            txtNumIn.Size = new Size(228, 200);
            txtNumIn.TabIndex = 1;
            txtNumIn.TextChanged += txtNumIn_TextChanged;
            // 
            // splitPanelMain
            // 
            splitPanelMain.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            splitPanelMain.Location = new Point(12, 12);
            splitPanelMain.Name = "splitPanelMain";
            // 
            // splitPanelMain.Panel1
            // 
            splitPanelMain.Panel1.Controls.Add(splitContainer1);
            // 
            // splitPanelMain.Panel2
            // 
            splitPanelMain.Panel2.Controls.Add(splitContainer2);
            splitPanelMain.Size = new Size(447, 400);
            splitPanelMain.SplitterDistance = 215;
            splitPanelMain.TabIndex = 2;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(txtHexIn);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(txtHexOut);
            splitContainer1.Size = new Size(215, 400);
            splitContainer1.SplitterDistance = 200;
            splitContainer1.TabIndex = 3;
            // 
            // txtHexIn
            // 
            txtHexIn.Dock = DockStyle.Fill;
            txtHexIn.Location = new Point(0, 0);
            txtHexIn.Multiline = true;
            txtHexIn.Name = "txtHexIn";
            txtHexIn.Size = new Size(215, 200);
            txtHexIn.TabIndex = 3;
            txtHexIn.TextChanged += txtHexIn_TextChanged;
            // 
            // txtHexOut
            // 
            txtHexOut.Dock = DockStyle.Fill;
            txtHexOut.Location = new Point(0, 0);
            txtHexOut.Multiline = true;
            txtHexOut.Name = "txtHexOut";
            txtHexOut.Size = new Size(215, 196);
            txtHexOut.TabIndex = 4;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(txtNumIn);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(txtNumOut);
            splitContainer2.Size = new Size(228, 400);
            splitContainer2.SplitterDistance = 200;
            splitContainer2.TabIndex = 4;
            // 
            // txtNumOut
            // 
            txtNumOut.Dock = DockStyle.Fill;
            txtNumOut.Location = new Point(0, 0);
            txtNumOut.Multiline = true;
            txtNumOut.Name = "txtNumOut";
            txtNumOut.Size = new Size(228, 196);
            txtNumOut.TabIndex = 2;
            // 
            // splitContainer3
            // 
            splitContainer3.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            splitContainer3.Location = new Point(465, 12);
            splitContainer3.Name = "splitContainer3";
            splitContainer3.Orientation = Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            splitContainer3.Panel1.Controls.Add(txtMmhIn);
            // 
            // splitContainer3.Panel2
            // 
            splitContainer3.Panel2.Controls.Add(txtMmhOut);
            splitContainer3.Size = new Size(396, 430);
            splitContainer3.SplitterDistance = 215;
            splitContainer3.TabIndex = 5;
            // 
            // txtMmhIn
            // 
            txtMmhIn.Dock = DockStyle.Fill;
            txtMmhIn.Location = new Point(0, 0);
            txtMmhIn.Multiline = true;
            txtMmhIn.Name = "txtMmhIn";
            txtMmhIn.Size = new Size(396, 215);
            txtMmhIn.TabIndex = 1;
            txtMmhIn.TextChanged += txtMmhIn_TextChanged;
            // 
            // txtMmhOut
            // 
            txtMmhOut.Dock = DockStyle.Fill;
            txtMmhOut.Location = new Point(0, 0);
            txtMmhOut.Multiline = true;
            txtMmhOut.Name = "txtMmhOut";
            txtMmhOut.Size = new Size(396, 211);
            txtMmhOut.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(873, 454);
            Controls.Add(chkHashcat);
            Controls.Add(splitContainer3);
            Controls.Add(splitPanelMain);
            Name = "MainForm";
            Text = "FastHex";
            splitPanelMain.Panel1.ResumeLayout(false);
            splitPanelMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitPanelMain).EndInit();
            splitPanelMain.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            splitContainer3.Panel1.ResumeLayout(false);
            splitContainer3.Panel1.PerformLayout();
            splitContainer3.Panel2.ResumeLayout(false);
            splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer3).EndInit();
            splitContainer3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtNumIn;
        private SplitContainer splitPanelMain;
        private SplitContainer splitContainer1;
        private TextBox txtHexIn;
        private TextBox txtHexOut;
        private SplitContainer splitContainer2;
        private TextBox txtNumOut;
        private SplitContainer splitContainer3;
        private TextBox txtMmhIn;
        private TextBox txtMmhOut;
        private CheckBox chkHashcat;
    }
}