using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Omron_csv
{
    public partial class Form2 : Form
    {
        private string MachineName { get; set; }
        private string BoardID { get; set; }
        private string ProcessTime { get; set; }
        PublicClass pc = new PublicClass();
        public Form2()
        {
            InitializeComponent();
            string apiUrl = "";
            //test
            apiUrl = "https://echo.getpostman.com/response-headers?Content-Type=text/html&test=response_headers";
            //On Server
            //apiUrl = "http://192.168.0.1:8080/prismRest/resources/GetPcbInfoByBarcode";
            txbApiUrl.Text = apiUrl;
            btnAPI.Enabled = false;
            cbMachineName.Visible = false;
            cbBoardID.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnAPI.Enabled = false;
            //手動修正
            pc.apiUrl = txbApiUrl.Text;
            string returnXML = "";
            pc.MachineName = cbMachineName.SelectedItem.ToString();
            pc.BoardID = cbBoardID.SelectedItem.ToString();
            pc.ProcessTime = txbProcessTime.Text;
            if (cbApi.SelectedItem.ToString().Equals("4-1"))
            {
                List<Inspection> ins = new List<Inspection>();
                try
                {
                    returnXML = pc.CallApi(cbApi.SelectedItem.ToString());
                    txbFileDetail.Text = returnXML;
                    ins = pc.getInspectionID(returnXML);
                    textBox1.Text = DateTime.Now.ToString() + Environment.NewLine;
                    textBox1.Text += "Record Count: " + ins.Count.ToString() + Environment.NewLine;
                    foreach (Inspection item in ins)
                    {
                        textBox1.Text += "Machine Name: " + item.MachineName + Environment.NewLine;
                        textBox1.Text += "Inspection ID: " + item.InspectionID + Environment.NewLine;
                        //textBox1.Text += "confirm_date: " + item.confrim_date + Environment.NewLine;
                        textBox1.Text += "inspection_end_time: " + item.inspection_end_time + Environment.NewLine;
                        textBox1.Text += "------------------------" + Environment.NewLine;
                    }
                }
                catch (Exception ex)
                {
                    textBox1.Text = ex.Message;
                    //throw;
                }

            }
            else if (cbApi.SelectedItem.ToString().Equals("4-3"))
            {
                if (txbInspectionID.Text.Trim().Length > 0)
                {
                    pc.InspectionID = txbInspectionID.Text;
                    //returnXML = pc.testXML_component;
                    returnXML = pc.CallApi(cbApi.SelectedItem.ToString());
                    txbFileDetail.Text = returnXML;
                    List<Inspection> output = new List<Inspection>();
                    pc.MachineName = cbMachineName.SelectedItem.ToString();
                    pc.InspectionID = txbInspectionID.Text;
                    output = pc.getComponentID(returnXML);
                    textBox1.Text = DateTime.Now.ToString() + Environment.NewLine;
                    textBox1.Text += "Record Count: " + output.Count.ToString() + Environment.NewLine;
                    foreach (Inspection item in output)
                    {
                        textBox1.Text += "Machine Name: " + item.MachineName + Environment.NewLine;
                        textBox1.Text += "Inspection ID: " + item.InspectionID + Environment.NewLine;
                        textBox1.Text += "SegmentCode: " + item.SegmentCode + Environment.NewLine;
                        textBox1.Text += "Segment NO: " + item.SegmentNo + Environment.NewLine;
                        textBox1.Text += "LastResult: " + item.LastResult + Environment.NewLine;
                        textBox1.Text += "------------------------" + Environment.NewLine;
                    }
                }
                else
                {
                    textBox1.Text = "Must have Inspection ID";
                }
            }
            else if (cbApi.SelectedItem.ToString().Equals("4-12"))
            {
                if (txbComponetID.Text.Trim().Length > 0 && txbInspectionID.Text.Trim().Length > 0)
                {
                    pc.MachineName = cbMachineName.SelectedItem.ToString();
                    pc.InspectionID = txbInspectionID.Text.Trim();
                    pc.ComponentID = txbComponetID.Text.Trim();
                    returnXML = pc.CallApi(cbApi.SelectedItem.ToString());
                    txbFileDetail.Text = returnXML;
                    List<Inspection> output = new List<Inspection>();
                    pc.MachineName = cbMachineName.SelectedItem.ToString();
                    pc.InspectionID = txbInspectionID.Text;
                    output = pc.getImagePath(returnXML);
                    textBox1.Text = DateTime.Now.ToString() + Environment.NewLine;
                    textBox1.Text += "Record Count: " + output.Count.ToString() + Environment.NewLine;
                    foreach (Inspection item in output)
                    {
                        textBox1.Text += "Machine Name: " + item.MachineName + Environment.NewLine;
                        textBox1.Text += "Inspection ID: " + item.InspectionID + Environment.NewLine;
                        textBox1.Text += "Component ID: " + item.ComponentID + Environment.NewLine;
                        textBox1.Text += "Inspection Image Path:" + item.InspectionImagePath + Environment.NewLine;
                        textBox1.Text += "------------------------" + Environment.NewLine;
                    }
                }
                else
                {
                    textBox1.Text = "Must have Component ID and Inspection ID";
                }
            }
            btnAPI.Enabled = true;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            DirectoryInfo di = new DirectoryInfo(@"C:\CSV\CSV\MCSV");
            if (di.Exists)
            {
                //set defalut folder
                fbdCsv.SelectedPath = @"C:\CSV\CSV\MCSV";
            }
            if (fbdCsv.ShowDialog() == DialogResult.OK)
                lbFolder.Text = fbdCsv.SelectedPath;
            else
                lbFolder.Text = "";
            //
            if (lbFolder.Text.Length == 0)
            {
                lbMsg.Text = "必須選擇資料夾";
            }
            else
            {
                lbMsg.Text = "檔案分析如右側";

                //
                var dir = new DirectoryInfo(lbFolder.Text);
                FileInfo[] files = dir.GetFiles("*.csv");
                //
                if (Directory.Exists(lbFolder.Text) && files.Length > 0)
                {
                    string showStr = "";
                    //改用CSV內容來建立下拉選單
                    bool csv = false;
                    if (csv)
                    {
                        cbMachineName.Items.Clear();
                        cbBoardID.Items.Clear();

                        DataTable dt = pc.Csv2Datatable(files[0].FullName);
                        var r = dt.Rows[0];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            string c = r[i].ToString();
                            if (c.Trim().Length != 0)
                            {
                                cbMachineName.Items.Add(c.Trim());
                                cbBoardID.Items.Add(c.Trim());
                            }
                        }
                        txbFileDetail.Text = "已從檔案中取出欄位當選項，請從下拉選單選擇數據後測試API";
                    }
                    else
                    {
                        CsvFileName ifn = pc.GetParameterFromCsv(Path.GetFileNameWithoutExtension(files[0].Name));
                        //檢查時間參數，如果時間參數錯誤代表檔案名稱中有多餘的底線
                        if (ifn.ProcessTime.Length == 12)
                        {
                            lbMsg.Text = "參數解析完成！";
                        }
                        else
                        {
                            lbMsg.Text = "參數解析錯誤，檔案名稱是否包含多餘的底線？";
                        }
                        showStr += "File: " + files[0].Name + Environment.NewLine;
                        showStr += "Machine Name: " + ifn.MachineName + Environment.NewLine;
                        showStr += "BoardID: " + ifn.BoardID + Environment.NewLine;
                        showStr += "ProcessName: " + ifn.ProcessName + Environment.NewLine;
                        showStr += "ProcessTime: 20" + ifn.ProcessTime + Environment.NewLine;
                        txbFileDetail.Text = showStr;
                        //將參數填入下拉選單
                        cbMachineName.Items.Add(ifn.MachineName);
                        cbMachineName.Items.Add(ifn.ProcessName);
                        cbMachineName.Items.Add(ifn.BoardID);
                        cbMachineName.Items.Add(ifn.ProcessTime);
                        cbBoardID.Items.Add(ifn.MachineName);
                        cbBoardID.Items.Add(ifn.ProcessName);
                        cbBoardID.Items.Add(ifn.BoardID);
                        cbBoardID.Items.Add(ifn.ProcessTime);
                        //
                        this.MachineName = ifn.MachineName;
                        this.BoardID = ifn.BoardID;
                        txbProcessTime.Text = "20" + ifn.ProcessTime;
                        this.ProcessTime = "20" + ifn.ProcessTime;
                    }

                    txbApiUrl.Text = "http://192.168.0.1:8080/prismRest/resources/GetPcbInfoByBarcode";
                    //default selected item
                    cbMachineName.SelectedIndex = 0;
                    cbBoardID.SelectedIndex = 0;
                    cbMachineName.Visible = true;
                    cbBoardID.Visible = true;

                }
                else
                {
                    lbMsg.Text = "無此路徑 或 無符合之檔案！";
                }
            }
        }

        private void cbApi_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbApi.SelectedIndex > 0)
            {
                switch (cbApi.SelectedItem.ToString())
                {
                    case "4-1":
                        pc.apiUrl = "http://192.168.0.1:8080/prismRest/resources/GetPcbInfoByBarcode";
                        break;
                    case "4-3":
                        pc.apiUrl = "http://192.168.0.1:8080/prismRest/resources/GetPcbCompInfo";
                        break;
                    case "4-12":
                        //pc.apiUrl = "http://192.168.0.1:8080/prismRest/resources/GetNGImageList";
                        pc.apiUrl = "http://192.168.0.1:8080/prismRest/resources/GetWindowDetailInfo";
                        break;
                    default:
                        break;
                }
                txbApiUrl.Text = pc.apiUrl;
                btnAPI.Enabled = true;
            }
        }
    }
}

