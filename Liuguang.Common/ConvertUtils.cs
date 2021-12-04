using System;

namespace LiuGuang.Common
{
    /// <summary>
    /// 转换工具
    /// </summary>
    public class ConvertUtils
    {
        public enum EndianType
        {
            LittleEndian,
            BigEndian,
        }

        /// <summary>
        /// 获取一个unit
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="endianType"></param>
        /// <returns></returns>
        public static uint GetUint(byte[] buff, int offset, EndianType endianType = EndianType.LittleEndian)
        {
            var convertEndian = BitConverter.IsLittleEndian ? EndianType.LittleEndian : EndianType.BigEndian;
            if (convertEndian == endianType)
            {
                return BitConverter.ToUInt32(buff, offset);
            }
            var copyBuff = new byte[4];
            Array.Copy(buff, offset, copyBuff, 0, copyBuff.Length);
            Array.Reverse(copyBuff);
            return BitConverter.ToUInt32(copyBuff, 0);
        }

        /// <summary>
        /// 设置一个unit
        /// </summary>
        /// <param name="buff"></param>
        /// <param name="offset"></param>
        /// <param name="itemValue"></param>
        /// <param name="endianType"></param>
        /// <returns></returns>
        public static void SetUint(byte[] buff, int offset, uint itemValue, EndianType endianType = EndianType.LittleEndian)
        {
            var itemData = BitConverter.GetBytes(itemValue);
            var convertEndian = BitConverter.IsLittleEndian ? EndianType.LittleEndian : EndianType.BigEndian;
            if (convertEndian != endianType)
            {
                Array.Reverse(itemData);
            }
            Array.Copy(itemData, 0, buff, offset, itemData.Length);
        }

    }
}
