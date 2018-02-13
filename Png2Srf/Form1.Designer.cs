namespace Png2Srf
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.DropLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DropLabel
            // 
            this.DropLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DropLabel.ForeColor = System.Drawing.Color.White;
            this.DropLabel.Location = new System.Drawing.Point(0, 0);
            this.DropLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.DropLabel.Name = "DropLabel";
            this.DropLabel.Size = new System.Drawing.Size(284, 262);
            this.DropLabel.TabIndex = 1;
            this.DropLabel.Text = "Drop Input PNG Here!";
            this.DropLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.DropLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Png2Srf - © OfficerFlake 2015";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label DropLabel;

    }
}

