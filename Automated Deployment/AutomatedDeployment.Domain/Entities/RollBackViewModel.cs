namespace AutomatedDeployment.Domain.Entities
{
    public class RollBackViewModel
    {
        public int hubId { get; set; }
        public int appID { get; set; }
        public string deployedBy { get; set; }
        public string requestedBy { get; set; }
        public string approvedBy { get; set; }
    }
}
