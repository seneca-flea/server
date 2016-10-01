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
            this.Items = new TestItemDbSet();
        }

        public override DbSet<Item> Items { get; set; }
        public override DbSet<Category> Categories { get; set; }
        public override DbSet<ItemStatus> ItemStatuses { get; set; }
        public override DbSet<Publisher> Publishers { get; set; }
        public override DbSet<Course> Courses { get; set; }
        public override DbSet<Program> Programs { get; set; }
        public override DbSet<Favorite> Favorites { get; set; }
        public override DbSet<PostHistory> PostHistories { get; set; }
        public override DbSet<Message> Messages { get; set; }
        public override DbSet<User> Users { get; set; }

        public void MarkAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public new void Dispose() { }
    }
}