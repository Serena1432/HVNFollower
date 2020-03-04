using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace HVNFollower
{
    public partial class AddThisChuThot : Form
    {
        public string ctlink;
        public AddThisChuThot(string img, string name, string hhimg, string hh, string tvnhom, string gt, string date, string cmt, string like, string yen, string mota, string link)
        {
            InitializeComponent();
            avtImg.ImageLocation = img;
            label2.Text = name;
            hhIcon.ImageLocation = hhimg;
            label3.Text = hh;
            label5.Text = tvnhom;
            label7.Text = gt;
            label8.Text = date;
            label10.Text = cmt;
            label12.Text = like;
            label14.Text = yen;
            label16.Text = mota;
            ctlink = link;
        }

        private void AddThisChuThot_Load(object sender, EventArgs e)
        {
            if (label3.Text.Contains("Banned"))
            {
                this.Size = new Size(377, 345);
                label1.ForeColor = Color.Red;
                label1.Text = "Chủ thớt này đã bị khóa tài khoản, vì vậy nên bạn không thể thêm Chủ thớt này vào danh sách theo dõi.";
                button1.Visible = false;
                button2.Visible = false;
            }
            else if (label2.Text.Length <= 0)
            {
                this.Size = new Size(377, 345);
                label1.ForeColor = Color.Red;
                label1.Text = "Không thể tìm thấy thông tin của Chủ thớt này.";
                button1.Visible = false;
                button2.Visible = false;
            }
            else
            {
                this.Size = new Size(377, 385);
            }
        }

        public Bitmap ResizeImage(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage((Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);

            return result;
        }

        private void AddThisChuThot_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form1.No();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
                if (rk.GetValueNames().Contains(ctlink.Substring(ctlink.IndexOf("-") + 1)))
                {
                    MessageBox.Show("Bạn đã thêm Chủ thớt này rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    Bitmap bmpOutput = ResizeImage((Bitmap)avtImg.Image, 16, 16);
                    bmpOutput.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctlink.Substring(ctlink.IndexOf("-") + 1) + "_img.tmp", System.Drawing.Imaging.ImageFormat.Icon);
                    rk.SetValue(ctlink.Substring(ctlink.IndexOf("-") + 1), label2.Text);
                    MessageBox.Show("Thêm Chủ thớt thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                
            }
            catch
            {
                MessageBox.Show("Thêm Chủ thớt thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }
    }
}
