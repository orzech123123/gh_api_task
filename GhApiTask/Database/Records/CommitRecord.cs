namespace GhApiTask.Database.Records
{
    public class CommitRecord
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Repository { get; set; }
        public string Sha { get; set; }
        public string Message { get; set; }

        public string Commiter { get; set; }
    }
}
