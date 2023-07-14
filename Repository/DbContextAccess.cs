using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class DbContextAccess: DbContext
    {


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Vendor_management;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }
        }
        public DbContextAccess(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<VendorDetails> VendorDetails { get; set; }


        public DbSet<ProductDetail> productDetails { get; set; }

        public DbSet<ProductPurchaseOrder> productpurchaseorder { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

    }
}