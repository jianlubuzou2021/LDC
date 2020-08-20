namespace LDC
{
    /// <summary>
    /// 文件与设备的简易绑定信息
    /// </summary>
    class Bind
    {
        /// <summary>
        /// 对应文件列表的位置
        /// </summary>
        public int FP { set; get; } = 0;
        /// <summary>
        /// 手机号
        /// </summary>
        public string PhoneNumber { set; get; } = string.Empty;
        /// <summary>
        /// 此号码存储的目录
        /// </summary>
        public string Path { set; get; } = string.Empty;

        public Bind(int fP, string phoneNumber, string path)
        {
            FP = fP;
            PhoneNumber = phoneNumber;
            Path = path;
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} - {2}", PhoneNumber, FP, Path);
        }
    }
}
