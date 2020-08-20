namespace LDC
{
    partial class MainPanel
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.ReadDevice = new System.Windows.Forms.Button();
            this.Devices = new System.Windows.Forms.ListView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Log_E = new System.Windows.Forms.TextBox();
            this.Log_I = new System.Windows.Forms.TextBox();
            this.Files = new System.Windows.Forms.ListView();
            this.ReadFiles = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ReadDevice
            // 
            this.ReadDevice.Location = new System.Drawing.Point(12, 513);
            this.ReadDevice.Name = "ReadDevice";
            this.ReadDevice.Size = new System.Drawing.Size(75, 43);
            this.ReadDevice.TabIndex = 3;
            this.ReadDevice.Text = "读取设备";
            this.ReadDevice.UseVisualStyleBackColor = true;
            this.ReadDevice.Click += new System.EventHandler(this.ReadDevice_Click);
            // 
            // Devices
            // 
            this.Devices.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Devices.FullRowSelect = true;
            this.Devices.GridLines = true;
            this.Devices.HideSelection = false;
            this.Devices.Location = new System.Drawing.Point(3, 3);
            this.Devices.Name = "Devices";
            this.Devices.Size = new System.Drawing.Size(715, 470);
            this.Devices.TabIndex = 2;
            this.Devices.UseCompatibleStateImageBehavior = false;
            this.Devices.View = System.Windows.Forms.View.Details;
            this.Devices.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Devices_MouseUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1180, 505);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Files);
            this.tabPage1.Controls.Add(this.Devices);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1172, 479);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "操作";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Log_E);
            this.tabPage2.Controls.Add(this.Log_I);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1172, 479);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "日志";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Log_E
            // 
            this.Log_E.Location = new System.Drawing.Point(582, 6);
            this.Log_E.Multiline = true;
            this.Log_E.Name = "Log_E";
            this.Log_E.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Log_E.Size = new System.Drawing.Size(584, 467);
            this.Log_E.TabIndex = 1;
            // 
            // Log_I
            // 
            this.Log_I.Location = new System.Drawing.Point(6, 6);
            this.Log_I.Multiline = true;
            this.Log_I.Name = "Log_I";
            this.Log_I.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Log_I.Size = new System.Drawing.Size(570, 467);
            this.Log_I.TabIndex = 0;
            // 
            // Files
            // 
            this.Files.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold);
            this.Files.FullRowSelect = true;
            this.Files.GridLines = true;
            this.Files.HideSelection = false;
            this.Files.Location = new System.Drawing.Point(724, 3);
            this.Files.Name = "Files";
            this.Files.Size = new System.Drawing.Size(441, 470);
            this.Files.TabIndex = 3;
            this.Files.UseCompatibleStateImageBehavior = false;
            this.Files.View = System.Windows.Forms.View.Details;
            this.Files.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Files_MouseUp);
            // 
            // ReadFiles
            // 
            this.ReadFiles.Location = new System.Drawing.Point(1097, 513);
            this.ReadFiles.Name = "ReadFiles";
            this.ReadFiles.Size = new System.Drawing.Size(75, 43);
            this.ReadFiles.TabIndex = 5;
            this.ReadFiles.Text = "读取号码";
            this.ReadFiles.UseVisualStyleBackColor = true;
            this.ReadFiles.Click += new System.EventHandler(this.ReadFiles_Click);
            // 
            // MainPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 561);
            this.Controls.Add(this.ReadFiles);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.ReadDevice);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainPanel";
            this.Text = "雷电群控";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ReadDevice;
        private System.Windows.Forms.ListView Devices;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox Log_I;
        private System.Windows.Forms.TextBox Log_E;
        private System.Windows.Forms.ListView Files;
        private System.Windows.Forms.Button ReadFiles;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}

