namespace LinkTracker.Shared.Models
{
    public class StoredFile
    {
        public string Name { get; set; }
        public string Ext { get; set; }
        public byte[] Data { get; set; }
        public string ContentType { get; set; }

        public StoredFile(string name, string ext, Stream data, string contentType)
        {
            Name = name;
            Ext = ext;
            Data = new byte[data.Length];
            data.Read(Data, 0, Data.Length);
            ContentType = contentType;
        }

        public StoredFile(string path, Stream data, string contentType) : this(Path.GetFileNameWithoutExtension(path), Path.GetExtension(path), data, contentType) { }

        public string GetPath()
        {
            return $"{Name}{Ext}";
        }
    }
}
