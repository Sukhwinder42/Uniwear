using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uniwear.Models;
using Uniwear.Services;
using System.Reflection.Emit;

namespace Uniwear.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
              : base(options)
        {
        }

        // DbSets (Tables)
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }


        //   public DbSet<ChatRequest> ChatRequests { get; set; }
        //  public DbSet<ChatResponse> ChatResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            // Product → Category (One-to-Many)
            modelbuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Order → OrderItems (One-to-Many)
            modelbuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            // ChatRequest → ChatResponse (One-to-One or One-to-Many)
            //builder.Entity<ChatResponse>()
            //    .HasOne<ChatRequest>()
            //    .WithMany()
            //    .HasForeignKey(cr => cr.ChatRequestId);


            modelbuilder.Entity<Product>().HasData(

                new Product
                {
                    
                    Id = 1,
                    Name = "Uniwear Hoodie",
                    Description = "A comfortable and stylish hoodie for university students.",
                    Price = 39.99m,
                    ImageUrl = "https://in.puma.com/in/en/pd/essentials-no--1-logo-mens-comfort-hoodie/682571?size=0140&swatch=55&utm_source=BING-DDA&utm_medium=DSP&utm_campaign=DSP_BING_DDA_IN_PMAX_agency_1000067495857508873&msclkid=2707980aa4df1e51bf0bf196b51250c6",
                    StockQuantity = 100,
                    CategoryId = 1,
                },
                new Product
                {
                    
                    Id = 2,
                    Name = "Uniwear T-Shirt",
                    Description = "A cool t-shirt with the Uniwear logo.",
                    Price = 19.99m,
                    ImageUrl = "https://sl.bing.net/fw3YOvHs6H6",
                    StockQuantity = 200,
                    CategoryId = 2
                },
                new Product
                {
                  
                    Id = 3,
                    Name = "Uniwear Backpack",
                    Description = "A durable backpack perfect for carrying your books and laptop.",
                    Price = 49.99m,
                    ImageUrl = "https://images.puma.com/image/upload/f_auto,q_auto,b_rgb:fafafa,w_750,h_750/global/091993/01/fnd/IND/fmt/png/Archive-KING-Backpack-28L",
                    StockQuantity = 150,
                    CategoryId = 3
                },
                new Product
                {
                   
                    Id = 4,
                    Name = "Uniwear Cap",
                    Description = "A stylish cap to complete your university look.",
                    Price = 14.99m,
                    ImageUrl = "https://sl.bing.net/eEj6P0lzOc8",
                    StockQuantity = 300,
                    CategoryId = 4
                },
                new Product
                {
                   
                    Id = 5,
                    Name = "Uniwear Sweatpants",
                    Description = "Comfortable sweatpants for lounging or studying.",
                    Price = 29.99m,
                    ImageUrl = "https://images.puma.com/image/upload/f_auto,q_auto,b_rgb:fafafa,w_750,h_750/global/691700/16/mod01/fnd/IND/fmt/png/Essentials-Block-Men's-Knitted-Sweatpants",
                    StockQuantity = 120,
                    CategoryId = 5
                }
                );

            modelbuilder.Entity<Category>().HasData(
                new Category
                {
                    Id = 1,
                    Name = "Clothing"
                },
                new Category
                {
                    Id = 2,
                    Name = "Accessories"
                },
                new Category
                {
                    Id = 3,
                    Name = "Bags"
                },
                new Category
                {
                    Id = 4,
                    Name = "Headwear"
                },
                new Category
                {
                    Id = 5,
                    Name = "Loungewear"
                }
                );

        }
    }
}

