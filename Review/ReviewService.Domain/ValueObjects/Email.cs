using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;
using System.Net.Mail;

namespace ReviewService.Domain.ValueObjects
{
    public class Email : ValueObject
    {
        public string Value { get; set; }
        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidEmailException("Email cannot be empty");

            if(!IsValideEmail(value))
                throw new InvalidEmailException($"Email '{value}' is not valid");

            Value = value.ToLowerInvariant();

        }

        private static bool IsValideEmail(string email)
        {
            try 
            { 
                var addr = new MailAddress(email);
                return addr.Address == email.ToLowerInvariant();
            }
            catch 
            { 
                return false;
            }
        }
        public override string ToString() => Value;

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Value;
        }
    }
    public class InvalidEmailException : DomainException
    {
        public InvalidEmailException(string message) : base(message) { }
    }
}
