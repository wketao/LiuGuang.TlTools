using System;
using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common
{
    public class IoUtils
    {
        public enum EndianType
        {
            LittleEndian,
            BigEndian,
        }
        /// <summary>
        /// 小端模式,读取一个unit
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="buff"></param>
        /// <returns></returns>
        public static async Task<uint> ReadUintAsync(FileStream fileStream, byte[] buff, EndianType endianType = EndianType.LittleEndian)
        {
            await fileStream.ReadAsync(buff, 0, 4);
            var needReverse = false;
            if (BitConverter.IsLittleEndian)
            {
                if (endianType != EndianType.LittleEndian)
                {
                    needReverse = true;
                }
            }
            else if (endianType != EndianType.BigEndian)
            {
                needReverse = true;
            }
            if (needReverse)
            {
                Array.Reverse(buff);
            }
            return BitConverter.ToUInt32(buff, 0);
        }
    }
}
