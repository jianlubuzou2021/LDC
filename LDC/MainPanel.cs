using NDevHelper.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace LDC
{
    public partial class MainPanel : Form
    {
        private ContextMenuStrip strip = new ContextMenuStrip();
        public MainPanel()
        {
            InitializeComponent();

            Actions.InitDevices = InitDevices;
            Actions.UpdatePN = UpdatePN;
            Actions.UpdateState = UpdateState;
            Actions.UpdateFileState = UpdateFileState;
            Xlog.LogInfoCallBack = LogIRefresh;
            Xlog.LogErrCallBack = LogERefresh;


            ToolStripItem SelectAll = new ToolStripMenuItem("全部连接");
            SelectAll.Click += SelectAll_Click;
            strip.Items.Add(SelectAll);
            ToolStripItem Kill = new ToolStripMenuItem("停止全部BTD");
            Kill.Click += Kill_Click; ;
            strip.Items.Add(Kill);
            ToolStripItem Start = new ToolStripMenuItem("拉起全部BTD");
            Start.Click += Start_Click;
            strip.Items.Add(Start);
            ToolStripItem CleanAll = new ToolStripMenuItem("清理数据");
            CleanAll.Click += CleanAll_Click;
            strip.Items.Add(CleanAll);
            ToolStripItem Fetch = new ToolStripMenuItem("拉取数据");
            Fetch.Click += Fetch_Click;
            strip.Items.Add(Fetch);

            //初始化右侧Files菜单
            InitFilesMenu();
        }

        private void ReadDevice_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().ReleaseDevices();
            new Thread(new ThreadStart(new Init().Run)).Start();

        }

        private void Devices_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = Devices.HitTest(e.X, e.Y);
                if (info.Item != null)
                {
                    strip.Show(Devices, e.Location);
                }
            }
        }

        private void InitDevices(List<ListViewItem> items)
        {
            if (InvokeRequired)
            {
                Action<List<ListViewItem>> callback = new Action<List<ListViewItem>>(InitDevices);
                Invoke(callback, items);
            }
            else
            {
                Devices.Clear();
                Devices.Columns.Add("序列", 50, HorizontalAlignment.Center);
                Devices.Columns.Add("设备号", 150, HorizontalAlignment.Center);
                Devices.Columns.Add("号码", 150, HorizontalAlignment.Center);
                Devices.Columns.Add("状态", 2000, HorizontalAlignment.Left);

                for (int i = 0; i < items.Count; i++)
                {
                    Devices.Items.Add(items[i]);
                }
            }
        }

        #region 代理
        public void LogIRefresh(string log)
        {
            if (InvokeRequired)
            {
                Action<string> callback = new Action<string>(LogIRefresh);
                Invoke(callback, log);
            }
            else
            {
                Log_I.AppendText(log);
            }
        }

        public void LogERefresh(string log)
        {
            if (InvokeRequired)
            {
                Action<string> callback = new Action<string>(LogERefresh);
                Invoke(callback, log);
            }
            else
            {
                Log_E.AppendText(log);
            }
        }

        /// <summary>
        /// 更新手机号
        /// </summary>
        /// <param name="pn"></param>
        public void UpdatePN(int index, string pn)
        {
            if (InvokeRequired)
            {
                Action<int, string> callback = new Action<int, string>(UpdatePN);
                Invoke(callback, index, pn);
            }
            else
            {
                Devices.Items[index].SubItems[2].Text = pn;
            }
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="index"></param>
        /// <param name="state"></param>
        public void UpdateState(int index, string state)
        {
            if (InvokeRequired)
            {
                Action<int, string> callback = new Action<int, string>(UpdateState);
                Invoke(callback, index, state);
            }
            else
            {
                Devices.Items[index].SubItems[3].Text = state;
            }
        }

        #endregion

        #region 菜单

        /// <summary>
        /// 保存数据到电脑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Fetch_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().FetchData();
        }

        /// <summary>
        /// 结束App
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Kill_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().CloseBtd();
        }

        /// <summary>
        /// 连接全部设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectAll_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().ReleaseDevices();
            for (int i = 0; i < Devices.Items.Count; i++)
            {
                DeviceManager.GetInstance().AddDevice(Devices.Items[i].SubItems[1].Text, i, Devices.Items[i]);
            }
        }

        /// <summary>
        /// 清理数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CleanAll_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().CleanData();
        }

        /// <summary>
        /// 拉起所有BTD
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Start_Click(object sender, EventArgs e)
        {
            DeviceManager.GetInstance().StartBtd();
        }
        #endregion


        #region Files
        private ContextMenuStrip FilesMenu = new ContextMenuStrip();
        private void InitFilesMenu()
        {
            ToolStripItem SelectPush = new ToolStripMenuItem("推入选中");
            SelectPush.Click += SelectPush_Click; ;
            FilesMenu.Items.Add(SelectPush);
        }

        private void SelectPush_Click(object sender, EventArgs e)
        {
            if (Devices.Items.Count <= 0)
            {
                MessageBox.Show("请连接设备后重试！", "雷电群控", MessageBoxButtons.OK);
                return;
            }

            foreach (ListViewItem item in Devices.Items)
            {
                if (!"".Equals(item.SubItems[2].Text))
                {
                    MessageBox.Show(string.Format("设备列表有未清除数据的设备！? {0} ?\r\n" +
                        "如果已写入数据，请清理应用数据后读取设备继续重试！", item.SubItems[1].Text), "雷电群控", MessageBoxButtons.OK);
                    return;
                }
            }

            if (Files.SelectedItems.Count > Devices.Items.Count)
            {
                MessageBox.Show("选择的号码数量多于设备数量！", "雷电群控", MessageBoxButtons.OK);
                return;
            }

            List<Bind> binds = new List<Bind>(10);
            foreach (ListViewItem item in Files.SelectedItems)
            {
                if ("是".Equals(item.SubItems[2].Text))
                {
                    MessageBox.Show("不能选定带有锁定标志的号码！", "雷电群控", MessageBoxButtons.OK);
                    return;
                }
                if (string.Empty.Equals(BasePath))
                {
                    MessageBox.Show(string.Format("{0} 数据路径为空！",item.SubItems[1].Text), "雷电群控", MessageBoxButtons.OK);
                    return;
                }
                binds.Add(new Bind(item.Index, item.SubItems[1].Text,BasePath));
            }
            DeviceManager.GetInstance().PushData(binds.ToArray());
        }

        private string BasePath = "";
        private void ReadFiles_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (Directory.Exists(folderBrowserDialog.SelectedPath))
                {
                    BasePath = folderBrowserDialog.SelectedPath;
                    Files.Clear();
                    Files.Columns.Add("序列", 50, HorizontalAlignment.Center);
                    Files.Columns.Add("号码", 120, HorizontalAlignment.Center);
                    Files.Columns.Add("锁定", 50, HorizontalAlignment.Center);
                    Files.Columns.Add("状态", 2000, HorizontalAlignment.Left);
                    string[] folders = Directory.GetDirectories(folderBrowserDialog.SelectedPath);
                    ListViewItem item;
                    for (int i = 0; i < folders.Length; i++)
                    {
                        item = new ListViewItem((i + 1) + "");
                        item.SubItems.Add(folders[i].Substring(folders[i].LastIndexOf("\\") + 1));
                        if (File.Exists(string.Format("{0}\\locked", folders[i])))
                        {
                            item.SubItems.Add("是");
                        }
                        else
                        {
                            item.SubItems.Add("否");
                        }
                        item.SubItems.Add("");
                        Files.Items.Add(item);
                    }
                }
            }
        }

        private void Files_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewHitTestInfo info = Files.HitTest(e.X, e.Y);
                if (info.Item != null)
                {
                    FilesMenu.Show(Files, e.Location);
                }
            }
        }

        public void UpdateFileState(int index, string pn)
        {
            if (InvokeRequired)
            {
                Action<int, string> callback = new Action<int, string>(UpdateFileState);
                Invoke(callback, index, pn);
            }
            else
            {
                Files.Items[index].SubItems[3].Text = pn;
            }
        }
        #endregion


    }
}
