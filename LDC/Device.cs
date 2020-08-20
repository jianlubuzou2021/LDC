using NDevHelper.Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LDC
{
    class Device : Operation
    {
        public string EmulatorName { set; get; } = "UNKNOW";
        public string PhoneNumber { set; get; } = "UNKNOW";

        private int ListPosition = -1;
        private string HomePath = "";
        private string DataPath = "";

        private ListViewItem item;

        private Process P;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emulatorName">设备名</param>
        /// <param name="phoneNumber">设备名对应的手机号</param>
        /// <param name="listPosition">再列表中对应的索引位置</param>
        public Device(string emulatorName, int listPosition, ListViewItem item)
        {
            EmulatorName = emulatorName;
            ListPosition = listPosition;
            InitADBShell();
            this.item = item;
            this.item.ForeColor = Color.Green;
            HomePath = AppDomain.CurrentDomain.BaseDirectory;
            Xlog.I(emulatorName, "HomePath: {0}", HomePath);
            Actions.UpdateState(ListPosition, "已初始化连接！");
        }

        /// <summary>
        /// 初始化ADB
        /// </summary>
        private void InitADBShell()
        {
            P = new Process()
            {
                StartInfo =
                {
                    FileName = "cmd",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            P.ErrorDataReceived += ErrReceived;
            P.Start();
            P.StandardInput.AutoFlush = true;
            P.BeginErrorReadLine();
            ReadLines(false);
        }

        /// <summary>
        /// 控制台错误委托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ErrReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Xlog.E(EmulatorName, "{0}", e.Data);
            }
        }

        /// <summary>
        /// 读取当前流内的所有行，尾行总是为0
        /// </summary>
        /// <returns></returns>
        private List<string> ReadLines(bool NeedLog)
        {
            List<string> lines = new List<string>();
            string line;
            int counter = 1;
            try
            {
                do
                {
                    line = P.StandardOutput.ReadLine();
                    lines.Add(line);
                    if (NeedLog)
                    {
                        Xlog.I(EmulatorName, "{0}: {1}", counter++, line);
                    }
                } while (!line.Equals(string.Empty) && !line.EndsWith("}"));
            }
            catch (Exception ex)
            {
                Xlog.E(EmulatorName, "读取流时发生错误，{0}", ex.Message);
            }
            return lines;
        }

        /// <summary>
        /// 关闭钱包
        /// </summary>
        public void CloseBtd()
        {
            Actions.UpdateState(ListPosition, "正准备关闭BTD！");
            new Thread(new ThreadStart(this._CloseBtd)).Start();
            Actions.UpdateState(ListPosition, "BTD已全部关闭！");
        }

        private void _CloseBtd()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell am force-stop com.btd.wallet", EmulatorName));
            ReadLines(true);
        }

        /// <summary>
        /// 查询钱包PID
        /// </summary>
        /// <returns></returns>
        public List<string> FindBtdWalletPid()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell ps", EmulatorName));
            List<string> pid = new List<string>(2);
            List<string> T = ReadLines(true);
            foreach (var item in T)
            {
                if (item.Contains("btd"))
                {
                    pid.Add(item.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                }
            }
            return pid;
        }

        /// <summary>
        /// 关闭ADB，并释放资源
        /// </summary>
        public void Dispose()
        {
            P.StandardInput.WriteLine("Exit");
            P.Close();
            Actions.UpdateState(ListPosition, "连接已释放！");
        }

        /// <summary>
        /// 清空应用应用数据，强烈建议再关闭应用后，立即删除，否则可能无效
        /// </summary>
        public void CleanData()
        {
            Actions.UpdateState(ListPosition, "正在清理应用数据！");
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/db.tar", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/sp.tar", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/bwuu.txt", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /sdcard/Android/data/com.btd.wallet", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/app_accs", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/app_e_qq_com*", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/cache", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/code_cache", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/files", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/databases", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/shared_prefs", EmulatorName));
            ReadLines(true);
            Thread.Sleep(100);
            Actions.UpdatePN(ListPosition, "");
            Actions.UpdateState(ListPosition, "应用数据清理结束！");
        }

        /// <summary>
        /// 拉取数据
        /// </summary>
        public void FetchData()
        {
            try
            {
                Actions.UpdateState(ListPosition, "正准备读取手机号码！");
                string pn = ReadPhoneNumber();// 读取最后登陆的手机号码
                Actions.UpdateState(ListPosition, string.Format("读取到的号码为：{0}", pn));
                if ("".Equals(pn))
                {
                    Actions.UpdateState(ListPosition, "未检测到已登录的号码！是否数据已经清理或者未登录！");
                    return;
                }
                else
                {
                    PhoneNumber = pn;
                    Actions.UpdatePN(ListPosition, PhoneNumber);
                    Actions.UpdateState(ListPosition, "正在构建用户目录！");
                    DataPath = string.Format("{0}{1}", HomePath, PhoneNumber);
                    if (!Directory.Exists(DataPath))
                    {
                        Directory.CreateDirectory(DataPath);
                        Xlog.I(EmulatorName, "Created DataDir: {0}", DataPath);
                    }
                }

                Actions.UpdateState(ListPosition, "正准备清理历史数据 db.tar！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/db.tar", EmulatorName));
                ReadLines(true);
                Actions.UpdateState(ListPosition, "正准备清理历史数据 sp.tar！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/sp.tar", EmulatorName));
                ReadLines(true);
                Actions.UpdateState(ListPosition, "正准备打包数据库！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} shell tar -cvf sdcard/db.tar /data/data/com.btd.wallet/databases", EmulatorName));
                ReadLines(true);
                Actions.UpdateState(ListPosition, "正准备打包配置文件！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} shell tar -cvf sdcard/sp.tar /data/data/com.btd.wallet/shared_prefs", EmulatorName));
                ReadLines(true);
                Actions.UpdateState(ListPosition, "正准备拷贝 db.tar 到本地！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} pull /sdcard/db.tar {1}", EmulatorName, DataPath));
                ReadLines(true);
                Actions.UpdateState(ListPosition, "正准备拷贝 sp.tar 到本地！");
                P.StandardInput.WriteLine(string.Format("adb -s {0} pull /sdcard/sp.tar {1}", EmulatorName, DataPath));
                ReadLines(true);

                Actions.UpdateState(ListPosition, "正准备读取DeviceKey！");
                string key = ReadDeviceKey();// 读取deviceKey
                Actions.UpdateState(ListPosition, string.Format("读取到的DeviceKey为：{0}", key));
                if (File.Exists(string.Format("{0}\\{1}.txt", DataPath, PhoneNumber)))
                {
                    File.Delete(string.Format("{0}\\{1}.txt", DataPath, PhoneNumber));
                }
                if (!"".Equals(key))
                {
                    File.AppendAllText(string.Format("{0}\\{1}.txt", DataPath, PhoneNumber), key + "\r\n");
                }
                Actions.UpdateState(ListPosition, "正准备读取Cookie！");
                string cookie = ReadCookie(); // 读取cookie
                Actions.UpdateState(ListPosition, string.Format("DeviceKey: {0}, Cookie: {1}", key, cookie));
                if (!"".Equals(cookie))
                {
                    File.AppendAllText(string.Format("{0}\\{1}.txt", DataPath, PhoneNumber), cookie);
                }
            }
            catch (Exception ex)
            {
                Xlog.E(EmulatorName, "拉取数据发生错误！{0}", ex.Message);
            }
        }

        public void PushData(Bind bind)
        {
            Actions.UpdateFileState(bind.FP, "正在清理旧的数据！");
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/sp.tar", EmulatorName));
            ReadLines(true);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/db.tar", EmulatorName));
            ReadLines(true);
            P.StandardInput.WriteLine(string.Format("adb -s {0} push {1}\\{2}\\db.tar /sdcard/Download", EmulatorName, bind.Path, bind.PhoneNumber));
            ReadLines(true);
            P.StandardInput.WriteLine(string.Format("adb -s {0} push {1}\\{2}\\sp.tar /sdcard/Download", EmulatorName, bind.Path, bind.PhoneNumber));
            ReadLines(true);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell tar -xvf /sdcard/Download/db.tar -C /", EmulatorName));
            ReadLines(true);
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell tar -xvf /sdcard/Download/sp.tar -C /", EmulatorName));
            ReadLines(true);
            Actions.UpdateFileState(bind.FP, "数据导入结束！");
            Thread.Sleep(100);
            Debug.WriteLine(string.Format("{0} - {1} - {2}", EmulatorName, ListPosition, bind));
        }

        /// <summary>
        /// 读取设备号
        /// </summary>
        /// <returns>如果未找到返回空字符""</returns>
        private string ReadDeviceKey()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell cat /sdcard/bwuu.txt", EmulatorName));
            List<string> lines = ReadLines(true);
            string key = "";
            Regex rx = new Regex("\\w{8}.{16}\\w{12}");
            foreach (var line in lines)
            {
                if (line.Contains("value1"))
                {
                    key = rx.Match(line).Value;
                }
            }
            return key;
        }

        /// <summary>
        /// 读取cookie
        /// </summary>
        /// <returns>如果未找到返回""</returns>
        private string ReadCookie()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell cat /data/data/com.btd.wallet/shared_prefs/cookie_share.xml", EmulatorName));
            List<string> lines = ReadLines(true);
            string cookie = "";
            Regex rx = new Regex("bt.{80,100}<");
            foreach (var line in lines)
            {
                if (line.Contains("key1"))
                {
                    cookie = rx.Match(line).Value;
                    cookie = cookie.Substring(0, cookie.Length - 1);
                }
            }
            return cookie;
        }

        /// <summary>
        /// 获得手机号
        /// </summary>
        /// <returns>如果未找到返回""</returns>
        private string ReadPhoneNumber()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell cat /data/data/com.btd.wallet/shared_prefs/com.btd.wallet.share.xml", EmulatorName));
            List<string> lines = ReadLines(true);
            string pn = "";
            Regex rx = new Regex("\\d{11}");
            foreach (var line in lines)
            {
                if (line.Contains("lastLoginAccount"))
                {
                    pn = rx.Match(line).Value;
                }
            }
            return pn;
        }

        /// <summary>
        /// 拉起所有的BTD
        /// </summary>
        public void StartBtd()
        {
            Actions.UpdateState(ListPosition, "正在拉起BTD！");
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell am start com.btd.wallet/.MainActivity", EmulatorName));
            Actions.UpdateState(ListPosition, "BTD已启动！");
        }
    }
}
