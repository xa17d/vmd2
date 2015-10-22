namespace Vmd2.Presentation
{
    partial class FormMain
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
            this.pictureBoxDisplay = new System.Windows.Forms.PictureBox();
            this.scrollBarSlice = new System.Windows.Forms.HScrollBar();
            this.controlLog1 = new Vmd2.Presentation.ControlLog();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxDisplay
            // 
            this.pictureBoxDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxDisplay.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxDisplay.Name = "pictureBoxDisplay";
            this.pictureBoxDisplay.Size = new System.Drawing.Size(705, 499);
            this.pictureBoxDisplay.TabIndex = 0;
            this.pictureBoxDisplay.TabStop = false;
            // 
            // scrollBarSlice
            // 
            this.scrollBarSlice.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.scrollBarSlice.LargeChange = 1;
            this.scrollBarSlice.Location = new System.Drawing.Point(0, 499);
            this.scrollBarSlice.Name = "scrollBarSlice";
            this.scrollBarSlice.Size = new System.Drawing.Size(705, 17);
            this.scrollBarSlice.TabIndex = 1;
            this.scrollBarSlice.Scroll += new System.Windows.Forms.ScrollEventHandler(this.scrollBarSlice_Scroll);
            // 
            // controlLog1
            // 
            this.controlLog1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.controlLog1.Font = new System.Drawing.Font("Consolas", 10F);
            this.controlLog1.Location = new System.Drawing.Point(0, 516);
            this.controlLog1.Multiline = true;
            this.controlLog1.Name = "controlLog1";
            this.controlLog1.Size = new System.Drawing.Size(705, 121);
            this.controlLog1.TabIndex = 2;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 637);
            this.Controls.Add(this.pictureBoxDisplay);
            this.Controls.Add(this.scrollBarSlice);
            this.Controls.Add(this.controlLog1);
            this.Name = "FormMain";
            this.Text = "VMD2";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisplay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxDisplay;
        private System.Windows.Forms.HScrollBar scrollBarSlice;
        private ControlLog controlLog1;
    }
}

