namespace WebDavManga
{
    public class WebDAVConfig
    {
        public WebDAVChild WebDAV { get; set; }
    }

    public class WebDAVChild
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Directory { get; set; }
    }
}
