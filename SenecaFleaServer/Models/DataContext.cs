using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SenecaFleaServer.Models
{
    public partial class DataContext : DbContext
    {
        public DataContext() : base("name=DataContext")
        {
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ItemStatus> ItemStatuses { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<Favorite> Favorites { get; set; }
        public virtual DbSet<PostHistory> PostHistories { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
        }
    }
}