namespace AutoDealerPro.Modules.Leads.Core.Entities
{
    public class FollowUp
    {
        public Guid Id { get; set; }
        public Guid LeadId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? NextFollowUpDate { get; set; }
    }
}
