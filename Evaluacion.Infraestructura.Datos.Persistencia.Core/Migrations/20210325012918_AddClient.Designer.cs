﻿// <auto-generated />
using System;
using Evaluacion.Infraestructura.Datos.Persistencia.Core.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Evaluacion.Infraestructura.Datos.Persistencia.Core.Migrations
{
    [DbContext(typeof(ContextoDb))]
    [Migration("20210325012918_AddClient")]
    partial class AddClient
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Clientes.ClienteEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Clientes");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Empleados.EmpleadoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("AreaEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AreaEntityId");

                    b.ToTable("Empleados");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Personas.PersonaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Apellido")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("ClienteEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CorreoElectronico")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("EmpleadoEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("FechaNacimiento")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("FechaRegistro")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NumeroTelefono")
                        .HasColumnType("int");

                    b.Property<Guid?>("ProveedorEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("TipoDocumentoEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ClienteEntityId");

                    b.HasIndex("EmpleadoEntityId");

                    b.HasIndex("ProveedorEntityId");

                    b.HasIndex("TipoDocumentoEntityId");

                    b.ToTable("Personas");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Proveedores.ProveedorEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Proveedores");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Genericas.Areas.AreaEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NombreArea")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<Guid?>("PersonaEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("PersonaEntityId");

                    b.ToTable("Areas");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Genericas.TipoDocumentos.TipoDocumentoEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CodigoTipoDocumento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NombreTipoDocumento")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("TipoDocumentos");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Empleados.EmpleadoEntity", b =>
                {
                    b.HasOne("Evaluacion.Dominio.Core.Genericas.Areas.AreaEntity", "AreaEntity")
                        .WithMany("Empleado")
                        .HasForeignKey("AreaEntityId");

                    b.Navigation("AreaEntity");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Personas.PersonaEntity", b =>
                {
                    b.HasOne("Evaluacion.Dominio.Core.Especificas.Clientes.ClienteEntity", "ClienteEntity")
                        .WithMany("PersonaCliente")
                        .HasForeignKey("ClienteEntityId");

                    b.HasOne("Evaluacion.Dominio.Core.Especificas.Empleados.EmpleadoEntity", "EmpleadoEntity")
                        .WithMany("PersonaEmpleado")
                        .HasForeignKey("EmpleadoEntityId");

                    b.HasOne("Evaluacion.Dominio.Core.Especificas.Proveedores.ProveedorEntity", "ProveedorEntity")
                        .WithMany("PersonaProveedor")
                        .HasForeignKey("ProveedorEntityId");

                    b.HasOne("Evaluacion.Dominio.Core.Genericas.TipoDocumentos.TipoDocumentoEntity", "TipoDocumentoEntity")
                        .WithMany("PersonaTipoDocumento")
                        .HasForeignKey("TipoDocumentoEntityId");

                    b.Navigation("ClienteEntity");

                    b.Navigation("EmpleadoEntity");

                    b.Navigation("ProveedorEntity");

                    b.Navigation("TipoDocumentoEntity");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Genericas.Areas.AreaEntity", b =>
                {
                    b.HasOne("Evaluacion.Dominio.Core.Especificas.Personas.PersonaEntity", "PersonaEntity")
                        .WithMany("Area")
                        .HasForeignKey("PersonaEntityId");

                    b.Navigation("PersonaEntity");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Clientes.ClienteEntity", b =>
                {
                    b.Navigation("PersonaCliente");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Empleados.EmpleadoEntity", b =>
                {
                    b.Navigation("PersonaEmpleado");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Personas.PersonaEntity", b =>
                {
                    b.Navigation("Area");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Especificas.Proveedores.ProveedorEntity", b =>
                {
                    b.Navigation("PersonaProveedor");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Genericas.Areas.AreaEntity", b =>
                {
                    b.Navigation("Empleado");
                });

            modelBuilder.Entity("Evaluacion.Dominio.Core.Genericas.TipoDocumentos.TipoDocumentoEntity", b =>
                {
                    b.Navigation("PersonaTipoDocumento");
                });
#pragma warning restore 612, 618
        }
    }
}
