namespace AutomatedDeployment.Domain.Entities
{
    public class LastDeploymentviewmodel
    {
        public int hubId { get; set; }
        public int appId { get; set; }
        public string ApplicationName { get; set; }
        public string HubName { get; set; }
    }
}
