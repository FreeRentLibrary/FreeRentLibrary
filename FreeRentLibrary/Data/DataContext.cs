using FreeRentLibrary.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreeRentLibrary.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<AuthorGenre> AuthorGenres { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookEdition> BookEditions { get; set; }
        public DbSet<BookGenre> BookGenres { get; set; }
        public DbSet<BookPublisher> Publishers { get; set; }
        public DbSet<BookTypes> BookTypes { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Library> Libraries { get; set; }
        public DbSet<LibraryStock> LibraryStocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }
        public DbSet<Rent> Rentals { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorGenre>()
                .HasKey(ag => new { ag.AuthorId, ag.GenreId });

            modelBuilder.Entity<AuthorGenre>()
                .HasOne(ag => ag.Author)
                .WithMany(a => a.AuthorGenres)
                .HasForeignKey(ag => ag.AuthorId);

            modelBuilder.Entity<AuthorGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(g => g.AuthorGenres)
                .HasForeignKey(ag => ag.GenreId);

            modelBuilder.Entity<BookGenre>()
                .HasKey(bg => new { bg.BookId, bg.GenreId });

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Book)
                .WithMany(b => b.BookGenres)
                .HasForeignKey(bg => bg.BookId);

            modelBuilder.Entity<BookGenre>()
                .HasOne(bg => bg.Genre)
                .WithMany(g => g.BookGenres)
                .HasForeignKey(bg => bg.GenreId);

            modelBuilder.Entity<BookEdition>()
                .HasIndex(be => be.ISBN)
                .IsUnique();

            //Rentals - User : One-to-many Relationship
            modelBuilder.Entity<Rent>()
                .HasOne(c => c.User)
                .WithMany(u => u.Rentals)
                .HasForeignKey(c => c.UserId);

            //Rentals - Book : One-to-many Relationship
            //modelBuilder.Entity<Rent>()
            //    .HasOne(c => c.Book)
            //    .WithMany(b => b.Rentals)
            //    .HasForeignKey(c => c.BookId);

            //Reservation - User : One-to-many Relationship
            modelBuilder.Entity<Reservation>()
				.HasOne(r => r.User)
				.WithMany(u => u.Reservations)
				.HasForeignKey(c => c.UserId);

            //Reservation - Book : One-to-many Relationship
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
