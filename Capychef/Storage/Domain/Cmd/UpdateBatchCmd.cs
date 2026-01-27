namespace Capychef.Storage.Domain.Cmd;

public class UpdateBatchCmd
{
    public DateTime? BestBeforeDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}