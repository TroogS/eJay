using System;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace eJay.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A transaction. </summary>
    ///
    /// <remarks>   Andre Beging, 10.09.2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Transaction
    {
        #region Id

        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        #endregion

        #region Person Relation

        [ForeignKey(typeof(Person))]
        public Guid PersonId { get; set; }

        [ManyToOne]
        public Person Person { get; set; }

        #endregion

        #region Description

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the description. </summary>
        ///
        /// <value> The description. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public string Description { get; set; }

        #endregion

        #region Type

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets or sets the type. </summary>
        ///
        /// <value> The type. </value>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public TransactionType Type
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(StringType))
                {
                    return (TransactionType)Enum.Parse(typeof(TransactionType), StringType);
                }
                return TransactionType.None;

            }
            set { StringType = value.ToString(); }
        }

        public string StringType;
        #endregion

        #region Time

        public DateTime Time { get; set; }

        #endregion

        #region Amount

        public double Amount { get; set; }

        #endregion

        #region SignedAmount

        [Ignore]
        public double SignedAmount
        {
            get
            {
                if (Type == TransactionType.Charge) return Amount * -1;
                return Amount;
            }
        }

        #endregion
    }
}
