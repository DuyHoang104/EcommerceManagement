namespace EcommerceManagement.Domain.Entities.Commons
{
    public interface IEntityAuditLog
    {
        public int CreateBy { get; set; }
        public DateTime CreateTimeStamp { get; set; }
        
        public int LastActionBy { get; set; }
        public char LastAction { get; set; }
        public DateTime LastActionTimeStamp { get; set; }
    }
}