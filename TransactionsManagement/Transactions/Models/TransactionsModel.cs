using System;

namespace Transactions.Models
{
    public class TransactionsModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Guid AccountId { get; set; }
    }

    public class TransactionsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public Guid AccountId { get; set; }
    }
}
