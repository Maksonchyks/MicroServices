using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReviewService.Domain.Common;

namespace ReviewService.Domain.ValueObjects
{
    public class Content : ValueObject
    {
        public string Text { get; private set; }
        public int WordCount { get; private set; }

        private Content() { }

        public Content(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidContentException("Content cannot be empty");

            if (text.Length > 5000)
                throw new InvalidContentException("Content cannot exceed 5000 characters");

            if (text.Length < 3)
                throw new InvalidContentException("Content must be at least 3 characters");

            Text = text.Trim();
            WordCount = Text.Split(new[] { ' ', '\t', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        }

        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return Text;
        }
    }

    public class InvalidContentException : DomainException
    {
        public InvalidContentException(string message) : base(message) { }
    }

}
