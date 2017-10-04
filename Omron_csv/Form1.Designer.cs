namespace Omron_csv
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txbFolder = new System.Windows.Forms.TextBox();
            this.txbTrickTimer = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.txbMsg = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 193);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Get Folder";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(19, 242);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Test API";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txbFolder
            // 
            this.txbFolder.Enabled = false;
            this.txbFolder.Location = new System.Drawing.Point(19, 165);
            this.txbFolder.Name = "txbFolder";
            this.txbFolder.Size = new System.Drawing.Size(367, 22);
            this.txbFolder.TabIndex = 4;
            // 
            // txbTrickTimer
            // 
            this.txbTrickTimer.Location = new System.Drawing.Point(132, 242);
            this.txbTrickTimer.Name = "txbTrickTimer";
            this.txbTrickTimer.Size = new System.Drawing.Size(75, 23);
            this.txbTrickTimer.TabIndex = 3;
            this.txbTrickTimer.Text = "Trick Timer";
            this.txbTrickTimer.UseVisualStyleBackColor = true;
            this.txbTrickTimer.Visible = false;
            this.txbTrickTimer.Click += new System.EventHandler(this.txbTrickTimer_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Location = new System.Drawing.Point(17, 150);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(44, 12);
            this.lbMsg.TabIndex = 8;
            this.lbMsg.Text = "Message";
            // 
            // txbMsg
            // 
            this.txbMsg.Location = new System.Drawing.Point(12, 12);
            this.txbMsg.Multiline = true;
            this.txbMsg.Name = "txbMsg";
            this.txbMsg.ReadOnly = true;
            this.txbMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbMsg.Size = new System.Drawing.Size(465, 135);
            this.txbMsg.TabIndex = 9;
            this.txbMsg.TextChanged += new System.EventHandler(this.txbMsg_TextChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(489, 273);
            this.Controls.Add(this.txbMsg);
            this.Controls.Add(this.lbMsg);
            this.Controls.Add(this.txbFolder);
            this.Controls.Add(this.txbTrickTimer);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Main";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txbFolder;
        private System.Windows.Forms.Button txbTrickTimer;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.TextBox txbMsg;
    }
}

