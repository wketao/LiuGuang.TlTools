using System.IO;
using System.Threading.Tasks;

namespace LiuGuang.Common.axp
{
    public class PackFile
    {
        public FileHead Head { get; set; } = new FileHead();

        public async Task LoadAsync(string axpFilePath)
        {
            using (var fileStream = new FileStream(axpFilePath, FileMode.Open, FileAccess.Read))
            {
                await Head.LoadAsync(fileStream);
            }
        }
    }
}
