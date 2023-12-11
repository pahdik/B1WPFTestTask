using B1WPFTestTask.DAL.Entities;
using B1WPFTestTask.DAL.Entities.Base;

namespace B1WPFTestTask.DAL.Models;

public class Balance : BaseEntity
{
    public int AccountNumber { get; set; }
    public decimal IncomingSaldoActive { get; set; }
    public decimal IncomingSaldoPassive { get; set; }
    public decimal TurnoverDebit { get; set; }
    public decimal TurnoverCredit { get; set; }
    public int FileInformationId { get; set; }
    public int AccountClassId { get; set; }
    public int AccountGroupId { get; set; }
    public AccountClass AccountClass { get; set; } = null!;
    public FileInformation FileInformation { get; set; } = null!;
    public AccountGroup AccountGroup { get; set; } = null!;
}
