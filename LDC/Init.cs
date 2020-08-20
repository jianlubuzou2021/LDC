using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace LDC
{
    class Init
    {
        private Process P;

        public void Run()
        {
            InitADBShell();
            P.StandardInput.WriteLine("adb devices");
            List<string> lines = ReadLines();
            List<ListViewItem> items = new List<ListViewItem>(10);
            if (lines.Count > 3)// 如果只有三行，说明不包含设备信息
            {
                int counter = 1;
                for (int i = 2; i < lines.Count - 1; i++)//如果超过三行从第二行开始是设备信息，最后一行为空白(0)
                {
                    if (lines[i].Contains("device"))// 其它未未授权或者离线，不能使用
                    {
                        ListViewItem item = new ListViewItem(counter++ + "");
                        item.SubItems.Add(lines[i].Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries)[0]);
                        item.SubItems.Add("");
                        item.SubItems.Add("");
                        items.Add(item);
                    }
                }
            }

            P.StandardInput.WriteLine("Exit");
            P.Close();
            Actions.InitDevices(items);
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
            P.Start();
            P.StandardInput.AutoFlush = true;
            P.BeginErrorReadLine();
            ReadLines();
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
                } while (!line.Equals(string.Empty));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return lines;
        }
    }
}
