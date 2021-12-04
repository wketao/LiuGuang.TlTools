using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// hash表
    /// </summary>
    public class FileHashTable
    {
        /// <summary>
        /// hash表的容量
        /// </summary>
        public const uint HASH_TABLE_SIZE = 0X8000;

        #region Properties
        public FileHashNode[] ItemList { get; set; } = new FileHashNode[HASH_TABLE_SIZE];
        #endregion
        public async Task LoadAsync(FileStream fileStream)
        {
            for(var itemIndex = 0; itemIndex < HASH_TABLE_SIZE; itemIndex++)
            {
                var itemInfo = new FileHashNode();
                await itemInfo.LoadAsync(fileStream);
                ItemList[itemIndex] = itemInfo;
            }
        }
    }
}
