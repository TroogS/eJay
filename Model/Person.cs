using System;
using System.Collections.Generic;
using System.Linq;
using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;

namespace DebtMgr.Model
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A person. </summary>
    ///
    /// <remarks>   Andre Beging, 10.09.2017. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    public class Person
    {
        #region Id

        [PrimaryKey]
        public Guid Id { get; set; } = Guid.NewGuid();

        #endregion

        #region Transaction Relation

        [OneToMany(CascadeOperations = CascadeOperation.All)]
        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        #endregion

        #region Total

        /// <summary>  Transaction Total </summary>
        public double Total
        {
            get
            {
                var sum = 0.0;

                foreach (var transaction in Transactions)
                {
                    if (transaction.Type == TransactionType.Charge)
                        sum -= transaction.Amount;
                    else if (transaction.Type == TransactionType.Deposit)
                        sum += transaction.Amount;

                }

                return sum;
            }
        }

        #endregion

        #region FirstName

        public string FirstName { get; set; }

        #endregion

        #region LastName

        public string LastName { get; set; }

        #endregion

        #region ToString()

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Convert this object into a string representation. </summary>
        ///
        /// <remarks>   Andre Beging, 09.09.2017. </remarks>
        ///
        /// <returns>   A string that represents this object. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////
        public override string ToString()
        {
            return string.Format("{0} {1}", FirstName, LastName);
        }

        #endregion
    }
}
