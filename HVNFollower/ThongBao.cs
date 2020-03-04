using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using System.Net;

namespace HVNFollower
{
    public partial class ThongBao : Form
    {
        public ThongBao()
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
            InitializeComponent();
        }

        public class Notifications
        {
            public string time
            {
                get;
                set;
            }
            public string text
            {
                get;
                set;
            }
            public string href
            {
                get;
                set;
            }
            public bool isread
            {
                get;
                set;
            }
        }

        private void ThongBao_Load(object sender, EventArgs e)
        {
            try
            {
                List<Notifications> noti = JsonConvert.DeserializeObject<List<Notifications>>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp"));
                if (noti.Count() != 0 && noti[0].time != null)
                {
                    label1.Visible = false;
                    Panel[] panel = new Panel[noti.Count()];
                    Label[] timeLabel = new Label[noti.Count()];
                    Label[] textLabel = new Label[noti.Count()];
                    Button[] clickButton = new Button[noti.Count()];
                    int skipped_i = 0;
                    for (int i = 0; i < noti.Count(); i++)
                    {
                        panel[i] = new Panel();
                        panel[i].Location = new Point(13, 12 + 95 * i - skipped_i);
                        panel[i].Size = new Size(760, 92);
                        panel[i].BorderStyle = BorderStyle.FixedSingle;
                        if (noti[i].isread)
                        {
                            panel[i].BackColor = SystemColors.Control;
                        }
                        else
                        {
                            panel[i].BackColor = Color.Turquoise;
                        }
                        if (noti[i].time != null)
                        {
                            panel1.Controls.Add(panel[i]);
                        }
                        else
                        {
                            skipped_i++;
                        }
                        timeLabel[i] = new Label();
                        timeLabel[i].Location = new Point(12, 9);
                        timeLabel[i].AutoSize = true;
                        timeLabel[i].ForeColor = Color.Gray;
                        if (noti[i].isread)
                        {
                            timeLabel[i].Text = noti[i].time;
                        }
                        else
                        {
                            timeLabel[i].Text = noti[i].time + " (Chưa đọc)";
                        }
                        panel[i].Controls.Add(timeLabel[i]);
                        textLabel[i] = new Label();
                        textLabel[i].Location = new Point(12, 31);
                        textLabel[i].Size = new Size(732, 29);
                        textLabel[i].AutoSize = false;
                        textLabel[i].Text = noti[i].text;
                        panel[i].Controls.Add(textLabel[i]);
                        clickButton[i] = new Button();
                        clickButton[i].BackColor = Color.Transparent;
                        clickButton[i].Location = new Point(689, 63);
                        clickButton[i].Size = new Size(55, 24);
                        clickButton[i].Text = "Click";
                        clickButton[i].Name = noti[i].href;
                        clickButton[i].Click += new EventHandler(ThongBao_Click);
                        panel[i].Controls.Add(clickButton[i]);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Không thể tải được thông báo từ phía dữ liệu của máy.\nTất cả các thông báo hiện tại sẽ bị xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{}]");
                this.Close();
            }
            
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp"))
            {
                
            }
            else
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{}]");
            }
            string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp");
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", noti_text.Replace("false", "true"));
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", "0");
        }

        void ThongBao_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(((Button)sender).Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa tất cả thông báo không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{}]");
                    MessageBox.Show("Xóa tất cả thông báo thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                catch
                {
                    MessageBox.Show("Không thể xóa thông báo.\nVui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
