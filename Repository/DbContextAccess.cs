using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class DbContextAccess: DbContext
    {
        public DbContextAccess(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<VendorDetails> VendorDetails { get; set; }


        public DbSet<ProductDetail> productDetails { get; set; }

        public DbSet<ProductPurchaseOrder> productpurchaseorder { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

    }
}