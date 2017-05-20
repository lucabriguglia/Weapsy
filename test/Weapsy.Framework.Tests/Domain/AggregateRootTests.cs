using System;
using NUnit.Framework;
using Weapsy.Framework.Domain;

namespace Weapsy.Framework.Tests.Domain
{
    [TestFixture]
    public class AggregateRootTests
    {
        [Test]
        public void Should_set_id()
        {
            var id = Guid.NewGuid();
            var aggregate = new FakeAggregate(id);
            Assert.AreEqual(id, aggregate.Id);
        }

        [Test]
        public void Should_set_new_id_if_it_is_empty()
        {
            var id = Guid.Empty;
            var aggregate = new FakeAggregate(id);
            Assert.AreNotEqual(id, aggregate.Id);
        }

        private class FakeAggregate : AggregateRoot
        {
            public FakeAggregate(Guid id) 
                : base(id)
            {
            }
        }
    }
}
