using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace Omron_csv
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            lbMsg.Text = "按下「Get Folder」後選擇CSV資料夾，按下確定程式便會開始轉換檔案";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            int rowscount = 0;
            int filescount = 0;
            string dirStr = "C:\\CSV\\Machine";
            selectFolder();
            //
            if (txbFolder.Text.Length > 0)
                dirStr = txbFolder.Text;

            txbMsg.Text = "Starting Check Folder!" + Environment.NewLine;
            Thread.Sleep(1000);
            var dir = new DirectoryInfo(dirStr);
            FileInfo[] files = dir.GetFiles("*.csv");
            if (Directory.Exists(dirStr) && files.Length > 0)
            {
                foreach (var f in files)
                {
                    //
                    Thread.Sleep(1000);
                    PublicClass pc = new PublicClass();
                    CsvFileName ifn = pc.GetParameterFromCsv(Path.GetFileNameWithoutExtension(f.Name));
                    //test DataTable
                    DataTable dt = new DataTable();
                    dt = pc.Csv2Datatable(f.FullName);
                    txbMsg.Text += "處理檔案：" + f.FullName + Environment.NewLine;
                    rowscount += dt.Rows.Count;
                    //Create Output
                    DirectoryInfo outputPath = new DirectoryInfo(dir.Parent.FullName + "\\output");

                    if (!outputPath.Exists)
                    {
                        txbMsg.Text += "新增output資料夾" + Environment.NewLine;
                        dir.Parent.CreateSubdirectory("output");
                    }
                    pc.Datatable2Csv(dt, outputPath.FullName + "\\" + f.Name);
                    filescount++;
                    //Delete Original Files
                    File.Delete(f.FullName);
                }
                txbMsg.Text += "共處理" + rowscount.ToString() + "行資料，" + filescount.ToString() + "個檔案";
                lbMsg.Text = "原檔案已刪除，檔案轉換至output成功！";
            }
            else
            {
                lbMsg.Text = "無此路徑 或 無符合之檔案！";
            }
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private void trickTimer()
        {
            lbMsg.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            MessageBox.Show("Alert", "1 minute is up!");
        }

        private void txbTrickTimer_Click(object sender, EventArgs e)
        {
            lbMsg.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            //
            System.Timers.Timer t = new System.Timers.Timer();
            t.Interval = 60 * 1000;
            t.Elapsed += timerEvent;
            t.AutoReset = true;
            t.Start();
        }

        private void timerEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            trickTimer();
            //throw new NotImplementedException();
        }

        private void selectFolder()
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txbFolder.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void txbMsg_TextChanged(object sender, EventArgs e)
        {
            Application.DoEvents();//即時顯示一行
            txbMsg.SelectionStart = txbMsg.Text.Length;
            txbMsg.ScrollToCaret();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            PublicClass pub = new PublicClass();
            List<Inspection> ins = new List<Inspection>();
            ins = pub.getComponentID(pub.testXML);
            List<Inspection> goodComponentList = new List<Inspection>();
            //先取得Segment Code名稱
            List<string> segmentCode = new List<string>();
            segmentCode = ins.Where(p => p.SegmentNo != "0").GroupBy(p => p.SegmentCode).Select(grp => grp.First()).Select(x => x.SegmentCode).ToList();
            foreach (string codename in segmentCode)
            {
                List<Inspection> _list = new List<Inspection>();
                _list = ins.Where(p => p.SegmentCode == codename).GroupBy(x => x.SegmentNo).Select(grp => grp.First()).ToList();
                goodComponentList =goodComponentList.Concat(_list).ToList();
            }
            Console.WriteLine(goodComponentList);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Show();
        }
    }
}
