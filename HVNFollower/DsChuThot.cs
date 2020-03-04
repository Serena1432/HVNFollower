using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Net;

namespace HVNFollower
{
    public partial class DsChuThot : Form
    {
        public DsChuThot()
        {
            InitializeComponent();
        }

        string selectedName;
        int nb;
        bool exporting;
        Panel[] panel = new Panel[9999];
        PictureBox[] ctpic = new PictureBox[9999];
        Label[] ctlabel = new Label[9999];
        Label[] ctidlabel = new Label[9999];
        CheckBox[] ctdelete = new CheckBox[9999];
        string[] ctdel = new string[9999];

        private bool UniqueKeyExists(string unique_key)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create("http://ichika.shiru2005.tk/HVNFollower/FollowerList/" + unique_key + ".reg") as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        private bool JsonUniqueKeyExists(string unique_key)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create("http://ichika.shiru2005.tk/HVNFollower/FollowerList/" + unique_key + ".json") as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }

        private void DsChuThot_Load(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
            label5.Text = "Số Chủ thớt hiện tại: " + rk.GetValueNames().Count().ToString();
            if (rk.GetValueNames().Count() == 0)
            {
                Label lbl = new Label();
                lbl.Location = new Point(259, 130);
                lbl.AutoSize = true;
                lbl.ForeColor = Color.Gray;
                lbl.Text = "Không có Chủ thớt nào trong Danh sách theo dõi.";
                panel1.Controls.Add(lbl);
                button1.Enabled = false;
                button2.Enabled = false;
            }
            else
            {
                for (int i = 0; i < rk.GetValueNames().Count(); i++)
                {
                    panel[i] = new Panel();
                    panel[i].Location = new Point(18, 13 + i * 33);
                    panel[i].Size = new Size(756, 29);
                    panel[i].BorderStyle = BorderStyle.FixedSingle;
                    panel1.Controls.Add(panel[i]);
                    ctdelete[i] = new CheckBox();
                    ctdelete[i].Location = new Point(8, 7);
                    ctdelete[i].Size = new Size(15, 14);
                    ctdelete[i].Text = "";
                    ctdelete[i].Name = rk.GetValueNames()[i].ToString();
                    panel[i].Controls.Add(ctdelete[i]);
                    ctpic[i] = new PictureBox();
                    ctpic[i].Location = new Point(29, 5);
                    ctpic[i].Size = new Size(16, 16);
                    ctpic[i].ImageLocation = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + rk.GetValueNames()[i] + "_img.tmp";
                    panel[i].Controls.Add(ctpic[i]);
                    ctlabel[i] = new Label();
                    ctlabel[i].Location = new Point(50, 6);
                    ctlabel[i].AutoSize = true;
                    ctlabel[i].Font = new Font("Tahoma", 9F, FontStyle.Bold);
                    ctlabel[i].Text = rk.GetValue(rk.GetValueNames()[i], "Không rõ").ToString();
                    panel[i].Controls.Add(ctlabel[i]);
                    ctidlabel[i] = new Label();
                    ctidlabel[i].Location = new Point(624, 6);
                    ctidlabel[i].AutoSize = true;
                    ctidlabel[i].Font = new Font("Tahoma", 9F);
                    ctidlabel[i].ForeColor = Color.Gray;
                    ctidlabel[i].Text = "ID Chủ thớt: " + rk.GetValueNames()[i];
                    panel[i].Controls.Add(ctidlabel[i]);
                }
            }
            RegistryKey rk2 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
            label3.Text = "Số Tác giả hiện tại: " + rk2.GetValueNames().Count().ToString();
            if (rk2.GetValueNames().Count() == 0)
            {
                Label lbl = new Label();
                lbl.Location = new Point(259, 130);
                lbl.AutoSize = true;
                lbl.ForeColor = Color.Gray;
                lbl.Text = "Không có Tác giả nào trong Danh sách theo dõi.";
                panel2.Controls.Add(lbl);
                button4.Enabled = false;
                button5.Enabled = false;
            }
            else
            {
                for (int i = 0; i < rk2.GetValueNames().Count(); i++)
                {
                    panel[i + 2500] = new Panel();
                    panel[i + 2500].Location = new Point(18, 13 + i * 33);
                    panel[i + 2500].Size = new Size(756, 29);
                    panel[i + 2500].BorderStyle = BorderStyle.FixedSingle;
                    panel2.Controls.Add(panel[i + 2500]);
                    ctdelete[i + 2500] = new CheckBox();
                    ctdelete[i + 2500].Location = new Point(8, 7);
                    ctdelete[i + 2500].Size = new Size(15, 14);
                    ctdelete[i + 2500].Text = "";
                    ctdelete[i + 2500].Name = rk2.GetValueNames()[i].ToString();
                    panel[i + 2500].Controls.Add(ctdelete[i + 2500]);
                    ctlabel[i + 2500] = new Label();
                    ctlabel[i + 2500].Location = new Point(29, 6);
                    ctlabel[i + 2500].AutoSize = true;
                    ctlabel[i + 2500].Font = new Font("Tahoma", 9F, FontStyle.Bold);
                    ctlabel[i + 2500].Text = rk2.GetValue(rk2.GetValueNames()[i], "Không rõ").ToString();
                    panel[i + 2500].Controls.Add(ctlabel[i + 2500]);
                }
            }
            RegistryKey rk3 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
            label4.Text = "Số Doujinshi hiện tại: " + rk3.GetValueNames().Count().ToString();
            if (rk3.GetValueNames().Count() == 0)
            {
                Label lbl = new Label();
                lbl.Location = new Point(259, 130);
                lbl.AutoSize = true;
                lbl.ForeColor = Color.Gray;
                lbl.Text = "Không có Doujinshi nào trong Danh sách theo dõi.";
                panel3.Controls.Add(lbl);
                button7.Enabled = false;
                button8.Enabled = false;
            }
            else
            {
                for (int i = 0; i < rk3.GetValueNames().Count(); i++)
                {
                    panel[i + 5000] = new Panel();
                    panel[i + 5000].Location = new Point(18, 13 + i * 33);
                    panel[i + 5000].Size = new Size(756, 29);
                    panel[i + 5000].BorderStyle = BorderStyle.FixedSingle;
                    panel3.Controls.Add(panel[i + 5000]);
                    ctdelete[i + 5000] = new CheckBox();
                    ctdelete[i + 5000].Location = new Point(8, 7);
                    ctdelete[i + 5000].Size = new Size(15, 14);
                    ctdelete[i + 5000].Text = "";
                    ctdelete[i + 5000].Name = rk3.GetValueNames()[i].ToString();
                    panel[i + 5000].Controls.Add(ctdelete[i + 5000]);
                    ctlabel[i + 5000] = new Label();
                    ctlabel[i + 5000].Location = new Point(29, 6);
                    ctlabel[i + 5000].AutoSize = true;
                    ctlabel[i + 5000].Font = new Font("Tahoma", 9F, FontStyle.Bold);
                    ctlabel[i + 5000].Text = rk3.GetValue(rk3.GetValueNames()[i], "Không rõ").ToString();
                    panel[i + 5000].Controls.Add(ctlabel[i + 5000]);
                }
            }
            RegistryKey rk4 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
            label2.Text = "Số Nhóm dịch hiện tại: " + rk4.GetValueNames().Count().ToString();
            if (rk4.GetValueNames().Count() == 0)
            {
                Label lbl = new Label();
                lbl.Location = new Point(259, 130);
                lbl.AutoSize = true;
                lbl.ForeColor = Color.Gray;
                lbl.Text = "Không có Nhóm dịch nào trong Danh sách theo dõi.";
                panel4.Controls.Add(lbl);
                button10.Enabled = false;
                button11.Enabled = false;
            }
            else
            {
                for (int i = 0; i < rk4.GetValueNames().Count(); i++)
                {
                    panel[i + 7500] = new Panel();
                    panel[i + 7500].Location = new Point(18, 13 + i * 33);
                    panel[i + 7500].Size = new Size(756, 29);
                    panel[i + 7500].BorderStyle = BorderStyle.FixedSingle;
                    panel4.Controls.Add(panel[i + 7500]);
                    ctdelete[i + 7500] = new CheckBox();
                    ctdelete[i + 7500].Location = new Point(8, 7);
                    ctdelete[i + 7500].Size = new Size(15, 14);
                    ctdelete[i + 7500].Text = "";
                    ctdelete[i + 7500].Name = rk4.GetValueNames()[i].ToString();
                    panel[i + 7500].Controls.Add(ctdelete[i + 7500]);
                    ctlabel[i + 7500] = new Label();
                    ctlabel[i + 7500].Location = new Point(29, 6);
                    ctlabel[i + 7500].AutoSize = true;
                    ctlabel[i + 7500].Font = new Font("Tahoma", 9F, FontStyle.Bold);
                    ctlabel[i + 7500].Text = rk4.GetValue(rk4.GetValueNames()[i], "Không rõ").ToString();
                    panel[i + 7500].Controls.Add(ctlabel[i + 7500]);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ctDelList = "";
            nb = 0;
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
            try
            {
                for (int i = 0; i < rk.GetValueNames().Count(); i++)
                {
                    foreach (Control c in panel[i].Controls)
                    {
                        if (c is CheckBox && (c as CheckBox).Checked)
                        {
                            nb++;
                            ctdel[nb] = c.Name;
                            ctDelList += rk.GetValue(c.Name, "Không rõ") + "\n";
                        }
                    }
                }
                if (ctDelList != "")
                {
                    DialogResult mess = MessageBox.Show("Bạn chuẩn bị xóa các Chủ thớt sau:\n" + ctDelList + "\nBạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        try
                        {
                            ctDelList = "";
                            for (int i = 1; i <= nb; i++)
                            {
                                string ctName = ctdel[i];
                                string tempName = rk.GetValue(ctName, "Không rõ").ToString();
                                rk.DeleteValue(ctName);
                                if (!rk.GetValueNames().Contains(ctName))
                                {
                                    ctDelList += tempName + "\n";
                                }
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + ".tmp");
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + "_img.tmp");
                            }
                            MessageBox.Show("Xóa thành công các Chủ thớt sau:\n" + ctDelList, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi trong khi xóa các Chủ thớt.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn Chủ thớt nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi trong khi thực hiện lệnh.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    

        private void button2_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa tất cả các Chủ thớt?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
                try
                {
                    foreach (string value in rk.GetValueNames())
                    {
                        rk.DeleteValue(value, false);
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + value + ".tmp");
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + value + "_img.tmp");
                    }
                    MessageBox.Show("Xóa tất cả Chủ thớt thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi trong khi xóa Chủ thớt. Vui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string ctDelList = "";
            nb = 0;
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
            try
            {
                for (int i = 0; i < rk.GetValueNames().Count(); i++)
                {
                    foreach (Control c in panel[i + 2500].Controls)
                    {
                        if (c is CheckBox && (c as CheckBox).Checked)
                        {
                            nb++;
                            ctdel[nb] = c.Name;
                            ctDelList += rk.GetValue(c.Name, "Không rõ") + "\n";
                        }
                    }
                }
                if (ctDelList != "")
                {
                    DialogResult mess = MessageBox.Show("Bạn chuẩn bị xóa các Tác giả sau:\n" + ctDelList + "\nBạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        try
                        {
                            ctDelList = "";
                            for (int i = 1; i <= nb; i++)
                            {
                                string ctName = ctdel[i];
                                string tempName = rk.GetValue(ctName, "Không rõ").ToString();
                                rk.DeleteValue(ctName);
                                if (!rk.GetValueNames().Contains(ctName))
                                {
                                    ctDelList += tempName + "\n";
                                }
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + ".tmp");
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + "_img.tmp");
                            }
                            MessageBox.Show("Xóa thành công các Tác giả sau:\n" + ctDelList, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi trong khi xóa các Tác giả.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn Tác giả nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Lỗi trong khi thực hiện lệnh.\nVui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa tất cả các Tác giả?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
                try
                {
                    foreach (string value in rk.GetValueNames())
                    {
                        rk.DeleteValue(value, false);
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + value + ".tmp");
                    }
                    MessageBox.Show("Xóa tất cả Tác giả thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

                catch
                {
                    MessageBox.Show("Có lỗi trong khi xóa các Tác giả. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void exportRegistry(string strKey, string filepath)
        {
            try
            {
                using (Process proc = new Process())
                {
                    proc.StartInfo.FileName = "reg.exe";
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.Arguments = "export \"" + strKey + "\" \"" + filepath + "\" /y";
                    proc.Start();
                    string stdout = proc.StandardOutput.ReadToEnd();
                    string stderr = proc.StandardError.ReadToEnd();
                    proc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tạo tệp tin sao lưu.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Export testDialog = new Export();
                MessageBox.Show("Kể từ phiên bản 1.4, bạn đã có thể sao lưu danh sách Chủ thớt và Tác giả đã theo dõi trực tiếp lên mạng bằng máy chủ của HVN Follower.\nNhững gì bạn cần là nhập một đoạn mã bất kỳ (không phải tên đăng nhập hay mật khẩu trên HentaiVN) để HVN Follower có thể nhận ra tệp sao lưu của bạn trên máy tính khác.\nHãy nhớ mã bạn đã nhập để có thể khôi phục lại trên máy tính khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button3.Enabled = false;
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    button3.Text = "Đang kiểm tra...";
                    string unique_key = testDialog.textBox1.Text;
                    if (UniqueKeyExists(unique_key) == true)
                    {
                        MessageBox.Show("Mã này đã tồn tại trên máy chủ.\nVui lòng thử lại bằng một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button3.Text = "Sao lưu danh sách";
                        button3.Enabled = true;
                    }
                    else
                    {
                        exporting = true;
                        button3.Text = "Đang tạo tệp sao lưu...";
                        exportRegistry("HKEY_CURRENT_USER\\SOFTWARE\\LilShieru\\HVNFollower", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg");
                        System.Net.WebClient Client = new System.Net.WebClient();

                        Client.Headers.Add("Content-Type", "binary/octet-stream");

                        byte[] result = Client.UploadFile("http://ichika.shiru2005.tk/HVNFollower/FollowListUploader.php", "POST",
                                                          Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg");

                        string s = System.Text.Encoding.UTF8.GetString(result, 0, result.Length); 
                        if (s == "Upload successfully!")
                        {
                            MessageBox.Show("Sao lưu lên máy chủ thành công!\n\nMã sao lưu của bạn là:\n" + unique_key + "\n\nHãy nhớ mã sao lưu này để khôi phục lại danh sách vào máy tính khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            exporting = false;
                            button3.Enabled = true;
                            button3.Text = "Sao lưu danh sách";
                        }
                        else
                        {
                            MessageBox.Show("Sao lưu lên máy chủ thất bại! Vui lòng thử lại sau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            exporting = false;
                            button3.Enabled = true;
                            button3.Text = "Sao lưu danh sách";
                        }
                    }
                }
                else
                {
                    button3.Enabled = true;
                }
                testDialog.Dispose();
            }
            catch
            {
                MessageBox.Show("Sao lưu lên máy chủ thất bại! Vui lòng thử lại sau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                exporting = false;
                button3.Enabled = true;
                button3.Text = "Sao lưu danh sách";
            }
        }

        private void DsChuThot_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (exporting == true)
            {
                MessageBox.Show("Chưa thể đóng cửa sổ khi đang sao lưu danh sách.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                Export testDialog = new Export();
                testDialog.label1.Text = "Nhập đoạn mã bạn đã nhập trong lúc sao lưu để khôi phục lại\ntệp của bạn:";
                button6.Enabled = false;
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    string unique_key = testDialog.textBox1.Text;
                    if (UniqueKeyExists(unique_key) == true)
                    {
                        using (var client = new WebClient())
                        {
                            client.Encoding = Encoding.UTF8;
                            client.DownloadFile("http://ichika.shiru2005.tk/HVNFollower/FollowerList/" + unique_key + ".reg", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg");
                        }
                        if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg"))
                        {
                            Process proc = new Process();

                            try
                            {
                                proc.StartInfo.FileName = "reg.exe";
                                proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                proc.StartInfo.CreateNoWindow = true;
                                proc.StartInfo.UseShellExecute = false;

                                string command = "import " + Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg";
                                proc.StartInfo.Arguments = command;
                                proc.Start();

                                proc.WaitForExit();
                                MessageBox.Show("Khôi phục danh sách thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                            catch (System.Exception)
                            {
                                MessageBox.Show("Khôi phục danh sách thất bại!\nVui lòng thử lại sau!\nLý do: Không thể khôi phục tệp tin đã tải xuống.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                proc.Dispose();
                            }
                            System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + unique_key + ".reg");
                        }
                        else
                        {
                            MessageBox.Show("Khôi phục danh sách thất bại!\nVui lòng thử lại sau!\nLý do: Không thể tìm được tệp tin khôi phục đã tải xuống từ máy chủ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mã này không tồn tại trên máy chủ!\nVui lòng thử lại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button6.Enabled = true;
                    }

                }
                else
                {
                    button6.Enabled = true;
                }
            }
            catch
            {
                MessageBox.Show("Khôi phục danh sách thất bại!\nVui lòng thử lại sau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string ctDelList = "";
            nb = 0;
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
            try
            {
                for (int i = 0; i < rk.GetValueNames().Count(); i++)
                {
                    foreach (Control c in panel[i + 5000].Controls)
                    {
                        if (c is CheckBox && (c as CheckBox).Checked)
                        {
                            nb++;
                            ctdel[nb] = c.Name;
                            ctDelList += rk.GetValue(c.Name, "Không rõ") + "\n";
                        }
                    }
                }
                if (ctDelList != "")
                {
                    DialogResult mess = MessageBox.Show("Bạn chuẩn bị xóa các Doujinshi sau:\n" + ctDelList + "\nBạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        try
                        {
                            ctDelList = "";
                            for (int i = 1; i <= nb; i++)
                            {
                                string ctName = ctdel[i];
                                string tempName = rk.GetValue(ctName, "Không rõ").ToString();
                                rk.DeleteValue(ctName);
                                if (!rk.GetValueNames().Contains(ctName))
                                {
                                    ctDelList += tempName + "\n";
                                }
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + ".tmp");
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + "_img.tmp");
                            }
                            MessageBox.Show("Xóa thành công các Doujinshi sau:\n" + ctDelList, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi trong khi xóa các Doujinshi.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn Doujinshi nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Lỗi trong khi thực hiện lệnh.\nVui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa tất cả các Doujinshi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
                try
                {
                    foreach (string value in rk.GetValueNames())
                    {
                        rk.DeleteValue(value, false);
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(value) + ".tmp");
                    }
                    MessageBox.Show("Xóa tất cả Doujinshi thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

                catch
                {
                    MessageBox.Show("Có lỗi trong khi xóa các Doujinshi. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
           try
           {
                Export testDialog = new Export();
                MessageBox.Show("Kể từ phiên bản 1.9.3, HVN Follower trên máy tính đã có thể chuyển danh sách tới HVN Follower trên điện thoại.\nNhững gì bạn cần là nhập một đoạn mã bất kỳ (không phải tên đăng nhập hay mật khẩu trên HentaiVN) để HVN Follower có thể nhận ra tệp sao lưu của bạn với người khác.\nHãy nhớ mã bạn đã nhập để có thể khôi phục lại về điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                button9.Enabled = false;
                if (testDialog.ShowDialog(this) == DialogResult.OK)
                {
                    button9.Text = "Đang kiểm tra...";
                    button9.Text = "Đang tạo tệp sao lưu...";
                    string unique_key = testDialog.textBox1.Text;
                    if (JsonUniqueKeyExists(unique_key) == true)
                    {
                        MessageBox.Show("Mã này đã tồn tại trên máy chủ.\nVui lòng thử lại bằng một mã khác.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        button9.Text = "Chuyển về điện thoại";
                        button9.Enabled = true;
                    }
                    else
                    {
                        exporting = true;
                        WebClient client = new WebClient();
                        RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
                        string json = "{\"uploader\":[";
                        foreach (string value in rk.GetValueNames())
                        {
                            string userResponse = client.DownloadString("https://hentaivn.net/user-" + value);
                            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                            doc.LoadHtml(userResponse);
                            var img = doc.DocumentNode.SelectSingleNode("//div[@class='wall-avatar']//img[@src]");
                            string src = img.GetAttributeValue("src", "nothing");
                            if (!src.Contains("https://"))
                            {
                                src = "https://hentaivn.net" + src;
                            }
                            json += "{\"id\":\"" + value + "\",\"avatar\":\"" + src + "\",\"name\":\"" + rk.GetValue(value) + "\"},";
                        }
                        json = json.Substring(0, json.Length - 1) + "],\"author\":[";
                        RegistryKey rk2 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
                        foreach (string value in rk2.GetValueNames())
                        {
                            
                            json += "{\"id\":\"" + value + "\",\"name\":\"" + rk2.GetValue(value) + "\"},";
                        }
                        json = json.Substring(0, json.Length - 1) + "],\"doujin\":[";
                        RegistryKey rk3 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
                        foreach (string value in rk3.GetValueNames())
                        {

                            json += "{\"id\":\"" + value + "\",\"name\":\"" + rk3.GetValue(value) + "\"},";
                        }
                        json = json.Substring(0, json.Length - 1) + "],\"group\":[";
                        RegistryKey rk4 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
                        foreach (string value in rk4.GetValueNames())
                        {

                            json += "{\"id\":\"" + value + "\",\"name\":\"" + rk4.GetValue(value) + "\"},";
                        }
                        json = json.Substring(0, json.Length - 1) + "]}";

                        var request = (HttpWebRequest)WebRequest.Create("http://ichika.shiru2005.tk/HVNFollower/JsonListUploader.php");

                        var postData = "data=" + Uri.EscapeDataString(json);
                        postData += "&unique_key=" + Uri.EscapeDataString(unique_key);
                        var data = Encoding.ASCII.GetBytes(postData);

                        request.Method = "POST";
                        request.ContentType = "application/x-www-form-urlencoded";
                        request.ContentLength = data.Length;

                        using (var stream = request.GetRequestStream())
                        {
                            stream.Write(data, 0, data.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();

                        var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

                        if (responseString == "Upload successfully!")
                        {
                            MessageBox.Show("Sao lưu lên máy chủ thành công!\n\nMã sao lưu của bạn là:\n" + unique_key + "\n\nHãy nhớ mã sao lưu này để khôi phục lại danh sách vào điện thoại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            exporting = false;
                            button9.Enabled = true;
                            button9.Text = "Chuyển về điện thoại";
                        }
                        else
                        {
                            MessageBox.Show("Sao lưu lên máy chủ thất bại! Vui lòng thử lại sau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            exporting = false;
                            button9.Enabled = true;
                            button9.Text = "Chuyển về điện thoại";
                        }
                    }
                }
                else
                {
                    button3.Enabled = true;
                }
                testDialog.Dispose();
           }
           catch
           {
               MessageBox.Show("Sao lưu lên máy chủ thất bại! Vui lòng thử lại sau!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
               exporting = false;
               button3.Enabled = true;
               button3.Text = "Sao lưu danh sách";
           }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            string ctDelList = "";
            nb = 0;
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
            try
            {
                for (int i = 0; i < rk.GetValueNames().Count(); i++)
                {
                    foreach (Control c in panel[i + 7500].Controls)
                    {
                        if (c is CheckBox && (c as CheckBox).Checked)
                        {
                            nb++;
                            ctdel[nb] = c.Name;
                            ctDelList += rk.GetValue(c.Name, "Không rõ") + "\n";
                        }
                    }
                }
                if (ctDelList != "")
                {
                    DialogResult mess = MessageBox.Show("Bạn chuẩn bị xóa các Nhóm dịch sau:\n" + ctDelList + "\nBạn có chắc chắn muốn xóa không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        try
                        {
                            ctDelList = "";
                            for (int i = 1; i <= nb; i++)
                            {
                                string ctName = ctdel[i];
                                string tempName = rk.GetValue(ctName, "Không rõ").ToString();
                                rk.DeleteValue(ctName);
                                if (!rk.GetValueNames().Contains(ctName))
                                {
                                    ctDelList += tempName + "\n";
                                }
                                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + ctName + ".tmp");
                            }
                            MessageBox.Show("Xóa thành công các Nhóm dịch sau:\n" + ctDelList, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi trong khi xóa các Nhóm dịch.\nVui lòng thử lại.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chưa chọn Nhóm dịch nào.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Lỗi trong khi thực hiện lệnh.\nVui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Xóa tất cả các Nhóm dịch?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
                try
                {
                    foreach (string value in rk.GetValueNames())
                    {
                        rk.DeleteValue(value, false);
                        System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(value) + ".tmp");
                    }
                    MessageBox.Show("Xóa tất cả Nhóm dịch thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }

                catch
                {
                    MessageBox.Show("Có lỗi trong khi xóa các Nhóm dịch. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
