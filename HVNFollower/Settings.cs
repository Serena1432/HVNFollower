using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HVNFollower
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings", numericUpDown1.Value.ToString());
            if (checkBox1.Checked == true)
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings", "1");
            }
            else
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings", "0");
            }
            if (checkBox2.Checked == true)
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings", "1");
            }
            else
            {
                System.IO.File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings", "0");
            }
            this.Close();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            numericUpDown1.Value = Convert.ToInt32(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings"));
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings"))
            {
                if (Convert.ToInt32(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings")) == 1)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
            }
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings"))
            {
                if (Convert.ToInt32(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings")) == 1)
                {
                    checkBox2.Checked = true;
                }
                else
                {
                    checkBox2.Checked = false;
                }
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(System.IO.File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings")) == 0)
            {
                if (checkBox2.Checked == true)
                {
                    if (MessageBox.Show("CẢNH BÁO!\nKể từ phiên bản 1.2, khi nhấn vào Thông báo đang hiện ở thanh tác vụ sẽ dẫn thẳng vào liên kết của Chủ thớt đó.\nTuy nhiên, chỉ bật tính năng này khi bạn chắc chắn rằng máy tính này là của bạn, và chỉ một mình bạn có thể mở máy tính hoặc app này.\nNếu có trường hợp gì xảy ra như việc người lớn trong gia đình bạn phát hiện rằng việc bạn có sử dụng phần mềm này và bạn phải chịu hậu quả nào đó thì LilShieru sẽ không chịu trách nhiệm.\nBạn có chắc chắn muốn bật nó không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        checkBox2.Checked = true;
                    }
                    else
                    {
                        checkBox2.Checked = false;
                    }
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
