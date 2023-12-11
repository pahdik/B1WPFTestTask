using B1WPFTestTask.DAL.Entities.Base;
using System.Collections.Generic;

namespace B1WPFTestTask.DAL.Models;

public class AccountClass : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Balance> Balances { get; } = new List<Balance>();
}
