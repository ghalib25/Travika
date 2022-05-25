using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Model.Model
{
    public partial class TravikaContext : DbContext
    {
        public TravikaContext()
        {
        }

        public TravikaContext(DbContextOptions<TravikaContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; } = null!;
        public virtual DbSet<DetailsHotel> DetailsHotels { get; set; } = null!;
        public virtual DbSet<DetailsTicketing> DetailsTicketings { get; set; } = null!;
        public virtual DbSet<Hotel> Hotels { get; set; } = null!;
        public virtual DbSet<MerchantProfile> MerchantProfiles { get; set; } = null!;
        public virtual DbSet<Payment> Payments { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<Ticketing> Ticketings { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Travika;uid=sa;pwd=123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerProfile>(entity =>
            {
                entity.ToTable("CustomerProfile");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DetailsHotel>(entity =>
            {
                entity.ToTable("DetailsHotel");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.HasOne(d => d.Hotel)
                    .WithMany(p => p.DetailsHotels)
                    .HasForeignKey(d => d.HotelId)
                    .HasConstraintName("FK_DetailsHotel_Hotel");

                entity.HasOne(d => d.Transaction)
                    .WithMany(p => p.DetailsHotels)
                    .HasForeignKey(d => d.TransactionId)
                    .HasConstraintName("FK_DetailsHotel_Transaction");
            });

            modelBuilder.Entity<DetailsTicketing>(entity =>
            {
                entity.ToTable("DetailsTicketing");

                entity.Property(e => e.Created).HasColumnType("datetime");

                entity.HasOne(d => d.Ticketing)
                    .WithMany(p => p.DetailsTicketings)
                    .HasForeignKey(d => d.TicketingId)
                    .HasConstraintName("FK_DetailsTicketing_Ticketing");

                entity.HasOne(d => d.Transcation)
                    .WithMany(p => p.DetailsTicketings)
                    .HasForeignKey(d => d.TranscationId)
                    .HasConstraintName("FK_DetailsTicketing_Transaction");
            });

            modelBuilder.Entity<Hotel>(entity =>
            {
                entity.ToTable("Hotel");

                entity.Property(e => e.Address)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HotelName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MerchantProfile>(entity =>
            {
                entity.ToTable("MerchantProfile");

                entity.Property(e => e.CompanyName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentMethod)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");

                entity.Property(e => e.Role1)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Role");
            });

            modelBuilder.Entity<Ticketing>(entity =>
            {
                entity.ToTable("Ticketing");

                entity.Property(e => e.Arrival).HasColumnType("datetime");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Departure).HasColumnType("datetime");

                entity.Property(e => e.Destination)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Origin)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.ToTable("Transaction");

                entity.Property(e => e.PaymentStatus)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.VirtualAccount)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password).HasColumnType("ntext");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
