using LegacyApp.Validation;
using System;
using System.Linq.Expressions;

namespace LegacyApp
{
    public class User
    {
        public Client Client { get; set; }
        public DateTime DateOfBirth { get; set; }

        [ObjectInvalidation(nameof(string.Contains), "@")]
        [ObjectInvalidation(nameof(string.Contains), ".", Negative = true)]
        public string EmailAddress { get; set; }

        [Invalidation(nameof(string.IsNullOrEmpty))]
        public string Firstname { get; set; }
        [Invalidation(nameof(string.IsNullOrEmpty))]
        public string Surname { get; set; }

        [BinaryInvalidation(true, ExpressionType.Equal, Dependance = nameof(User.CreditLimit))]
        public bool HasCreditLimit
        {
            get
            {
                return Client?.Name != "VeryImportantPerson";
            }
        }

        int CreditFactor
        {
            get
            {
                return Client?.Name == "ImportantPerson" ? 2 : 1;
            }
        }

        int? creditLimit;
        [BinaryInvalidation(500, ExpressionType.LessThan, Dependance = nameof(User.HasCreditLimit))]
        public int CreditLimit
        {
            get
            {
                if (!HasCreditLimit)
                {
                    return -1;
                }
                else if (creditLimit == null)
                {
#if DEBUG
                    creditLimit = 700;
#else
                    using (var userCreditService = new UserCreditServiceClient())
                    {
                        creditLimit = userCreditService.GetCreditLimit(Firstname, Surname, DateOfBirth) * CreditFactor;
                    }
#endif
                }
                return creditLimit.Value;
            }
        }


#if DEBUG
        public void SetMockCredit(int creditLimit)
        {
            this.creditLimit = creditLimit;
        }
#endif

        [BinaryInvalidation(21, ExpressionType.LessThan)]
        public int Age
        {
            get
            {
                var now = DateTime.Now;
                int age = now.Year - DateOfBirth.Year;
                if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
                {
                    age--;
                }
                return age;
            }
        }

        public bool IsValid
        {
            get
            {
                return new ModelValidation<User>(this);
            }
        }
    }
}