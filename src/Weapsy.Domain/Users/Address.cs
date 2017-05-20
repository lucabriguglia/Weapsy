using Weapsy.Framework.Domain;

namespace Weapsy.Domain.Users
{
    public class Address : ValueObject
    {
        public string Unit { get; private set; }
        public string Street { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string Country { get; private set; }
        public string PostCode { get; private set; }
    }
}
