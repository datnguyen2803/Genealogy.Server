using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Genealogy.Models;

public partial class GenealogyContext : DbContext
{
    public GenealogyContext()
    {
    }

    public GenealogyContext(DbContextOptions<GenealogyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ClanEventTable> ClanEventTables { get; set; }

    public virtual DbSet<MemberTable> MemberTables { get; set; }

    public virtual DbSet<RelationshipTable> RelationshipTables { get; set; }

    public virtual DbSet<ServerEventTable> ServerEventTables { get; set; }

    public virtual DbSet<UserTable> UserTables { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;database=genealogy;uid=root;password=Nguyendat1999", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.33-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<ClanEventTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clan_event_table");

            entity.HasIndex(e => e.MainMemId, "main_mem_id");

            entity.HasIndex(e => e.SubMemId, "sub_mem_id");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.Detail)
                .HasMaxLength(50)
                .HasColumnName("detail");
            entity.Property(e => e.MainMemId)
                .HasMaxLength(50)
                .HasColumnName("main_mem_id");
            entity.Property(e => e.SubMemId)
                .HasMaxLength(50)
                .HasColumnName("sub_mem_id");
            entity.Property(e => e.TimeOccur)
                .HasColumnType("datetime")
                .HasColumnName("time_occur");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.MainMem).WithMany(p => p.ClanEventTableMainMems)
                .HasForeignKey(d => d.MainMemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clan_event_table_ibfk_1");

            entity.HasOne(d => d.SubMem).WithMany(p => p.ClanEventTableSubMems)
                .HasForeignKey(d => d.SubMemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("clan_event_table_ibfk_2");
        });

        modelBuilder.Entity<MemberTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("member_table");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.BirthPlace)
                .HasMaxLength(50)
                .HasColumnName("birth_place");
            entity.Property(e => e.CurrentPlace)
                .HasMaxLength(50)
                .HasColumnName("current_place");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("dob");
            entity.Property(e => e.Dod)
                .HasColumnType("datetime")
                .HasColumnName("dod");
            entity.Property(e => e.GenNo).HasColumnName("gen_no");
            entity.Property(e => e.Gender).HasColumnName("gender");
            entity.Property(e => e.Image)
                .HasMaxLength(50)
                .HasColumnName("image");
            entity.Property(e => e.IsClanLeader).HasColumnName("is_clan_leader");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("lastname");
            entity.Property(e => e.Note)
                .HasMaxLength(50)
                .HasColumnName("note");
            entity.Property(e => e.Surname)
                .HasMaxLength(50)
                .HasColumnName("surname");
        });

        modelBuilder.Entity<RelationshipTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("relationship_table");

            entity.HasIndex(e => e.MainMemId, "main_mem_id");

            entity.HasIndex(e => e.SubMemId, "sub_mem_id");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.DateStart)
                .HasColumnType("datetime")
                .HasColumnName("date_start");
            entity.Property(e => e.MainMemId)
                .HasMaxLength(50)
                .HasColumnName("main_mem_id");
            entity.Property(e => e.RelateCode).HasColumnName("relate_code");
            entity.Property(e => e.SubMemId)
                .HasMaxLength(50)
                .HasColumnName("sub_mem_id");

            entity.HasOne(d => d.MainMem).WithMany(p => p.RelationshipTableMainMems)
                .HasForeignKey(d => d.MainMemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("relationship_table_ibfk_1");

            entity.HasOne(d => d.SubMem).WithMany(p => p.RelationshipTableSubMems)
                .HasForeignKey(d => d.SubMemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("relationship_table_ibfk_2");
        });

        modelBuilder.Entity<ServerEventTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("server_event_table");

            entity.HasIndex(e => e.Actuator, "actuator");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.Actuator)
                .HasMaxLength(36)
                .HasColumnName("actuator");
            entity.Property(e => e.Detail)
                .HasMaxLength(50)
                .HasColumnName("detail");
            entity.Property(e => e.RecordId)
                .HasMaxLength(36)
                .HasColumnName("record_id");
            entity.Property(e => e.TableName)
                .HasMaxLength(50)
                .HasColumnName("table_name");
            entity.Property(e => e.TimeOccur)
                .HasColumnType("datetime")
                .HasColumnName("time_occur");
            entity.Property(e => e.Type).HasColumnName("type");

            entity.HasOne(d => d.ActuatorNavigation).WithMany(p => p.ServerEventTables)
                .HasForeignKey(d => d.Actuator)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("server_event_table_ibfk_1");
        });

        modelBuilder.Entity<UserTable>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_table");

            entity.Property(e => e.Id)
                .HasMaxLength(36)
                .HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .HasColumnName("password");
            entity.Property(e => e.Role).HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
