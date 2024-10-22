﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    public class PackFile
    {
        /// <summary>
        /// 列表文件路径
        /// </summary>
        const string LIST_FILENAME = "(list)";
        public FileHead Head { get; set; } = new FileHead();
        public FileHashTable HashTable { get; set; } = new FileHashTable();
        public FileBlockTable BlockTable { get; set; } = new FileBlockTable();

        public List<FileInfoNode> FileList { get; set; } = new List<FileInfoNode>();

        public async Task LoadAsync(FileStream fileStream)
        {
            //读取文件头
            await Head.LoadAsync(fileStream);
            //读取hash表
            fileStream.Seek(Head.HashTableOffset, SeekOrigin.Begin);
            await HashTable.LoadAsync(fileStream);
            //读取block表
            fileStream.Seek(Head.BlockTableOffset, SeekOrigin.Begin);
            await BlockTable.LoadAsync(fileStream, Head.BlockTableCount);
            //初始化hash tool
            HashTools.InitMathDataBuf();
            await LoadFileListAsync(fileStream);
        }

        private async Task LoadFileListAsync(FileStream fileStream)
        {
            var listHashTablePos = HashTools.GetPosInHashTable(HashTable, LIST_FILENAME);
            if (listHashTablePos < 0)
            {
                throw new Exception("未找到文件列表在hash表的节点位置");
            }
            var hashNode = HashTable.ItemList[listHashTablePos];
            var blockInfo = BlockTable.ItemList[(int)hashNode.BlockIndex()];
            var fileData = new byte[blockInfo.BlockSize];
            fileStream.Seek(blockInfo.DataOffset, SeekOrigin.Begin);
            await fileStream.ReadAsync(fileData, 0, fileData.Length);
            var listFileContent = HashTools.StrEncoding.GetString(fileData);
            using (var reader = new StringReader(listFileContent))
            {
                while (true)
                {
                    var lineContent = await reader.ReadLineAsync();
                    //没有新行了
                    if (lineContent == null)
                    {
                        break;
                    }
                    //格式不对,跳过这一行
                    var pIndex = lineContent.IndexOf("|");
                    if (pIndex <= 0)
                    {
                        continue;
                    }
                    //获取文件路径
                    var filePath = lineContent.Substring(0, pIndex);
                    //查找hash表
                    var normaliseName = filePath.Replace("\\", "/").ToLower();
                    var fileHashTablePos = HashTools.GetPosInHashTable(HashTable, normaliseName);
                    if (fileHashTablePos < 0)
                    {
                        //var strData = HashTools.StrEncoding.GetBytes(normaliseName);
                        //var strDataHex = string.Join(" ", Array.ConvertAll(strData, x => x.ToString("X2")));
                        //System.Console.WriteLine($"normaliseName={strDataHex}");
                        throw new Exception($"未找到文件[{filePath}]在hash表的节点位置");
                    }
                    var fileHashNode = HashTable.ItemList[fileHashTablePos];
                    var fileInfoNode = new FileInfoNode
                    {
                        HashTablePos = fileHashTablePos,
                        BlockTablePos = (int)fileHashNode.BlockIndex(),
                        FilePath = filePath,
                    };
                    FileList.Add(fileInfoNode);
                }
            }
            //System.Console.WriteLine(listFileContent);
        }

        public async Task UnPackAsync(string axpFilePath, string outputPath, Action<int, int> callback)
        {

            var baseName = Path.GetFileNameWithoutExtension(axpFilePath);
            callback.Invoke(0, 100);
            using (var fileStream = new FileStream(axpFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                //加载文件信息
                await LoadAsync(fileStream);
                callback.Invoke(0, FileList.Count);
                var processCount = 0;
                //依次保存文件
                foreach (var fileInfo in FileList)
                {
                    processCount++;
                    var fileBlockNode = BlockTable.ItemList[fileInfo.BlockTablePos];
                    await SaveFile(fileStream, fileBlockNode, Path.Combine(outputPath, baseName, fileInfo.FilePath));
                    callback.Invoke(processCount, FileList.Count);
                }
            }
        }

        private async Task SaveFile(FileStream fileStream, FileBlockNode fileBlockNode, string outputFilePath)
        {
            var dirPath = Path.GetDirectoryName(outputFilePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            fileStream.Seek(fileBlockNode.DataOffset, SeekOrigin.Begin);
            using (var outStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
            {
                //Console.WriteLine($"filename={outputFilePath}, size={fileBlockNode.BlockSize:x}");
                var fileData = new byte[fileBlockNode.BlockSize];
                await fileStream.ReadAsync(fileData, 0, fileData.Length);
                await outStream.WriteAsync(fileData, 0, fileData.Length);
            }
        }

        /// <summary>
        /// 读取文本文件
        /// </summary>
        /// <param name="fileStream"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<string> ReadTextFile(FileStream fileStream, string filePath)
        {
            foreach (var fileInfo in FileList)
            {
                if (!fileInfo.FilePath.Equals(filePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                var fileBlockNode = BlockTable.ItemList[fileInfo.BlockTablePos];
                fileStream.Seek(fileBlockNode.DataOffset, SeekOrigin.Begin);
                var fileData = new byte[fileBlockNode.BlockSize];
                await fileStream.ReadAsync(fileData, 0, fileData.Length);
                return HashTools.StrEncoding.GetString(fileData);
            }
            throw new Exception("未找到文件" + filePath);
        }

        /// <summary>
        /// 判断某个文件是否在包里
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool FileExists(string filePath)
        {
            foreach (var fileInfo in FileList)
            {
                if (fileInfo.FilePath.Equals(filePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
