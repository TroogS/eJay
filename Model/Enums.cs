using System.ComponentModel;

namespace DebtMgr.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   Values that represent transaction types. </summary>
    ///
    /// <remarks>   Andre Beging, 08.09.2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public enum TransactionType
    {
        [Description("None")]
        None,
        [Description("Deposit")]
        Deposit,
        [Description("Charge")]
        Charge
    }
}
