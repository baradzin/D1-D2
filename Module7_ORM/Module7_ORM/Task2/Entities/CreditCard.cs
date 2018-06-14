using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.Entities
{
    public class CreditCard
    {
        public int CreditCardID { get; set; }

        public string Number { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string CardHolderName { get; set; }

        public virtual Employee Holder { get; set; }
    }
}
