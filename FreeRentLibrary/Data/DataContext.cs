using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Rent> Rentals { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<BookEdition> Editions { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Rentals - User : Relation
            modelBuilder.Entity<Rent>()
                .HasOne(c => c.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(c => c.UserId);

            //Rentals - Book : Relation
            //modelBuilder.Entity<Rent>()
            //    .HasOne(c => c.Book)
            //    .WithMany(b => b.Rentals)
            //    .HasForeignKey(c => c.BookId);

			//Reservation - User : Relation
			modelBuilder.Entity<Reservation>()
				.HasOne(c => c.User)
				.WithMany(u => u.Reservations)
				.HasForeignKey(c => c.UserId);

			//Reservation - Book : Relation
			//modelBuilder.Entity<Reservation>()
			//	.HasOne(c => c.Book)
			//	.WithMany(b => b.Reservations)
			//	.HasForeignKey(c => c.BookId);

			modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();

            modelBuilder.Entity<OrderDetailTemp>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<OrderDetail>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");


            base.OnModelCreating(modelBuilder);
        }

        //Habilita a regra de apagar em cascata chama-se cascate delete rule
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var cascadeFKs = modelBuilder.Model
        //        .GetEntityTypes()
        //        .SelectMany(t => t.GetForeignKeys())
        //        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        //    foreach (var fk in cascadeFKs)
        //    {
        //        fk.DeleteBehavior = DeleteBehavior.Restrict;
        //    }
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
