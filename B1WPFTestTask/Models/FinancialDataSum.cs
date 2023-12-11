namespace B1WPFTestTask.Models;

public class FinancialDataSum
{

    public decimal IncomingSaldoActive { get; set; } = 0.0M;
    public decimal IncomingSaldoPassive { get; set; } = 0.0M;
    public decimal TurnoverDebit { get; set; } = 0.0M;
    public decimal TurnoverCredit { get; set; } = 0.0M;
    public decimal OutcomingSaldoActive { get; set; } = 0.0M;
    public decimal OutcomingSaldoPassive { get; set; } = 0.0M;

    public void Add(
        decimal incomingSaldoActive,
        decimal incomingSaldoPassive,
        decimal turnoverDebit,
        decimal turnoverCredit,
        decimal outcomingSaldoActive,
        decimal outcomingSaldoPassive)
    {
        IncomingSaldoActive += incomingSaldoActive;
        IncomingSaldoPassive += incomingSaldoPassive;
        TurnoverDebit += turnoverDebit;
        TurnoverCredit += turnoverCredit;
        OutcomingSaldoActive += incomingSaldoActive;
        OutcomingSaldoPassive += incomingSaldoPassive;
    }
}
