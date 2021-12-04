using LiuGuang.Common.axp;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LiuGuang.Common
{
    public class Imageset
    {
        public string Name { get; set; } = string.Empty;
        public string Filename { get; set; } = string.Empty;

        /// <summary>
        /// 检测文件的存在性
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="interfacePackFile"></param>
        /// <param name="materialPackFile"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CheckFileAsync(string sourcePath, FileStream interfaceStream, PackFile interfacePackFile, PackFile materialPackFile)
        {
            var fileContent = await readInterfaceFileContentAsync(sourcePath, interfaceStream, interfacePackFile);
            //读取 imageset
            var schemeXml = XDocument.Parse(fileContent);
            var rootEl = schemeXml.Root;
            var imagefileAttribute = rootEl.Attribute("Imagefile");
            if (imagefileAttribute == null)
            {
                return;
            }
            var imagefile = imagefileAttribute.Value;
            var sourceFilePath = Path.Combine(sourcePath, "Material", imagefile);
            if (File.Exists(sourceFilePath))
            {
                return;
            }
            if (materialPackFile.FileExists(imagefile))
            {
                return;
            }
            throw new Exception("找不到文件: [Material]" + imagefile + " from " + Filename);
        }

        private async Task<string> readInterfaceFileContentAsync(string sourcePath, FileStream interfaceStream, PackFile interfacePackFile)
        {
            var sourceFilePath = Path.Combine(sourcePath, "Interface", Filename);
            if (File.Exists(sourceFilePath))
            {
                return File.ReadAllText(sourceFilePath);
            }
            //读取axp中的文件内容
            return await interfacePackFile.ReadTextFile(interfaceStream, Filename);
        }
    }
}
