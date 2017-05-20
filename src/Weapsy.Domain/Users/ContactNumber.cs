using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users
{
    public class ContactNumber : ValueObject
    {
        public NumberType NumberType { get; private set; }
        public string Number { get; private set; }
    }
}
