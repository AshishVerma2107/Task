
using SQLite;

namespace TaskAppWithLogin.Models
{
    public class Comp_AttachmentModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string taskId { get; set; }
        public string file_type { get; set; }
        public string file_format { get; set; }
        public string max_numbers { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public string GeoLocation { get; set; }
        public string localPath { get; set; }
        public int Checked { get; set; }
        public string status { get; set; }
    }
}