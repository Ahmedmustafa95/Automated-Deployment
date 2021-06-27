namespace AutomatedDeployment.Domain.Entities
{
    public class ConfigSearchResult
    {
        public int HubID { get; set; }
        public int AppID { get; set; }
        public string HubName { get; set; }
        public string AppName { get; set; }
        public string FileName { get; set; }
        public string NewConfigurationResult { get; set; }
        public string OldConfigurationResult { get; set; }

    }
}
