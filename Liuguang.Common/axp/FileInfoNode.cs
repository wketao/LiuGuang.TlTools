namespace LiuGuang.Common.axp
{
    public class FileInfoNode
    {
        public string FilePath { get; set; } = string.Empty;
        public int HashTablePos { get; set; } = -1;
        public int BlockTablePos { get; set; } = -1;
    }
}
