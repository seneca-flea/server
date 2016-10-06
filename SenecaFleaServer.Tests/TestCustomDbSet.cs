using System;
using System.Linq;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Tests
{
    class TestItemDbSet : TestDbSet<Item>
    {
        public override Item Find(params object[] keyValues)
        {
            return this.SingleOrDefault(i => i.ItemId == (int)keyValues.Single());
        }
    }

    class TestMessageDbSet : TestDbSet<Message>
    {
        public override Message Find(params object[] keyValues)
        {
            return this.SingleOrDefault(i => i.id == (int)keyValues.Single());
        }
    }

    class TestUserDbSet : TestDbSet<User>
    {
        public override User Find(params object[] keyValues)
        {
            return this.SingleOrDefault(i => i.Id == (int)keyValues.Single());
        }
    }
}