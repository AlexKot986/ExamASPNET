using Microsoft.EntityFrameworkCore;

namespace ApiMessage.Contexts.Models
{
    public class MessageContext : DbContext
    {
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public string _connectionString;

        public MessageContext() { }
        public MessageContext(string connectionString)
        {
            _connectionString = connectionString;
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().LogTo(Console.WriteLine)
                          //.UseNpgsql("Host=localhost;Port=5433;Username=postgres;Password=example;Database=TestMessageDb");
                          .UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(x => x.Id).HasName("message_pkey");
                entity.ToTable("messages");
                entity.Property(x => x.Id).HasColumnName("id");
                entity.Property(x => x.Text).HasColumnName("text");
                entity.Property(e => e.ToUserId).HasColumnName("to_userId");
                entity.Property(e => e.Received).HasColumnName("received");              
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.ToTable("users");
                entity.HasIndex(u => u.Id).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Id).HasColumnName("user_id");
                entity.Property(u => u.Email).HasColumnName("user_email");

            });

            base.OnModelCreating(modelBuilder);
        }


    }
}
