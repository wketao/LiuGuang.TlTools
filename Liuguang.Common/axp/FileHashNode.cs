using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// hash表单元
    /// </summary>
    public class FileHashNode
    {
        #region Fields
        private byte[] binaryData = new byte[3 * 4];
        #endregion

        #region Properties
        /// <summary>
        /// 哈希值 A，用于校验
        /// </summary>
        public uint HashA
        {
            get => ConvertUtils.GetUint(binaryData, 0);
            set => ConvertUtils.SetUint(binaryData, 0, value);
        }

        /// <summary>
        /// 哈希值 B，用于校验
        /// </summary>
        public uint HashB
        {
            get => ConvertUtils.GetUint(binaryData, 4);
            set => ConvertUtils.SetUint(binaryData, 4, value);
        }

        /// <summary>
        /// 数据
        /// </summary>
        public uint Data
        {
            get => ConvertUtils.GetUint(binaryData, 8);
            set => ConvertUtils.SetUint(binaryData, 8, value);
        }
        #endregion

        public async Task LoadAsync(FileStream fileStream)
        {
            await fileStream.ReadAsync(binaryData, 0, binaryData.Length);
        }

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            return (Data & 0X80000000) != 0;
        }

        public uint BlockIndex()
        {
            return Data & 0X3FFFFFFF;
        }
    }
}
