using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    public class PackFile
    {
        public FileHead Head { get; set; } = new FileHead();
        public FileHashNode[] HashTable { get; set; }
        public FileBlockNode[] BlockTable { get; set; }

        public async Task LoadAsync(string axpFilePath)
        {
            using (var fileStream = new FileStream(axpFilePath, FileMode.Open, FileAccess.Read))
            {
                //读取文件头
                await Head.LoadAsync(fileStream);
                //读取hash表
                fileStream.Seek(Head.HashTableOffset, SeekOrigin.Begin);
                var hashTable = new FileHashNode[Head.BlockTableCount];
                for(var hashIndex= 0;hashIndex < hashTable.Length;hashIndex++)
                {
                    var hashNode = new FileHashNode();
                    await hashNode.LoadAsync(fileStream);
                    hashTable[hashIndex] = hashNode;
                }
                HashTable = hashTable;
                //读取block表
                fileStream.Seek(Head.HashTableOffset, SeekOrigin.Begin);
                var blockTable = new FileBlockNode[Head.BlockTableCount];
                for (var hashIndex = 0; hashIndex < blockTable.Length; hashIndex++)
                {
                    var blockNode = new FileBlockNode();
                    await blockNode.LoadAsync(fileStream);
                    blockTable[hashIndex] = blockNode;
                }
                BlockTable = BlockTable;
                System.Console.WriteLine(HashTable.Length);
            }
        }
    }
}
