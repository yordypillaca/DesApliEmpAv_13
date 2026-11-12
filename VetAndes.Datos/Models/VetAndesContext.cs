using Microsoft.EntityFrameworkCore;
using VetAndes.Entidades;

namespace VetAndes.Datos.Models;

public partial class VetAndesContext : DbContext
{
    public VetAndesContext(DbContextOptions<VetAndesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Paciente> Pacientes { get; set; }

    public virtual DbSet<Propietario> Propietarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasIndex(e => new { e.PropietarioId, e.Nombre }, "UQ_Pacientes_Propietario_Nombre").IsUnique();

            entity.Property(e => e.Especie).HasMaxLength(20);
            entity.Property(e => e.Nombre).HasMaxLength(80);
            entity.Property(e => e.PesoKg).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Raza).HasMaxLength(60);
            entity.Property(e => e.Sexo).HasMaxLength(10);

            entity.HasOne(d => d.Propietario).WithMany(p => p.Pacientes)
                .HasForeignKey(d => d.PropietarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Pacientes_Propietarios");
        });

        modelBuilder.Entity<Propietario>(entity =>
        {
            entity.HasIndex(e => e.Documento, "UQ_Propietarios_Documento").IsUnique();

            entity.Property(e => e.Correo).HasMaxLength(120);
            entity.Property(e => e.Documento).HasMaxLength(15);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
