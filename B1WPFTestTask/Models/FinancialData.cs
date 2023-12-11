namespace B1WPFTestTask.Models;

// Класс для отображения данных 
public class FinancialData
{
    public string AccountNumber { get; set; } = string.Empty;
    public string IncomingSaldoActive { get; set; } = string.Empty;
    public string IncomingSaldoPassive { get; set; } = string.Empty;
    public string TurnoverDebit { get; set; } = string.Empty;
    public string TurnoverCredit { get; set; } = string.Empty;
    public string OutcomingSaldoActive { get; set; } = string.Empty;
    public string OutcomingSaldoPassive { get; set; } = string.Empty;
    public bool IsHeaderRow { get; set; } = false;
}

