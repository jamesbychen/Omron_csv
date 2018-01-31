using NT88Test;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omron_csv
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
            textBox1.Text = "1234567890ABCDEF";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String NTCode = textBox1.Text;//获取到加密锁识别码

            int Rtn = NT88API.NTFindFirst(NTCode);//查找指定加密锁识别码的加密锁，如果返回值为 0，表示加密锁存在。
            //如果返回值不为0，则可以通过返回值Rtn查看错误代码
            if (Rtn != 0)
            {
                this.listBox1.Items.Add("Error on getting lock:" + Rtn.ToString());
            }
            else
            {
                this.listBox1.Items.Add("Lock Checked!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder HardwareID = new StringBuilder(32);// 硬件序列号

            int Rtn = NT88API.NTGetHardwareID(HardwareID);//获取硬件序列号，如果返回值为 0，表示获取硬件序列号成功。
            //如果返回值不为0，则可以通过返回值Rtn查看错误代码
            if (Rtn != 0)
            {
                this.listBox1.Items.Add("Error on getting Hardware ID:" + Rtn.ToString());
            }
            else
            {
                this.listBox1.Items.Add("Hardware ID:"+ HardwareID.ToString());
            }
        }
    }
}
