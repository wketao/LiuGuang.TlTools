using System.Text;

namespace LiuGuang.Common.axp
{
    public class HashTools
    {
        enum HASH_TYPE
        {
            /// <summary>
            /// A 计算方法
            /// </summary>
            HT_A = 1,
            /// <summary>
            /// B 计算方法
            /// </summary>
            HT_B,
            /// <summary>
            /// Offset 计算方法
            /// </summary>
            HT_OFFSET,

            HT_NUMBER,
        };

        /// <summary>
        /// 用于hash计算的数据表
        /// </summary>
        public static uint[] MathDataBuf;

        public static readonly Encoding StrEncoding = Encoding.GetEncoding("GB18030");
        /// <summary>
        /// 初始化数据表
        /// </summary>
        public static void InitMathDataBuf()
        {
            if (MathDataBuf != null)
            {
                return;
            }
            MathDataBuf = new uint[(int)HASH_TYPE.HT_NUMBER * 0x100];
            uint seed = 0x00100001;
            for (int index1 = 0; index1 < 0x100; index1++)
            {
                int index2 = index1;
                for (int i = 0; i < (int)HASH_TYPE.HT_NUMBER; i++, index2 += 0x100)
                {
                    uint temp1, temp2;
                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp1 = (seed & 0xFFFF) << 0x10;

                    seed = (seed * 125 + 3) % 0x2AAAAB;
                    temp2 = (seed & 0xFFFF);

                    MathDataBuf[index2] = (temp1 | temp2);
                }
            }
        }

        private static uint hashString(HASH_TYPE hashType, string str)
        {

            uint seed1 = 0x7FED7FED;
            uint seed2 = 0xEEEEEEEE;
            var strData = StrEncoding.GetBytes(str);
            foreach (var byteP in strData)
            {
                uint ch = byteP;
                uint bufIndex = (((uint)hashType) << 8) + ch;
                if (ch > 127)
                {
                    bufIndex -= 256;
                }
                seed1 = MathDataBuf[bufIndex] ^ (seed1 + seed2);
                seed2 = ch + seed1 + seed2 + (seed2 << 5) + 3;
                if (ch > 127)
                {
                    seed2 -= 256;
                }

            }

            return seed1;
        }

        /// <summary>
        /// 根据字符串得到位置，如果不存在，返回-1
        /// </summary>
        /// <param name="hashTable"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int GetPosInHashTable(FileHashTable hashTable, string str)
        {
            if (string.IsNullOrEmpty(str)) { return -1; }
            uint nHash = hashString(HASH_TYPE.HT_OFFSET, str);      // for pos
            uint nHashA = hashString(HASH_TYPE.HT_A, str);  // for check
            uint nHashB = hashString(HASH_TYPE.HT_B, str);	// for check again

            uint nHashStart = nHash % FileHashTable.HASH_TABLE_SIZE;
            uint nHashPos = nHashStart;

            do
            {
                var hashNode = hashTable.ItemList[nHashPos];
                //校验值正确
                if (hashNode.Exists() && hashNode.HashA == nHashA && hashNode.HashB == nHashB)
                {
                    return (int)nHashPos;
                }

                //查找下一个
                nHashPos = (nHashPos + 1) % FileHashTable.HASH_TABLE_SIZE;
                if (nHashPos == nHashStart)
                {
                    //回到了起点,退出,防止死循环
                    break;
                }

            } while (true);

            // not find
            return -1;
        }
    }
}
