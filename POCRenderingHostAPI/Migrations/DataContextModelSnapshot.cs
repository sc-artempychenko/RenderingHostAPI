﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using POCRenderingHostAPI.Data;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("POCRenderingHostAPI.Models.DTO.RenderingHostDTO", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DefinitionItemId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EnvironmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Host")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RenderingHostHostingMethod")
                        .HasColumnType("int");

                    b.Property<string>("RenderingHostId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RenderingHostUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RepositoryUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SiteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkspaceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkspaceUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RenderingHosts");
                });
#pragma warning restore 612, 618
        }
    }
}
