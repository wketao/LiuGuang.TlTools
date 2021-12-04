using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// Block表单元
    /// </summary>
    public class FileBlockNode
    {
        #region Fields
        private byte[] binaryData = new byte[3 * 4];
        #endregion

        #region Properties
        /// <summary>
        /// 对应的数据块在文件中的偏移
        /// </summary>
        public uint DataOffset
        {
            get => ConvertUtils.GetUint(binaryData, 0);
            set => ConvertUtils.SetUint(binaryData, 0, value);
        }

        /// <summary>
        /// 本数据块所对应的文件大小(bytes)
        /// </summary>
        public uint BlockSize
        {
            get => ConvertUtils.GetUint(binaryData, 4);
            set => ConvertUtils.SetUint(binaryData, 4, value);
        }

        /// <summary>
        /// 数据块标志
        /// </summary>
        public uint Flags
        {
            get => ConvertUtils.GetUint(binaryData, 8);
            set => ConvertUtils.SetUint(binaryData, 8, value);
        }
        #endregion
        public async Task LoadAsync(FileStream fileStream)
        {
            await fileStream.ReadAsync(binaryData, 0, binaryData.Length);
        }
    }
}
