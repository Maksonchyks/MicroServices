using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;

namespace ReviewService.Domain.ValueObjects
{
    public class UserInfo : ValueObject
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public Email Email { get; private set; }

        private UserInfo() { }

        public UserInfo(string userId, string userName, Email email)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidUserInfoException("UserId cannot be empty");

            if (string.IsNullOrWhiteSpace(userName))
                throw new InvalidUserInfoException("UserName cannot be empty");

            if (email == null)
                throw new InvalidUserInfoException("Email cannot be null");

            UserId = userId;
            UserName = userName;
            Email = email;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return UserId;
            yield return UserName;
            yield return Email;
        }
    }

    public class InvalidUserInfoException : DomainException
    {
        public InvalidUserInfoException(string message) : base(message) { }
    }
}
