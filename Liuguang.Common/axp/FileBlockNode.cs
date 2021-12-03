using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// Block表单元
    /// </summary>
    public class FileBlockNode
    {
        /// <summary>
        /// 对应的数据块在文件中的偏移
        /// </summary>
        public uint DataOffset;

        /// <summary>
        /// 本数据块所对应的文件大小(bytes)
        /// </summary>
        public uint BlockSize;

        /// <summary>
        /// 数据块标志
        /// </summary>
        public uint Flags;

        public async Task LoadAsync(FileStream fileStream)
        {
            var buff = new byte[4];
            //
            DataOffset = await IoUtils.ReadUintAsync(fileStream, buff);
            BlockSize = await IoUtils.ReadUintAsync(fileStream, buff);
            Flags = await IoUtils.ReadUintAsync(fileStream, buff);
        }
    }
}
