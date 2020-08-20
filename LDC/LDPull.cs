using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace LDC
{
    class LDPull
    {
        private string EmulatorName;
        private string PhoneNumber;
        private Process P;

        public LDPull(string emulatorName, string phoneNumber)
        {
            EmulatorName = emulatorName;
            PhoneNumber = phoneNumber;
        }

        public void Run()
        {
            InitADBShell();

            List<string> pid = FindBtdWalletPid();
            foreach (var item in pid)
            {
                KillPid(item);
            }
            Thread.Sleep(1000);
            CleanAllData();
            //StartBidWallet();
        }

        private void CleanAllData()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/db.tar", EmulatorName));
            ReadLines();
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/Download/sp.tar", EmulatorName));
            ReadLines();
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm /sdcard/bwuu.txt", EmulatorName));
            ReadLines();
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /sdcard/Android/data/com.btd.wallet", EmulatorName));
            ReadLines();
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell rm -rf /data/data/com.btd.wallet/*", EmulatorName));
            ReadLines();
        }

        /// <summary>
        /// 在进程中查询所有btd钱包进程
        /// </summary>
        /// <returns></returns>
        private List<string> FindBtdWalletPid()
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell ps", EmulatorName));
            List<string> pid = new List<string>(2);
            List<string> T = ReadLines();
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
        /// 结束指定进程
        /// </summary>
        /// <param name="pid"></param>
        private void KillPid(string pid)
        {
            P.StandardInput.WriteLine(string.Format("adb -s {0} shell kill {1}", EmulatorName, pid));
            ReadLines();
        }

        /// <summary>
        /// 启动钱包
        /// </summary>
        public void StartBidWallet()
        {
            if (FindBtdWalletPid().Count == 0)
            {
                P.StandardInput.WriteLine(string.Format("adb -s {0} shell am start com.btd.wallet/.MainActivity", EmulatorName));
            }
        }

        /// <summary>
        /// 读取当前流内的所有行，尾行总是为0
        /// </summary>
        /// <returns></returns>
        private List<string> ReadLines()
        {
            List<string> lines = new List<string>();
            string line;
            try
            {
                do
                {
                    line = P.StandardOutput.ReadLine();
                    lines.Add(line);
                    Debug.WriteLine(line);
                } while (!line.Equals(string.Empty));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return lines;
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
            ReadLines();
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
                Debug.WriteLine(e.Data);
            }
        }

    }
}
