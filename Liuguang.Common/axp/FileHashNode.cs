using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    /// <summary>
    /// hash表单元
    /// </summary>
    public class FileHashNode
    {
        /// <summary>
        /// 哈希值 A，用于校验
        /// </summary>
        public uint HashA;

        /// <summary>
        /// 哈希值 B，用于校验
        /// </summary>
        public uint HashB;

        /// <summary>
        /// 数据
        /// </summary>
        public uint Data;

        public async Task LoadAsync(FileStream fileStream)
        {
            var buff = new byte[4];
            //
            HashA = await IoUtils.ReadUintAsync(fileStream, buff);
            HashB = await IoUtils.ReadUintAsync(fileStream, buff);
            Data = await IoUtils.ReadUintAsync(fileStream, buff);
        }
    }
}
