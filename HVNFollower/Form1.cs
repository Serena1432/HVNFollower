using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Threading.Tasks;
using System.Resources;
using Newtonsoft.Json;

namespace HVNFollower
{
    public partial class Form1 : Form
    {
        string v = "1.10.2";
        private static Form1 _instance;
        public int notiEnabled = 0;
        bool exit = false, noticed = false;
        string option = "chuthot", version;
        string ping = "Chưa có";
        bool pingNotified = false;
        long pingTimestamp;
        public Form1()
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
            _instance = this;
        }

        public class groupData
        {
            public string displayName
            {
                get;
                set;
            }
            public string comicLink
            {
                get;
                set;
            }
            public string firstComic
            {
                get;
                set;
            }
        }

        string img, name, hhimg, hh, tvnhom, gt, date, cmt, like, yen, mota, link;
        string currentUser, currentUserName, currentTacgia, currentUpdate, currentTacgiaName = "Không rõ", currentDoujin, currentDoujinName = "Không rõ", currentGroup, currentUserLink, currentTacgiaLink, currentDoujinLink, currentGroupLink, currentGroupName = "Không rõ";
        bool broken, checkip, checkedip = false, checkipfailed, notified, chuthot;
        string tac_gia_2, usr_string, tg_string, doujin_string, group_string;
        int i = -1, imax, itacgia = -1, itacgiamax = 0, idoujin = -1, idoujinmax = 0, igroup = -1, igroupmax;
        Random rnd = new Random();

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "HVN Follower v" + v + " by LilShieru";
            File.WriteAllBytes(Application.StartupPath + "\\HtmlAgilityPack.dll", Properties.Resources.HtmlAgilityPack);
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower");
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp"))
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{}]");
            }
            string temp_version = this.Text.Substring(this.Text.IndexOf("v") + 1);
            version = temp_version.Substring(0, temp_version.IndexOf("by") - 1);
            comboBox1.Text = "Thêm Chủ thớt";
            this.notifyIcon1.BalloonTipClicked += new EventHandler(notifyIcon1_BalloonTipClicked);
            this.notifyIcon1.DoubleClick += new EventHandler(notifyIcon1_DoubleClick);
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings"))
            {
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings", "0");
            }
                button1.Enabled = false;
                status.Text = "Đang kiểm tra việc check IP, vui lòng đợi một lát...";
            using (WebClient client = new WebClient())
            {
                if (!noticed)
                {
                    try
                    {
                        string s = client.DownloadString("https://hentaivn.tv/user-108808");
                        if (s.Contains("before accessing"))
                        {
                            if (!notified)
                            {
                                notifyIcon1.BalloonTipText = "Hiện tại HentaiVN đang yêu cầu check IP, vậy nên tiến trình cập nhật đã bị tạm dừng để tránh lỗi.";
                                notifyIcon1.ShowBalloonTip(10000);
                                notified = true;
                            }
                            checkipfailed = true;
                            button1.Enabled = true;
                        }
                        else
                        {
                            if (checkipfailed)
                            {
                                notifyIcon1.BalloonTipText = "Đã vượt qua việc check IP. Tiến trình cập nhật đã được tiếp tục.";
                                notifyIcon1.ShowBalloonTip(10000);
                                checkipfailed = false;
                            }
                            button1.Enabled = true;
                            timer2.Enabled = true;
                            checkedip = true;
                            status.Text = "Hiện tại bên HentaiVN không yêu cầu check IP. Bạn đã có thể tiếp tục.";
                        }
                    }
                    catch (Exception ex)
                    {
                        timer4.Enabled = false;
                        MessageBox.Show("Có lỗi đã xảy ra với HVN Follower. Vui lòng báo cáo cho LilShieru để tiếp tục.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            Process[] pname = Process.GetProcessesByName("HVN Follower");
            if (pname.Length > 1)
            {
                MessageBox.Show("HVN Follower hiện đang chạy. Bạn có thể mở HVN Follower trên Thanh tác vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                exit = true;
                Application.Exit();
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings"))
            {
                timer2.Interval = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings")) * 1000;
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings", "30");
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings"))
            {
                if (Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings")) == 1)
                {
                    notiEnabled = 1;
                }
                else
                {
                    notiEnabled = 0;
                }
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings", "1");
            }
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings", "0");
            }
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk.GetValueNames().Contains("HVNFollower"))
            {
                checkBox1.Checked = true;
                if (rk.GetValue("HVNFollower").ToString() != Application.ExecutablePath)
                {
                    rk.SetValue("HVNFollower", Application.ExecutablePath);
                    MessageBox.Show("Đã chỉnh sửa đường dẫn tự khởi động sai của HVN Follower thành đường dẫn hiện tại của app.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp"))
            {
                string noti = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp");
                button4.Text = "Thông báo (" + noti + ")";
                if (noti != "0")
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                    notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("noticeIcon")));
                    button4.ForeColor = Color.Red;
                    notifyIcon1.Text = "HVN Follower - " + noti + " thông báo mới";
                    notifyIcon1.BalloonTipText = "Bạn có " + noti + " thông báo mới chưa đọc.";
                    notifyIcon1.ShowBalloonTip(5000);
                }
                else
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                    notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
                    notifyIcon1.Text = "HVN Follower";
                }
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", "0");
            }
        }

        void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\ClickNotify.settings") == "1")
            {
                if (notifyIcon1.BalloonTipText.Contains("Chủ thớt"))
                {
                    System.Diagnostics.Process.Start("https://hentaivn.tv/user-" + currentUserLink);
                }
                else if (notifyIcon1.BalloonTipText.Contains("Tác giả"))
                {
                    System.Diagnostics.Process.Start("https://hentaivn.tv/tacgia=" + currentTacgiaLink + ".html");
                }
                else if (notifyIcon1.BalloonTipText.Contains("Doujinshi"))
                {
                    System.Diagnostics.Process.Start("https://hentaivn.tv/tim-kiem-doujinshi.html?key=" + currentDoujinLink);
                }
                else if (notifyIcon1.BalloonTipText.Contains("thông báo"))
                {
                    ThongBao frm = new ThongBao();
                    frm.Show();
                }
                else if (notifyIcon1.BalloonTipText.Contains("phiên bản mới"))
                {
                    System.Diagnostics.Process.Start("https://hentaivn.tv/forum/t33003-hvn-follower-app-theo-doi-cac-chu-thot-va-se-co-nhieu-thu-khac-tren-hentaivn.html");
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (checkBox1.Checked == true)
            {
                try
                {
                    rk.SetValue("HVNFollower", Application.ExecutablePath);
                }
                catch
                {
                    MessageBox.Show("Một lỗi không xác định đã xảy ra. Không thể cài đặt chương trình này vào danh sách tự động chạy.");
                    checkBox1.Checked = false;
                }
            }
            if (checkBox1.Checked == false)
            {
                try
                {
                    rk.DeleteValue("HVNFollower", false);   
                }
                catch
                {
                    checkBox1.Checked = true;
                }
            }
        }

        private void thoátHVNFollowerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            exit = true;
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!exit)
            {
                this.Hide();
                notifyIcon1.BalloonTipText = "HVN Follower đã được ẩn vào thanh tác vụ.";
                notifyIcon1.ShowBalloonTip(5000);
                e.Cancel = true;
            }
        }

        private void mởRộngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        public static string convertToUnSign3(string s)
        {
            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
        }  

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" && comboBox1.Text == "Thêm Chủ thớt")
            {
                status.Text = "Link không được để trống!";
            }
            else if (textBox1.Text == "" && comboBox1.Text == "Thêm Tác giả")
            {
                status.Text = "Tên Tác giả không được để trống!";
            }
            else if (textBox1.Text == "" && comboBox1.Text == "Thêm Doujinshi")
            {
                status.Text = "Tên Doujinshi không được để trống!";
            }
            else if (textBox1.Text.Contains("hentaivn.tv/forum/user-") && comboBox1.Text == "Thêm Chủ thớt")
            {
                status.Text = "Đang chuyển hướng sang link Chủ thớt bên Cổng truyện...";
                textBox1.Text = "https://hentaivn.tv/user-" + textBox1.Text.Substring(textBox1.Text.IndexOf("-") + 1);
                checkip = false;
                broken = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
                status.Text = "Đang khởi động...";
                status.Text = "Đang lấy thông tin...";
                webBrowser1.Navigate(textBox1.Text + "?rand=" + rnd.Next(100000000, 999999999));
                option = "chuthot";
            }
            else if (!textBox1.Text.Contains("hentaivn.tv/user-") && comboBox1.Text == "Thêm Chủ thớt")
            {
                status.Text = "Định dạng link không hợp lệ!";
            }
            else if (comboBox1.Text == "Thêm Chủ thớt")
            {
                broken = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
                checkip = false;
                status.Text = "Đang khởi động...";
                status.Text = "Đang lấy thông tin...";
                webBrowser1.Navigate(textBox1.Text + "?rand=" + rnd.Next(100000000, 999999999));
                option = "chuthot";
            }
            else if (comboBox1.Text == "Thêm Tác giả" && textBox1.Text.Contains("://"))
            {
                status.Text = "Tên Tác giả không thể là một đường link!";
            }
            else if (comboBox1.Text == "Thêm Tác giả")
            {
                string tac_gia = textBox1.Text.Replace(" ", "+");
                broken = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
                status.Text = "Đang khởi động...";
                status.Text = "Đang lấy thông tin...";
                webBrowser1.Navigate("https://hentaivn.tv/tacgia=" + tac_gia + ".html?rand=" + rnd.Next(100000000, 999999999));
                option = "tacgia";
            }
            else if (comboBox1.Text == "Thêm Doujinshi" && textBox1.Text.Contains("://"))
            {
                status.Text = "Tên Doujinshi không thể là một đường link!";
            }
            else if (comboBox1.Text == "Thêm Doujinshi")
            {
                string doujin = textBox1.Text.Replace(" ", "+");
                broken = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
                status.Text = "Đang khởi động...";
                status.Text = "Đang lấy thông tin...";
                webBrowser1.Navigate("https://hentaivn.tv/tim-kiem-doujinshi.html?key=" + Uri.EscapeDataString(doujin) + "&rand=" + rnd.Next(100000000, 999999999));
                option = "doujinshi";
            }
            else if (comboBox1.Text == "Thêm Nhóm dịch" && textBox1.Text.Contains("://"))
            {
                status.Text = "Tên Nhóm dịch không thể là một đường link!";
            }
            else if (comboBox1.Text == "Thêm Nhóm dịch")
            {
                string group = convertToUnSign3(textBox1.Text.Replace(" ", ""));
                broken = false;
                textBox1.Enabled = false;
                button1.Enabled = false;
                status.Text = "Đang khởi động...";
                status.Text = "Đang lấy thông tin...";
                var request = (HttpWebRequest)WebRequest.Create("http://kirarin2005.000webhostapp.com/HVNFollower/API/GetGroupInfo.php?id=" + group);

                request.Method = "GET";

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new System.IO.StreamReader(response.GetResponseStream()).ReadToEnd();

                try
                {
                    groupData info = JsonConvert.DeserializeObject<groupData>(responseString);
                    DialogResult mess = MessageBox.Show("Bạn có chắc chắn muốn thêm Nhóm dịch " + info.displayName + " vào Danh sách theo dõi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (mess == DialogResult.Yes)
                    {
                        try
                        {
                            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
                            if (rk.GetValueNames().Contains(convertToUnSign3(info.displayName.Replace(" ", ""))))
                            {
                                MessageBox.Show("Bạn đã thêm Nhóm dịch này rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                rk.SetValue(convertToUnSign3(info.displayName.Replace(" ", "")), info.displayName);
                                MessageBox.Show("Thêm Nhóm dịch thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        catch
                        {
                            MessageBox.Show("Thêm Nhóm dịch thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                    status.Text = "Sẵn sàng";
                    broken = true;
                }
                catch
                {
                    MessageBox.Show("Không thể lấy được thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                    status.Text = "Sẵn sàng";
                    broken = true;
                }
            }
        }

        void BrowserDocumentCompleted(object sender,
        WebBrowserDocumentCompletedEventArgs e)
        {
            if (e.Url.AbsolutePath != (sender as WebBrowser).Url.AbsolutePath)
                return;
            if (option == "chuthot")
            {
                mota = "";
                img = "";
                name = "";
                hhimg = "";
                hh = "";
                tvnhom = "";
                gt = "";
                date = "";
                cmt = "";
                like = "";
                yen = "";
                var divElement = webBrowser1.Document.GetElementsByTagName("div");
                foreach (HtmlElement info in divElement)
                {
                    if (info.GetAttribute("className") == "wall-avatar")
                    {
                        img = info.GetElementsByTagName("img")[0].GetAttribute("src");
                    }
                    if (info.GetAttribute("className") == "wall-name")
                    {
                        name = info.GetElementsByTagName("h2")[0].InnerText;
                        hhimg = info.GetElementsByTagName("img")[0].GetAttribute("src");
                        hh = info.GetElementsByTagName("p")[0].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Thành viên nhóm:") || info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Trưởng nhóm:"))
                    {
                        tvnhom = info.GetElementsByTagName("a")[0].InnerText;
                    }
                    if (tvnhom == "")
                    {
                        tvnhom = "Không có";
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Giới tính:"))
                    {
                        gt = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Gia nhập:"))
                    {
                        date = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Đã bình luận:"))
                    {
                        cmt = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Được thích:"))
                    {
                        like = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Yên:"))
                    {
                        yen = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    if (info.GetAttribute("className") == "info-row" && info.InnerText.Contains("Giới thiệu:"))
                    {
                        mota = info.GetElementsByTagName("div")[1].InnerText;
                    }
                    link = textBox1.Text;
                }
                if (!checkip)
                {
                    if (broken == false)
                    {
                        status.Text = "Lấy thông tin thành công!";
                        AddThisChuThot frm = new AddThisChuThot(img, name, hhimg, hh, tvnhom, gt, date, cmt, like, yen, mota, link);
                        frm.Show();
                    }
                }
                checkip = false;
            }
            else if (option == "tacgia")
            {
                tac_gia_2 = "";
                var h1Element = webBrowser1.Document.GetElementsByTagName("h1");
                var divElement = webBrowser1.Document.GetElementsByTagName("div");
                foreach (HtmlElement info in h1Element)
                {
                    if (info.GetAttribute("className") == "bar-title")
                    {
                        tac_gia_2 = info.InnerText.Substring(0, info.InnerText.IndexOf("Xế") - 1);
                        string tac_gia_3 = tac_gia_2.Substring(tac_gia_2.IndexOf("]") + 2);
                        foreach (HtmlElement info2 in divElement)
                        {
                            if (info2.GetAttribute("className") == "block-item")
                            {
                                if (info2.GetElementsByTagName("li")[0].InnerText == "Not found")
                                {
                                    status.Text = "Lấy thông tin thành công!";
                                    DialogResult mess = MessageBox.Show("Không thể tìm thấy Truyện hiện có trên trang của Tác giả " + tac_gia_3 + ".\nĐây có thể là một Tác giả không tồn tại, hoặc chưa có một truyện nào được tải lên từ tác giả này.\nNếu bạn tin rằng tác giả này có thực, bạn vẫn có thể kiểm tra lại tên của tác giả, nếu bạn chắc chắn nó đúng, bạn có thể thêm vào danh sách bằng cách nhấn vào nút Yes ở bên dưới.\nBạn có chắc chắn thêm Tác giả này vào danh sách theo dõi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    textBox1.Enabled = true;
                                    button1.Enabled = true;
                                    status.Text = "Sẵn sàng";
                                    broken = true;
                                }
                                else
                                {
                                    status.Text = "Lấy thông tin thành công!";
                                    DialogResult mess = MessageBox.Show("Bạn có chắc chắn muốn thêm Tác giả " + tac_gia_3 + " vào Danh sách theo dõi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (mess == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
                                            if (rk.GetValueNames().Contains(tac_gia_3.Replace(" ", "+")))
                                            {
                                                MessageBox.Show("Bạn đã thêm Tác giả này rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                rk.SetValue(tac_gia_3.Replace(" ", "+"), tac_gia_3);
                                                MessageBox.Show("Thêm Tác giả thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show("Thêm Tác giả thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    textBox1.Enabled = true;
                                    button1.Enabled = true;
                                    status.Text = "Sẵn sàng";
                                    broken = true;
                                }
                            }
                        }
                    }
                }
                if (tac_gia_2 == "")
                {
                    MessageBox.Show("Không thể lấy thông tin của Tác giả này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                    status.Text = "Không thể lấy được thông tin!";
                    broken = true;
                }
            }
            else if (option == "doujinshi")
            {
                tac_gia_2 = "";
                var h1Element = webBrowser1.Document.GetElementsByTagName("h1");
                var divElement = webBrowser1.Document.GetElementsByTagName("div");
                foreach (HtmlElement info in h1Element)
                {
                    if (info.GetAttribute("className") == "bar-title")
                    {
                        tac_gia_2 = info.InnerText.Substring(0, info.InnerText.IndexOf("Xế") - 1);
                        string tac_gia_3 = tac_gia_2.Substring(tac_gia_2.IndexOf("]") + 2);
                        foreach (HtmlElement info2 in divElement)
                        {
                            if (info2.GetAttribute("className") == "block-item")
                            {
                                if (info2.GetElementsByTagName("li")[0].InnerText == "Not found")
                                {
                                    status.Text = "Lấy thông tin thành công!";
                                    DialogResult mess = MessageBox.Show("Không thể tìm thấy Truyện hiện có trên trang của Doujinshi " + tac_gia_3 + ".\nĐây có thể là một Doujinshi không tồn tại, hoặc chưa có một truyện nào được tải lên từ Doujinshi này.\nNếu bạn tin rằng Doujinshi này có thực, bạn vẫn có thể kiểm tra lại tên của Doujinshi, nếu bạn chắc chắn nó đúng, bạn có thể thêm vào danh sách bằng cách nhấn vào nút Yes ở bên dưới.\nBạn có chắc chắn thêm Doujinshi này vào danh sách theo dõi không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    textBox1.Enabled = true;
                                    button1.Enabled = true;
                                    status.Text = "Sẵn sàng";
                                    broken = true;
                                }
                                else
                                {
                                    status.Text = "Lấy thông tin thành công!";
                                    DialogResult mess = MessageBox.Show("Bạn có chắc chắn muốn thêm Doujinshi " + tac_gia_3 + " vào Danh sách theo dõi?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (mess == DialogResult.Yes)
                                    {
                                        try
                                        {
                                            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
                                            if (rk.GetValueNames().Contains(tac_gia_3.Replace(" ", "+")))
                                            {
                                                MessageBox.Show("Bạn đã thêm Doujinshi này rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                            else
                                            {
                                                rk.SetValue(tac_gia_3.Replace(" ", "+"), tac_gia_3);
                                                MessageBox.Show("Thêm Doujinshi thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        catch
                                        {
                                            MessageBox.Show("Thêm Doujinshi thất bại, vui lòng thử lại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    textBox1.Enabled = true;
                                    button1.Enabled = true;
                                    status.Text = "Sẵn sàng";
                                    broken = true;
                                }
                            }
                        }
                    }
                }
                if (tac_gia_2 == "")
                {
                    MessageBox.Show("Không thể lấy thông tin của Doujinshi này!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Enabled = true;
                    button1.Enabled = true;
                    status.Text = "Không thể lấy được thông tin!";
                    broken = true;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            RegistryKey rk = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (rk.GetValueNames().Contains("HVNFollower"))
            {
                this.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DsChuThot frm = new DsChuThot();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Settings frm = new Settings();
            frm.Show();
        }

        string GetLine(string text, int lineNo)
        {
            string[] lines = text.Replace("\r", "").Split('\n');
            return lines.Length >= lineNo ? lines[lineNo - 1] : null;
        }

        private void GetUsrData(string currentUser, string currentUserName)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    usr_string = client.DownloadString("https://hentaivn.tv/user-" + currentUser + "?rand=" + rnd.Next(100000000, 999999999));
                    long pingTime = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond) - pingTimestamp;
                    ping = pingTime.ToString() + "ms";
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(usr_string);
                    var itemList = doc.DocumentNode.SelectNodes("//div[@class='box-description']//h2")
                                  .Select(p => p.InnerText)
                                  .ToList();
                    if (!usr_string.Contains("Tường nhà") && !usr_string.Contains("Thông tin cá nhân"))
                    {
                        notifyIcon1.BalloonTipText = "Có vẻ như web HVN đang gặp lỗi, vì vậy nên tiến trình cập nhật Chủ thớt hiện tại đã bị bỏ qua để tránh lỗi.";
                        notifyIcon1.ShowBalloonTip(5000);
                    }
                    else
                    {
                        string truyen_list = "Không rõ";
                        truyen_list = GetLine(itemList[0].ToString(), 2);
                        var img = doc.DocumentNode.SelectSingleNode("//div[@class='wall-avatar']//img[@src]");
                        string src = img.GetAttributeValue("src", "nothing");
                        if (!src.Contains("https://"))
                        {
                            src = "https://hentaivn.tv" + src;
                        }
                        var newUserName = doc.DocumentNode.SelectNodes("//div[@class='wall-name']//h2").Select(p => p.InnerText).ToList()[0];
                        try
                        {
                            Stream stream = client.OpenRead(src); Bitmap bitmap; bitmap = new Bitmap(stream);
                            Bitmap bmpOutput = ResizeImage(bitmap, 16, 16);
                            bmpOutput.Save(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + "_img.tmp", System.Drawing.Imaging.ImageFormat.Icon);
                        }
                        catch
                        {
                        }
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + ".tmp"))
                        {
                            if (truyen_list != File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + ".tmp"))
                            {
                                currentUserLink = currentUser;
                                if (notiEnabled == 1)
                                {
                                    notifyIcon1.BalloonTipText = "Chủ thớt " + currentUserName + " vừa ra truyện mới \'" + truyen_list + "\', vào xem nhanh kẻo muộn nào!";
                                    notifyIcon1.ShowBalloonTip(10000);
                                }
                                string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"Chủ thớt " + currentUserName + " vừa ra truyện mới \'" + truyen_list + "\', vào xem nhanh kẻo muộn nào!\",\"href\":\"https://hentaivn.tv/user-" + currentUserLink + "\",\"isread\":false},{" + noti_text);
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + ".tmp", truyen_list);
                                int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + ".tmp", truyen_list);
                        }
                        if (newUserName != currentUserName)
                        {
                            currentUserLink = currentUser;
                            if (notiEnabled == 1)
                            {
                                notifyIcon1.BalloonTipText = "Chủ thớt " + currentUserName + " đã đổi tên hiển thị thành " + newUserName + ", hãy ghi nhớ để tránh nhầm lẫn về sau!";
                                notifyIcon1.ShowBalloonTip(10000);
                            }
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentUser + ".tmp", truyen_list);
                            string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"Chủ thớt " + currentUserName + " đã đổi tên hiển thị thành " + newUserName + ", hãy ghi nhớ để tránh nhầm lẫn về sau!\",\"href\":\"https://hentaivn.tv/user-" + currentUserLink + "\",\"isread\":false},{" + noti_text);
                            int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            RegistryKey rkusr = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
                            rkusr.SetValue(currentUser, newUserName);
                        }
                    }
                }
                catch (Exception e)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    MessageBox.Show("Không thể kết nối đến máy chủ HentaiVN.\nHãy kiểm tra lại kết nối mạng, đường truyền mạng và thử vào HentaiVN trên một trình duyệt khác xem nó có hoạt động tốt hay không.\nNếu mạng vẫn hoạt động bình thường nhưng lỗi này vẫn hiển thị, hãy thông báo cho LilShieru biết.\n\nThông tin lỗi:\n" + e.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exit = true;
                    Application.Exit();
                }
            }
        }

        private Task GetUsrDataAsync(string currentUser, string currentUserName)
        {
            return Task.Factory.StartNew(() => GetUsrData(currentUser, currentUserName));
        }

        private void UpdateDoujin(string currentDoujin, string currentDoujinName)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    doujin_string = client.DownloadString("https://hentaivn.tv/tim-kiem-doujinshi.html?key=" + currentDoujin + "&rand=" + rnd.Next(100000000, 999999999));
                    long pingTime = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond) - pingTimestamp;
                    ping = pingTime.ToString() + "ms";
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(doujin_string);
                    var itemList = doc.DocumentNode.SelectNodes("//div[@class='box-description']")
                                  .Select(p => p.InnerText)
                                  .ToList();
                    if (!doujin_string.Contains("Danh sách truyện:"))
                    {
                        notifyIcon1.BalloonTipText = "Có vẻ như web HVN đang gặp lỗi, vì vậy nên tiến trình cập nhật Tác giả hiện tại đã bị bỏ qua để tránh lỗi.";
                        notifyIcon1.ShowBalloonTip(5000);
                    }
                    else
                    {
                        string doujin_truyen_list = "Không rõ";
                        doujin_truyen_list = GetLine(itemList[0].ToString(), 3);
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentDoujin) + ".tmp"))
                        {
                            if (doujin_truyen_list != File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentDoujin) + ".tmp"))
                            {
                                currentDoujinLink = currentDoujin;
                                if (notiEnabled == 1)
                                {
                                    notifyIcon1.BalloonTipText = "Doujinshi " + currentDoujinName + " vừa có truyện mới \'" + doujin_truyen_list + "\', vào xem nhanh kẻo muộn nào!";
                                    notifyIcon1.ShowBalloonTip(10000);
                                }
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentDoujin) + ".tmp", doujin_truyen_list);
                                string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"Doujinshi " + currentDoujinName + " vừa có truyện mới \'" + doujin_truyen_list + "\', vào xem nhanh kẻo muộn nào!\",\"href\":\"https://hentaivn.tv/tim-kiem-doujinshi.html?key=" + Uri.EscapeDataString(currentDoujinLink) + "\",\"isread\":false},{" + noti_text);
                                int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentDoujin) + ".tmp", doujin_truyen_list);
                        }
                    }
                }
                catch (Exception e)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    MessageBox.Show("Không thể kết nối đến máy chủ HentaiVN.\nHãy kiểm tra lại kết nối mạng, đường truyền mạng và thử vào HentaiVN trên một trình duyệt khác xem nó có hoạt động tốt hay không.\nNếu mạng vẫn hoạt động bình thường nhưng lỗi này vẫn hiển thị, hãy thông báo cho LilShieru biết.\n\nThông tin lỗi:\n" + e.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exit = true;
                    Application.Exit();
                }
            }
        }

        private Task UpdateDoujinAsync(string currentDoujin, string currentDoujinName)
        {
            return Task.Factory.StartNew(() => UpdateDoujin(currentDoujin, currentDoujinName));
        }

        private void UpdateGroup(string currentGroup, string currentGroupName)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    group_string = client.DownloadString("https://hentaivn.tv/g/" + currentGroup);
                    long pingTime = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond) - pingTimestamp;
                    ping = pingTime.ToString() + "ms";
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(group_string);
                    var itemList = doc.DocumentNode.SelectNodes("//div[@class='box-description']")
                                  .Select(p => p.InnerText)
                                  .ToList();
                    if (!group_string.Contains("Danh sách truyện:"))
                    {
                        notifyIcon1.BalloonTipText = "Có vẻ như web HVN đang gặp lỗi, vì vậy nên tiến trình cập nhật Tác giả hiện tại đã bị bỏ qua để tránh lỗi.";
                        notifyIcon1.ShowBalloonTip(5000);
                    }
                    else
                    {
                        string group_truyen_list = "Không rõ";
                        group_truyen_list = GetLine(itemList[0].ToString(), 3);
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentGroup) + ".tmp"))
                        {
                            if (group_truyen_list != File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentGroup) + ".tmp"))
                            {
                                currentGroupLink = currentGroup;
                                if (notiEnabled == 1)
                                {
                                    notifyIcon1.BalloonTipText = "Nhóm dịch " + currentGroupName + " vừa có truyện mới \'" + group_truyen_list + "\', vào xem nhanh kẻo muộn nào!";
                                    notifyIcon1.ShowBalloonTip(10000);
                                }
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentGroup) + ".tmp", group_truyen_list);
                                string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"Nhóm dịch " + currentGroupName + " vừa có truyện mới \'" + group_truyen_list + "\', vào xem nhanh kẻo muộn nào!\",\"href\":\"https://hentaivn.tv/tim-kiem-Nhóm dịch.html?key=" + Uri.EscapeDataString(currentGroupLink) + "\",\"isread\":false},{" + noti_text);
                                int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + Uri.EscapeDataString(currentGroup) + ".tmp", group_truyen_list);
                        }
                    }
                }
                catch (Exception e)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    MessageBox.Show("Không thể kết nối đến máy chủ HentaiVN.\nHãy kiểm tra lại kết nối mạng, đường truyền mạng và thử vào HentaiVN trên một trình duyệt khác xem nó có hoạt động tốt hay không.\nNếu mạng vẫn hoạt động bình thường nhưng lỗi này vẫn hiển thị, hãy thông báo cho LilShieru biết.\n\nThông tin lỗi:\n" + e.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exit = true;
                    Application.Exit();
                }
            }
        }

        private Task UpdateGroupAsync(string currentGroup, string currentGroupName)
        {
            return Task.Factory.StartNew(() => UpdateGroup(currentGroup, currentGroupName));
        }

        private void UpdateAuthor(string currentTacgia, string currentTacgiaName)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                try
                {
                    tg_string = client.DownloadString("https://hentaivn.tv/tacgia=" + currentTacgia + ".html" + "?rand=" + rnd.Next(100000000, 999999999));
                    long pingTime = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond) - pingTimestamp;
                    ping = pingTime.ToString() + "ms";
                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(tg_string);
                    var itemList = doc.DocumentNode.SelectNodes("//div[@class='box-description']")
                                  .Select(p => p.InnerText)
                                  .ToList();
                    if (!tg_string.Contains("Danh sách truyện:"))
                    {
                        notifyIcon1.BalloonTipText = "Có vẻ như web HVN đang gặp lỗi, vì vậy nên tiến trình cập nhật Tác giả hiện tại đã bị bỏ qua để tránh lỗi.";
                        notifyIcon1.ShowBalloonTip(5000);
                    }
                    else
                    {
                        string tg_truyen_list = "Không rõ";
                        tg_truyen_list = GetLine(itemList[0].ToString(), 3);
                        if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentTacgia + ".tmp"))
                        {
                            if (tg_truyen_list != File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentTacgia + ".tmp"))
                            {
                                currentTacgiaLink = currentTacgia;
                                if (notiEnabled == 1)
                                {
                                    notifyIcon1.BalloonTipText = "Tác giả " + currentTacgiaName + " vừa ra truyện mới \'" + tg_truyen_list + "\', vào xem nhanh kẻo muộn nào!";
                                    notifyIcon1.ShowBalloonTip(10000);
                                }
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentTacgia + ".tmp", tg_truyen_list);
                                string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"Tác giả " + currentTacgiaName + " vừa ra truyện mới \'" + tg_truyen_list + "\', vào xem nhanh kẻo muộn nào!\",\"href\":\"https://hentaivn.tv/tacgia=" + currentTacgiaLink + ".html\",\"isread\":false},{" + noti_text);
                                int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            }
                        }
                        else
                        {
                            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\" + currentTacgia + ".tmp", tg_truyen_list);
                        }
                    }
                }
                catch (Exception e)
                {
                    timer1.Enabled = false;
                    timer2.Enabled = false;
                    timer3.Enabled = false;
                    timer4.Enabled = false;
                    MessageBox.Show("Không thể kết nối đến máy chủ HentaiVN.\nHãy kiểm tra lại kết nối mạng, đường truyền mạng và thử vào HentaiVN trên một trình duyệt khác xem nó có hoạt động tốt hay không.\nNếu mạng vẫn hoạt động bình thường nhưng lỗi này vẫn hiển thị, hãy thông báo cho LilShieru biết.\n\nThông tin lỗi:\n" + e.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    exit = true;
                    Application.Exit();
                }
            }
        }

        private Task UpdateAuthorAsync(string currentTacgia, string currentTacgiaName)
        {
            return Task.Factory.StartNew(() => UpdateAuthor(currentTacgia, currentTacgiaName));
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pingTimestamp = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).Ticks / TimeSpan.TicksPerMillisecond;
            this.Text = "HVN Follower v" + v + " by LilShieru | Ping: " + ping;
            if (ping.Contains("ms") && Convert.ToInt32(ping.Substring(0, ping.Length - 2)) > timer2.Interval && !pingNotified)
            {
                notifyIcon1.BalloonTipText = "Ping từ máy chủ HentaiVN đang lớn hơn thời gian cập nhật của app, điều này có thể gây ra lỗi. Vui lòng thay đổi lại thời gian cập nhật.";
                notifyIcon1.ShowBalloonTip(10000);
                pingNotified = true;
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings"))
            {
                timer2.Interval = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings")) * 1000;
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\TimerInterval.settings", "30");
            }
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings"))
            {
                if (Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings")) == 1)
                {
                    notiEnabled = 1;
                }
                else
                {
                    notiEnabled = 0;
                }
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\NotificationEnabled.settings", "1");
            }
            RegistryKey rk = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower");
            RegistryKey rk4 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Groups");
            if (rk.GetValueNames().Count() != 0 && rk4.GetValueNames().Count() != 0)
            {
                igroupmax = rk4.GetValueNames().Count();
                imax = rk.GetValueNames().Count();
                if (i < imax)
                {
                    i++;
                    chuthot = true;
                }
                if (i == imax && igroup < igroupmax)
                {
                    i--;
                    igroup++;
                    chuthot = false;
                }
                if (i == imax - 1 && igroup == igroupmax)
                {
                    i = 0; igroup = -1;
                    chuthot = true;
                }
                if (chuthot)
                {
                    currentUser = rk.GetValueNames()[i].ToString();
                    currentUserName = rk.GetValue(rk.GetValueNames()[i].ToString(), "Không rõ").ToString();
                    GetUsrDataAsync(currentUser, currentUserName);
                }
                else
                {
                    currentGroup = rk4.GetValueNames()[igroup].ToString();
                    currentGroupName = rk4.GetValue(rk4.GetValueNames()[igroup].ToString(), "Không rõ").ToString();
                    UpdateGroupAsync(currentGroup, currentGroupName);
                }
            }
            else if (rk.GetValueNames().Count() != 0 && rk4.GetValueNames().Count() == 0)
            {
                imax = rk.GetValueNames().Count();
                if (i < imax)
                {
                    i++;
                }
                if (i == imax)
                {
                    i = 0;
                }
                    currentUser = rk.GetValueNames()[i].ToString();
                    currentUserName = rk.GetValue(rk.GetValueNames()[i].ToString(), "Không rõ").ToString();
                    GetUsrDataAsync(currentUser, currentUserName);
            }
            
            RegistryKey rk2 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Authors");
            RegistryKey rk3 = Registry.CurrentUser.CreateSubKey("SOFTWARE\\LilShieru\\HVNFollower\\Doujins");
            int tg_doujin = rk2.GetValueNames().Count() + rk3.GetValueNames().Count();
            if (rk2.GetValueNames().Count() != 0 && rk.GetValueNames().Count() != 0 && rk3.GetValueNames().Count() != 0 && rk4.GetValueNames().Count() != 0)
            {
                itacgiamax = rk2.GetValueNames().Count() + rk3.GetValueNames().Count();
                itacgia++;
                if (itacgia <= itacgiamax - 1 - rk3.GetValueNames().Count())
                {
                    currentTacgia = rk2.GetValueNames()[itacgia].ToString();
                    currentTacgiaName = currentTacgia.Replace("+", " ");
                    if (chuthot)
                    {
                        status.Text = "Đang cập nhật Chủ thớt " + currentUserName + " và Tác giả " + currentTacgiaName + "...";
                    }
                    else
                    {
                        status.Text = "Đang cập nhật Nhóm dịch " + currentGroupName + " và Tác giả " + currentTacgiaName + "...";
                    }
                    UpdateAuthorAsync(currentTacgia, currentTacgiaName);
                    
                }
                else if (itacgia >= itacgiamax - 1 - rk3.GetValueNames().Count())
                {
                    idoujin = itacgia - rk2.GetValueNames().Count();
                    currentDoujin = rk3.GetValueNames()[idoujin].ToString();
                    currentDoujinName = currentDoujin.Replace("+", " ");
                    if (chuthot)
                    {
                        status.Text = "Đang cập nhật Chủ thớt " + currentUserName + " và Doujinshi " + currentDoujinName + "...";
                    }
                    else
                    {
                        status.Text = "Đang cập nhật Nhóm dịch " + currentGroupName + " và Doujinshi " + currentDoujinName + "...";
                    }
                    UpdateDoujinAsync(currentDoujin, currentDoujinName);
                }
                if (itacgia == itacgiamax - 1)
                {
                    itacgia = -1;
                }

            }
            else if (rk2.GetValueNames().Count() != 0 && rk.GetValueNames().Count() != 0 && rk3.GetValueNames().Count() != 0 && rk4.GetValueNames().Count() == 0)
            {
                itacgiamax = rk2.GetValueNames().Count() + rk3.GetValueNames().Count();
                itacgia++;
                if (itacgia <= itacgiamax - 1 - rk3.GetValueNames().Count())
                {
                    currentTacgia = rk2.GetValueNames()[itacgia].ToString();
                    currentTacgiaName = currentTacgia.Replace("+", " ");
                        status.Text = "Đang cập nhật Chủ thớt " + currentUserName + " và Tác giả " + currentTacgiaName + "...";
                    UpdateAuthorAsync(currentTacgia, currentTacgiaName);

                }
                else if (itacgia >= itacgiamax - 1 - rk3.GetValueNames().Count())
                {
                    idoujin = itacgia - rk2.GetValueNames().Count();
                    currentDoujin = rk3.GetValueNames()[idoujin].ToString();
                    currentDoujinName = currentDoujin.Replace("+", " ");
                        status.Text = "Đang cập nhật Chủ thớt " + currentUserName + " và Doujinshi " + currentDoujinName + "...";
                    UpdateDoujinAsync(currentDoujin, currentDoujinName);
                }
                if (itacgia == itacgiamax - 1)
                {
                    itacgia = -1;
                }

            }
            else if (rk.GetValueNames().Count() == 0 && rk2.GetValueNames().Count() == 0 && rk3.GetValueNames().Count() != 0)
            {
                idoujinmax = rk3.GetValueNames().Count();
                idoujin++;
                if (idoujin <= idoujinmax - 1)
                {
                    currentDoujin = rk3.GetValueNames()[idoujin].ToString();
                    currentDoujinName = currentDoujin.Replace("+", " ");
                    UpdateDoujinAsync(currentDoujin, currentDoujinName);
                }
                if (idoujin == idoujinmax - 1)
                {
                    idoujin = -1;
                }

            }
            else if (rk2.GetValueNames().Count() == 0 && rk.GetValueNames().Count() != 0 && rk3.GetValueNames().Count() == 0)
            {
                itacgia = -1;
            }
            else if (rk.GetValueNames().Count() == 0 && rk2.GetValueNames().Count() != 0 && rk3.GetValueNames().Count() == 0)
            {
                i = -1;
            }
            else if (rk.GetValueNames().Count() == 0 && rk2.GetValueNames().Count() == 0 && rk3.GetValueNames().Count() > 0)
            {
                i = -1; itacgia = -1;
            }
            else if (rk.GetValueNames().Count() == 0 && rk2.GetValueNames().Count() == 0 && rk3.GetValueNames().Count() == 0)
            {
                i = -1; itacgia = -1; idoujin = -1;
            }
            else if (rk.GetValueNames().Count() == 0 && rk2.GetValueNames().Count() == 0 && rk3.GetValueNames().Count() == 0 && rk4.GetValueNames().Count() == 0)
            {
                status.Text = "Không có Chủ thớt, Tác giả, Doujinshi hay Nhóm dịch nào!";
            }
        }

        public Bitmap ResizeImage(Bitmap b, int nWidth, int nHeight)
        {
            Bitmap result = new Bitmap(nWidth, nHeight);

            using (Graphics g = Graphics.FromImage((Image)result))
                g.DrawImage(b, 0, 0, nWidth, nHeight);

            return result;
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Thêm Chủ thớt")
            {
                textBox1.Text = "https://hentaivn.tv/user-108808";
            }
            else if (comboBox1.Text == "Thêm Tác giả")
            {
                textBox1.Text = "Fummy";
            }
            else if (comboBox1.Text == "Thêm Doujinshi")
            {
                textBox1.Text = "Love Live!";
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ThongBao frm = new ThongBao();
            frm.Show();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp"))
            {
                string noti = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp");
                button4.Text = "Thông báo (" + noti + ")";
                thôngBáoToolStripMenuItem.Text = "Thông báo (" + noti + ")";
                if (noti != "0")
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                    notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("noticeIcon")));
                    button4.ForeColor = Color.Red;
                    thôngBáoToolStripMenuItem.ForeColor = Color.Red;
                    notifyIcon1.Text = "HVN Follower - " + noti + " thông báo mới";
                }
                else
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
                    notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
                    button4.ForeColor = Color.Black;
                    thôngBáoToolStripMenuItem.ForeColor = Color.Black;
                    notifyIcon1.Text = "HVN Follower";
                }
            }
            else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\");
                File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", "0");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Thêm Chủ thớt")
            {
                label1.Text = "Nhập Link User của chủ thớt (https://hentaivn.tv/user-xxxxx):";
            }
            else if (comboBox1.Text == "Thêm Tác giả")
            {
                label1.Text = "Nhập tên của Tác giả muốn tìm:";
            }
            else if (comboBox1.Text == "Thêm Doujinshi")
            {
                label1.Text = "Nhập tên của Doujinshi muốn tìm:";
            }
            else if (comboBox1.Text == "Thêm Nhóm dịch")
            {
                label1.Text = "Nhập tên của Nhóm dịch muốn tìm:";
            }
        }

        public static void No()
        {
            _instance.textBox1.Enabled = true;
            _instance.status.Text = "Sẵn sàng";
            _instance.button1.Enabled = true;
        }


        private void timer4_Tick(object sender, EventArgs e)
        {
            using (WebClient client = new WebClient())
            {
                if (!checkedip)
                {
                    if (!noticed)
                    {
                        try
                        {
                            string s = client.DownloadString("https://hentaivn.tv/user-108808");
                            if (s.Contains("before accessing"))
                            {
                                if (!notified)
                                {
                                    notifyIcon1.BalloonTipText = "Hiện tại HentaiVN đang yêu cầu check IP, vậy nên tiến trình cập nhật đã bị tạm dừng để tránh lỗi.";
                                    notifyIcon1.ShowBalloonTip(10000);
                                    notified = true;
                                }
                                checkipfailed = true;
                                button1.Enabled = true;
                            }
                            else
                            {
                                if (checkipfailed)
                                {
                                    notifyIcon1.BalloonTipText = "Đã vượt qua việc check IP. Tiến trình cập nhật đã được tiếp tục.";
                                    notifyIcon1.ShowBalloonTip(10000);
                                    checkipfailed = false;
                                }
                                button1.Enabled = true;
                                timer2.Enabled = true;
                                checkedip = true;
                                status.Text = "Hiện tại bên HentaiVN không yêu cầu check IP. Bạn đã có thể tiếp tục.";
                            }
                        }
                        catch (Exception ex)
                        {
                            timer4.Enabled = false;
                            MessageBox.Show("Có lỗi đã xảy ra với HVN Follower. Vui lòng báo cáo cho LilShieru để tiếp tục.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            using (WebClient client = new WebClient())
            {
                if (!noticed)
                {
                    try
                    {
                        string s = client.DownloadString("http://ichika.shiru2005.tk/HVNFollower/LatestVersion.txt");
                        if (s != version)
                        {
                            string noti_text = File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp").Substring(2);
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\History.tmp", "[{\"time\":\"" + DateTime.Now.ToString() + "\",\"text\":\"HVN Follower đã có phiên bản mới, vào trang của app tại forum HVN để cập nhật nhé!\",\"href\":\"https://hentaivn.tv/forum/t33003-hvn-follower-app-theo-doi-cac-chu-thot-va-se-co-nhieu-thu-khac-tren-hentaivn.html\",\"isread\":false},{" + noti_text);
                            notifyIcon1.BalloonTipText = "HVN Follower đã có phiên bản mới, vào trang của app tại forum HVN để cập nhật nhé!";
                            notifyIcon1.ShowBalloonTip(5000);
                            int notis = Convert.ToInt32(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp")) + 1;
                            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\LilShieru\\HVNFollower\\Notifications.tmp", notis.ToString());
                            noticed = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        timer4.Enabled = false;
                        MessageBox.Show("Có lỗi đã xảy ra với HVN Follower. Vui lòng báo cáo cho LilShieru để tiếp tục.\n\nThông tin lỗi:\n" + ex.ToString(), "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
