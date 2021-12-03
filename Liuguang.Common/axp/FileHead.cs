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
        const uint AXPK_FILE_FLAG = 0x4158504b;

        /// <summary>
        /// 标记位
        /// </summary>
        public uint Identity;

        /// <summary>
        /// 版本(Major|Minor)
        /// </summary>
        public uint Version;

        /// <summary>
        /// 编辑标志,当这个整数为1时，表示该文件正在被编辑
        /// </summary>
        public uint EditFlag;

        /// <summary>
        /// Hash表在文件中的偏移
        /// </summary>
        public uint HashTableOffset;

        /// <summary>
        /// Block表在文件中的偏移
        /// </summary>
        public uint BlockTableOffset;

        /// <summary>
        /// Block表内容的个数
        /// </summary>
        public uint BlockTableCount;

        /// <summary>
        /// Block表最大容量大小(bytes)
        /// </summary>
        public uint BlockTableMaxSize;

        /// <summary>
        /// 数据块在文件中的偏移
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// 数据块的大小,包括空洞(bytes)
        /// </summary>
        public uint DataSize;

        /// <summary>
        /// 其中空洞数据块的大小(bytes)
        /// </summary>
        public uint DataHoleSize;

        public async Task LoadAsync(FileStream fileStream)
        {
            var buff = new byte[4];
            //
            Identity = await IoUtils.ReadUintAsync(fileStream, buff, IoUtils.EndianType.BigEndian);
            Version = await IoUtils.ReadUintAsync(fileStream, buff);
            EditFlag = await IoUtils.ReadUintAsync(fileStream, buff);
            //
            HashTableOffset = await IoUtils.ReadUintAsync(fileStream, buff);
            BlockTableOffset = await IoUtils.ReadUintAsync(fileStream, buff);
            BlockTableCount = await IoUtils.ReadUintAsync(fileStream, buff);
            //
            BlockTableMaxSize = await IoUtils.ReadUintAsync(fileStream, buff);
            DataOffset = await IoUtils.ReadUintAsync(fileStream, buff);
            DataSize = await IoUtils.ReadUintAsync(fileStream, buff);
            DataHoleSize = await IoUtils.ReadUintAsync(fileStream, buff);
            if (Identity != AXPK_FILE_FLAG)
            {
                throw new Exception("无效的AXP文件");
            }
        }

        /// <summary>
        /// 小端模式
        /// </summary>
        private void SetLittleEndian(byte[] buff)
        {
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(buff);
            }
        }

        /// <summary>
        /// 大端模式
        /// </summary>
        private void SetBigEndian(byte[] buff)
        {
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(buff);
            }
        }
    }
}
