namespace Omron_csv
{
    partial class Form2
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.btnAPI = new System.Windows.Forms.Button();
            this.txbApiUrl = new System.Windows.Forms.TextBox();
            this.fbdCsv = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFolder = new System.Windows.Forms.Button();
            this.lbMsg = new System.Windows.Forms.Label();
            this.txbFileDetail = new System.Windows.Forms.TextBox();
            this.lbMachine = new System.Windows.Forms.Label();
            this.lbBoardID = new System.Windows.Forms.Label();
            this.lbFolder = new System.Windows.Forms.Label();
            this.cbMachineName = new System.Windows.Forms.ComboBox();
            this.cbBoardID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txbInspectionID = new System.Windows.Forms.TextBox();
            this.txbComponetID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbApi = new System.Windows.Forms.ComboBox();
            this.txbProcessTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.textBox1.Location = new System.Drawing.Point(4, 278);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(540, 183);
            this.textBox1.TabIndex = 1;
            // 
            // btnAPI
            // 
            this.btnAPI.Location = new System.Drawing.Point(431, 172);
            this.btnAPI.Name = "btnAPI";
            this.btnAPI.Size = new System.Drawing.Size(113, 23);
            this.btnAPI.TabIndex = 0;
            this.btnAPI.Text = "test API";
            this.btnAPI.UseVisualStyleBackColor = true;
            this.btnAPI.Click += new System.EventHandler(this.button1_Click);
            // 
            // txbApiUrl
            // 
            this.txbApiUrl.Location = new System.Drawing.Point(4, 250);
            this.txbApiUrl.Name = "txbApiUrl";
            this.txbApiUrl.Size = new System.Drawing.Size(540, 22);
            this.txbApiUrl.TabIndex = 2;
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(13, 13);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(125, 23);
            this.btnFolder.TabIndex = 3;
            this.btnFolder.Text = "取得CSV資料夾";
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // lbMsg
            // 
            this.lbMsg.AutoSize = true;
            this.lbMsg.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbMsg.Location = new System.Drawing.Point(161, 15);
            this.lbMsg.Name = "lbMsg";
            this.lbMsg.Size = new System.Drawing.Size(0, 16);
            this.lbMsg.TabIndex = 4;
            // 
            // txbFileDetail
            // 
            this.txbFileDetail.Location = new System.Drawing.Point(260, 39);
            this.txbFileDetail.Multiline = true;
            this.txbFileDetail.Name = "txbFileDetail";
            this.txbFileDetail.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbFileDetail.Size = new System.Drawing.Size(292, 127);
            this.txbFileDetail.TabIndex = 7;
            // 
            // lbMachine
            // 
            this.lbMachine.AutoSize = true;
            this.lbMachine.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbMachine.Location = new System.Drawing.Point(1, 70);
            this.lbMachine.Name = "lbMachine";
            this.lbMachine.Size = new System.Drawing.Size(103, 16);
            this.lbMachine.TabIndex = 8;
            this.lbMachine.Text = "Machine Name";
            // 
            // lbBoardID
            // 
            this.lbBoardID.AutoSize = true;
            this.lbBoardID.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbBoardID.Location = new System.Drawing.Point(38, 100);
            this.lbBoardID.Name = "lbBoardID";
            this.lbBoardID.Size = new System.Drawing.Size(66, 16);
            this.lbBoardID.TabIndex = 9;
            this.lbBoardID.Text = "Board ID";
            // 
            // lbFolder
            // 
            this.lbFolder.AutoSize = true;
            this.lbFolder.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.lbFolder.Location = new System.Drawing.Point(12, 39);
            this.lbFolder.Name = "lbFolder";
            this.lbFolder.Size = new System.Drawing.Size(76, 16);
            this.lbFolder.TabIndex = 10;
            this.lbFolder.Text = "folder path";
            this.lbFolder.Visible = false;
            // 
            // cbMachineName
            // 
            this.cbMachineName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbMachineName.FormattingEnabled = true;
            this.cbMachineName.Location = new System.Drawing.Point(111, 70);
            this.cbMachineName.Name = "cbMachineName";
            this.cbMachineName.Size = new System.Drawing.Size(121, 20);
            this.cbMachineName.TabIndex = 11;
            // 
            // cbBoardID
            // 
            this.cbBoardID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbBoardID.FormattingEnabled = true;
            this.cbBoardID.Location = new System.Drawing.Point(111, 96);
            this.cbBoardID.Name = "cbBoardID";
            this.cbBoardID.Size = new System.Drawing.Size(121, 20);
            this.cbBoardID.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(11, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 16;
            this.label1.Text = "Inspection ID";
            // 
            // txbInspectionID
            // 
            this.txbInspectionID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbInspectionID.Location = new System.Drawing.Point(111, 122);
            this.txbInspectionID.Name = "txbInspectionID";
            this.txbInspectionID.Size = new System.Drawing.Size(110, 22);
            this.txbInspectionID.TabIndex = 17;
            // 
            // txbComponetID
            // 
            this.txbComponetID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbComponetID.Location = new System.Drawing.Point(111, 150);
            this.txbComponetID.Name = "txbComponetID";
            this.txbComponetID.Size = new System.Drawing.Size(110, 22);
            this.txbComponetID.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(11, 156);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 16);
            this.label2.TabIndex = 18;
            this.label2.Text = "Componet ID";
            // 
            // cbApi
            // 
            this.cbApi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbApi.FormattingEnabled = true;
            this.cbApi.Items.AddRange(new object[] {
            "",
            "4-1",
            "4-3",
            "4-12"});
            this.cbApi.Location = new System.Drawing.Point(283, 172);
            this.cbApi.Name = "cbApi";
            this.cbApi.Size = new System.Drawing.Size(121, 20);
            this.cbApi.TabIndex = 20;
            this.cbApi.SelectedIndexChanged += new System.EventHandler(this.cbApi_SelectedIndexChanged);
            // 
            // txbProcessTime
            // 
            this.txbProcessTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txbProcessTime.Location = new System.Drawing.Point(109, 178);
            this.txbProcessTime.Name = "txbProcessTime";
            this.txbProcessTime.Size = new System.Drawing.Size(110, 22);
            this.txbProcessTime.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新細明體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label3.Location = new System.Drawing.Point(12, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Process Time";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(564, 473);
            this.Controls.Add(this.txbProcessTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbApi);
            this.Controls.Add(this.txbComponetID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbInspectionID);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbBoardID);
            this.Controls.Add(this.cbMachineName);
            this.Controls.Add(this.lbFolder);
            this.Controls.Add(this.lbBoardID);
            this.Controls.Add(this.lbMachine);
            this.Controls.Add(this.txbFileDetail);
            this.Controls.Add(this.lbMsg);
            this.Controls.Add(this.btnFolder);
            this.Controls.Add(this.txbApiUrl);
            this.Controls.Add(this.btnAPI);
            this.Controls.Add(this.textBox1);
            this.Name = "Form2";
            this.Text = "API Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnAPI;
        private System.Windows.Forms.TextBox txbApiUrl;
        private System.Windows.Forms.FolderBrowserDialog fbdCsv;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.Label lbMsg;
        private System.Windows.Forms.TextBox txbFileDetail;
        private System.Windows.Forms.Label lbMachine;
        private System.Windows.Forms.Label lbFolder;
        private System.Windows.Forms.Label lbBoardID;
        private System.Windows.Forms.ComboBox cbMachineName;
        private System.Windows.Forms.ComboBox cbBoardID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbInspectionID;
        private System.Windows.Forms.TextBox txbComponetID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbApi;
        private System.Windows.Forms.TextBox txbProcessTime;
        private System.Windows.Forms.Label label3;
    }
}