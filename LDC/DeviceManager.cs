using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LDC
{
    class DeviceManager
    {
        public static string TAG = "DeviceManager";
        private static DeviceManager manager;
        Dictionary<string, Device> Devices;

        private DeviceManager()
        {
            Devices = new Dictionary<string, Device>(10);
        }

        public static DeviceManager GetInstance()
        {
            if (manager == null)
            {
                manager = new DeviceManager();
            }
            return manager;
        }

        public void CleanData()
        {
            lock (this)
            {
                foreach (Device device in Devices.Values)
                {
                    device.CleanData();
                }
            }
        }

        public void ReleaseDevices()
        {
            lock (this)
            {
                foreach (Device device in Devices.Values)
                {
                    device.Dispose();
                }
                Devices.Clear();
            }
        }

        public void FetchData()
        {
            lock (this)
            {
                foreach (Device device in Devices.Values)
                {
                    device.FetchData();
                }
            }
        }

        public void AddDevice(string DeviceName, int index, ListViewItem item)
        {
            lock (this)
            {
                if (Devices.ContainsKey(DeviceName))
                {
                    Devices.Remove(DeviceName);
                }
                Devices.Add(DeviceName, new Device(DeviceName, index, item));
            }
        }

        public void CloseBtd()
        {
            lock (this)
            {
                foreach (Device device in Devices.Values)
                {
                    device.CloseBtd();
                }
            }
        }

        public void StartBtd()
        {
            lock (this)
            {
                foreach (Device device in Devices.Values)
                {
                    device.StartBtd();
                }
            }
        }

        public void PushData(Bind[] binds)
        {
            Device[] ds = Devices.Values.ToArray();
            if (ds.Length == 0)
            {
                MessageBox.Show("设备未连接！", "雷电群控", MessageBoxButtons.OK);
                return;
            }
            for (int i = 0; i < binds.Length; i++)
            {
                ds[i].PushData(binds[i]);
            }
        }

    }
}
