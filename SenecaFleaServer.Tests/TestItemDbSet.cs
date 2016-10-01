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
}