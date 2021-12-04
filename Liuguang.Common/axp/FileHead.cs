using System;
using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    public class FileHead
    {
        /// <summary>
        /// AXPK 标记位
        /// </summary>
        const uint AXPK_FILE_FLAG = 0x4b505841;


        #region Fields
        private byte[] binaryData = new byte[10 * 4];
        #endregion

        #region Properties
        /// <summary>
        /// 标记位
        /// </summary>
        public uint Identity
        {
            get => ConvertUtils.GetUint(binaryData, 0);
            set => ConvertUtils.SetUint(binaryData, 0, value);
        }

        /// <summary>
        /// 版本(Major|Minor)
        /// </summary>
        public uint Version
        {
            get => ConvertUtils.GetUint(binaryData, 4);
            set => ConvertUtils.SetUint(binaryData, 4, value);
        }

        /// <summary>
        /// 编辑标志,当这个整数为1时，表示该文件正在被编辑
        /// </summary>
        public uint EditFlag
        {
            get => ConvertUtils.GetUint(binaryData, 8);
            set => ConvertUtils.SetUint(binaryData, 8, value);
        }

        /// <summary>
        /// Hash表在文件中的偏移
        /// </summary>
        public uint HashTableOffset
        {
            get => ConvertUtils.GetUint(binaryData, 12);
            set => ConvertUtils.SetUint(binaryData, 12, value);
        }

        /// <summary>
        /// Block表在文件中的偏移
        /// </summary>
        public uint BlockTableOffset
        {
            get => ConvertUtils.GetUint(binaryData, 16);
            set => ConvertUtils.SetUint(binaryData, 16, value);
        }

        /// <summary>
        /// Block表内容的个数
        /// </summary>
        public uint BlockTableCount
        {
            get => ConvertUtils.GetUint(binaryData, 20);
            set => ConvertUtils.SetUint(binaryData, 20, value);
        }

        /// <summary>
        /// Block表最大容量大小(bytes)
        /// </summary>
        public uint BlockTableMaxSize
        {
            get => ConvertUtils.GetUint(binaryData, 24);
            set => ConvertUtils.SetUint(binaryData, 24, value);
        }

        /// <summary>
        /// 数据块在文件中的偏移
        /// </summary>
        public uint DataOffset
        {
            get => ConvertUtils.GetUint(binaryData, 28);
            set => ConvertUtils.SetUint(binaryData, 28, value);
        }

        /// <summary>
        /// 数据块的大小,包括空洞(bytes)
        /// </summary>
        public uint DataSize
        {
            get => ConvertUtils.GetUint(binaryData, 32);
            set => ConvertUtils.SetUint(binaryData, 32, value);
        }

        /// <summary>
        /// 其中空洞数据块的大小(bytes)
        /// </summary>
        public uint DataHoleSize
        {
            get => ConvertUtils.GetUint(binaryData, 36);
            set => ConvertUtils.SetUint(binaryData, 36, value);
        }
        #endregion

        public async Task LoadAsync(FileStream fileStream)
        {
            await fileStream.ReadAsync(binaryData, 0, binaryData.Length);
            if (Identity != AXPK_FILE_FLAG)
            {
                throw new Exception("无效的AXP文件");
            }
        }
    }
}
