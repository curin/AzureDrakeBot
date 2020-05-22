using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AzureDrake.Bot.Models
{
    public partial class DragonBotContext : DbContext
    {
        public DragonBotContext()
        {
        }

        public DragonBotContext(DbContextOptions<DragonBotContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ban> Bans { get; set; }
        public virtual DbSet<Clip> Clips { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<RankPerms> RankPerms { get; set; }
        public virtual DbSet<Rank> Ranks { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                BotInfo info = BotInfo.Load();
                optionsBuilder.UseSqlServer(info.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "curin_bot");

            modelBuilder.Entity<Ban>(entity =>
            {
                entity.ToTable("bans");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.BanEnd)
                    .HasColumnName("banEnd")
                    .HasColumnType("datetime");

                entity.Property(e => e.BanReason)
                    .IsRequired()
                    .HasColumnName("banReason")
                    .HasMaxLength(256);

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasColumnName("userId")
                    .HasMaxLength(32);

                entity.HasOne(d => d.Users)
                    .WithMany(p => p.Bans)
                    .HasForeignKey(d => new { d.UserId, d.Channel })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_bans_users");
            });

            modelBuilder.Entity<Clip>(entity =>
            {
                entity.ToTable("clips");

                entity.HasIndex(e => e.Id)
                    .HasName("UQ__clips__3213E83EB7648B63")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(64);

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.CreationTime)
                    .HasColumnName("creationTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Game)
                    .IsRequired()
                    .HasColumnName("game")
                    .HasMaxLength(128);

                entity.Property(e => e.SubmitTime)
                    .HasColumnName("submitTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Submitter)
                    .IsRequired()
                    .HasColumnName("submitter")
                    .HasMaxLength(32);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(128);
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.ToTable("quotes");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AttributedTo)
                    .IsRequired()
                    .HasColumnName("attributedTo")
                    .HasMaxLength(256);

                entity.Property(e => e.Channel)
                    .IsRequired()
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.Quote)
                    .IsRequired()
                    .HasColumnName("quote")
                    .HasMaxLength(256);

                entity.Property(e => e.StreamDate)
                    .HasColumnName("streamDate")
                    .HasColumnType("datetime");

                entity.Property(e => e.StreamGame)
                    .IsRequired()
                    .HasColumnName("streamGame")
                    .HasMaxLength(128);

                entity.Property(e => e.StreamTime)
                    .IsRequired()
                    .HasColumnName("streamTime")
                    .HasMaxLength(13)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.StreamTitle)
                    .IsRequired()
                    .HasColumnName("streamTitle")
                    .HasMaxLength(128);

                entity.Property(e => e.SubmitTime)
                    .HasColumnName("submitTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Submitter)
                    .IsRequired()
                    .HasColumnName("submitter")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<RankPerms>(entity =>
            {
                entity.HasKey(e => new { e.RankId, e.Channel, e.RankPerm })
                    .HasName("PK_RANKPERMS");

                entity.ToTable("rankPerms");

                entity.HasIndex(e => new { e.RankId, e.Channel, e.RankPerm })
                    .HasName("IX_UniquePerm")
                    .IsUnique();

                entity.Property(e => e.RankId).HasColumnName("rankId");

                entity.Property(e => e.Channel)
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.RankPerm)
                    .HasColumnName("rankPerm")
                    .HasMaxLength(64);

                entity.HasOne(d => d.Ranks)
                    .WithMany(p => p.RankPerms)
                    .HasForeignKey(d => new { d.RankId, d.Channel })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_rankPerms_ranks");
            });

            modelBuilder.Entity<Rank>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Channel });

                entity.ToTable("ranks");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Channel)
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.RankName)
                    .IsRequired()
                    .HasColumnName("rankName")
                    .HasMaxLength(50);

                entity.Property(e => e.RankPriority).HasColumnName("rankPriority");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Channel });

                entity.ToTable("users");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasMaxLength(32);

                entity.Property(e => e.Channel)
                    .HasColumnName("channel")
                    .HasMaxLength(32);

                entity.Property(e => e.Banned).HasColumnName("banned");

                entity.Property(e => e.BitsTotal).HasColumnName("bitsTotal");

                entity.Property(e => e.Moderator).HasColumnName("moderator");

                entity.Property(e => e.Points).HasColumnName("points");

                entity.Property(e => e.RankId).HasColumnName("rankId");

                entity.Property(e => e.SubStreak).HasColumnName("subStreak");

                entity.Property(e => e.SubTier).HasColumnName("subTier");

                entity.Property(e => e.SubTotal).HasColumnName("subTotal");

                entity.Property(e => e.Subbed).HasColumnName("subbed");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasColumnName("username")
                    .HasMaxLength(32);

                entity.Property(e => e.Vip).HasColumnName("vip");

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => new { d.RankId, d.Channel })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_users_ranks");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
