namespace ResWeb_Installer
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
            this.install = new System.Windows.Forms.Button();
            this.outputbox = new System.Windows.Forms.RichTextBox();
            this.start = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // install
            // 
            this.install.Location = new System.Drawing.Point(12, 12);
            this.install.Name = "install";
            this.install.Size = new System.Drawing.Size(118, 38);
            this.install.TabIndex = 0;
            this.install.Text = "Install Service";
            this.install.UseVisualStyleBackColor = true;
            this.install.Click += new System.EventHandler(this.install_Click);
            // 
            // outputbox
            // 
            this.outputbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputbox.Location = new System.Drawing.Point(12, 237);
            this.outputbox.Name = "outputbox";
            this.outputbox.Size = new System.Drawing.Size(360, 212);
            this.outputbox.TabIndex = 2;
            this.outputbox.Text = "";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(136, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(118, 38);
            this.start.TabIndex = 3;
            this.start.Text = "Start Service";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 461);
            this.Controls.Add(this.start);
            this.Controls.Add(this.outputbox);
            this.Controls.Add(this.install);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.Text = "ResWeb  Installer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button install;
        private System.Windows.Forms.RichTextBox outputbox;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Timer timer1;
    }
}

