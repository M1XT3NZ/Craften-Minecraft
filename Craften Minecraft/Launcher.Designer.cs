namespace Craften_Minecraft
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.Versions = new System.Windows.Forms.ComboBox();
            this.Btn_Launch = new System.Windows.Forms.Button();
            this.MCPicture = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.MCPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // Versions
            // 
            this.Versions.FormattingEnabled = true;
            resources.ApplyResources(this.Versions, "Versions");
            this.Versions.Name = "Versions";
            // 
            // Btn_Launch
            // 
            resources.ApplyResources(this.Btn_Launch, "Btn_Launch");
            this.Btn_Launch.Name = "Btn_Launch";
            this.Btn_Launch.UseVisualStyleBackColor = true;
            this.Btn_Launch.Click += new System.EventHandler(this.Btn_Launch_Click);
            // 
            // MCPicture
            // 
            resources.ApplyResources(this.MCPicture, "MCPicture");
            this.MCPicture.Name = "MCPicture";
            this.MCPicture.TabStop = false;
            // 
            // Launcher
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Btn_Launch);
            this.Controls.Add(this.MCPicture);
            this.Controls.Add(this.Versions);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Launcher";
            this.Load += new System.EventHandler(this.Launcher_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MCPicture)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox Versions;
        private System.Windows.Forms.PictureBox MCPicture;
        private System.Windows.Forms.Button Btn_Launch;
    }
}