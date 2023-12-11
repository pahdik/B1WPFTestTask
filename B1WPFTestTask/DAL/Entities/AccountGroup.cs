using B1WPFTestTask.DAL.Entities.Base;
using B1WPFTestTask.DAL.Models;
using System.Collections.Generic;

namespace B1WPFTestTask.DAL.Entities;

public class AccountGroup : BaseEntity
{
    public int GroupNumber { get; set; }
    public ICollection<Balance> Balances { get; set; }
}
