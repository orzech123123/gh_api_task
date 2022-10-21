using CommandLine;

namespace GhApiTask
{
    public class Options
    {
        [Value(0, MetaName = "username",
            HelpText = "Github username",
            Required = true)]
        public string Username { get; set; }

        [Value(1, MetaName = "repository",
            HelpText = "Github repository name",
            Required = true)]
        public string Repository { get; set; }
    }
}
