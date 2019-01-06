namespace eJay.Data
{
    public class EJaySettings
    {
        public string Database { get; set; }

        public string TelegramPath { get; set; }

        public EJaySettings()
        {
            Database = string.Empty;
            TelegramPath = string.Empty;
        }
    }
}
