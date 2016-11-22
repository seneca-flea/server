
using System;
using System.Data.Entity;
using SenecaFleaServer.Models;

namespace SenecaFleaServer.Tests
{
    public interface IDataContext : IDisposable
    {
        DbSet<Item> Items { get; }
        int SaveChanges();
        void MarkAsModified(object item);
    }

    public class TestAppContext : DataContext, IDataContext
    {
        public TestAppContext()
        {
            Items = new TestItemDbSet();
            Messages = new TestMessageDbSet();
            Users = new TestUserDbSet();
            Courses = new TestDbSet<Course>();
        }

        public override DbSet<Book> Books { get; set; }
        public override DbSet<Course> Courses { get; set; }
        public override DbSet<Image> Images { get; set; }
        public override DbSet<Item> Items { get; set; }
        public override DbSet<Location> Locations { get; set; }
        public override DbSet<Message> Messages { get; set; }
        public override DbSet<PickUpDetail> PickUpDetails { get; set; }
        public override DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public override DbSet<User> Users { get; set; }

        public new void MarkAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public new void Dispose() { }
    }
}
