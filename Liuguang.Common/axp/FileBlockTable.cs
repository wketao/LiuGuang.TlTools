using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// hash表
    /// </summary>
    public class FileBlockTable
    {
        /// <summary>
        /// block表的最大容量
        /// </summary>
        const uint BLOCK_TABLE_MAXSIZE = 1024 * 1024;

        #region Properties
        public List<FileBlockNode> ItemList { get; set; }
        #endregion
        public async Task LoadAsync(FileStream fileStream, uint blockCount)
        {
            ItemList = new List<FileBlockNode>();
            for (var itemIndex = 0; itemIndex < blockCount; itemIndex++)
            {
                var itemInfo = new FileBlockNode();
                await itemInfo.LoadAsync(fileStream);
                ItemList.Add(itemInfo);
            }
        }
    }
}
