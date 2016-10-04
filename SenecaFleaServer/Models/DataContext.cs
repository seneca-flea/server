using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public interface IDataContext : IDisposable
    {
        DbSet<Item> Items { get; }
        int SaveChanges();
        void MarkAsModified(object item);
    }

    public partial class DataContext : DbContext, IDataContext
    {
        public DataContext() : base("name=DataContext")
        {
        }

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<GoogleMap> GoogleMaps { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<PickUpDetail> PickUpDetails { get; set; }
        public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public void MarkAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}