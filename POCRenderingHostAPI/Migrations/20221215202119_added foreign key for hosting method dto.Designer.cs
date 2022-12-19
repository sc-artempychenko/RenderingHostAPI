﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using POCRenderingHostAPI.Data;

#nullable disable

namespace POCRenderingHostAPI.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20221215202119_added foreign key for hosting method dto")]
    partial class addedforeignkeyforhostingmethoddto
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("POCRenderingHostAPI.Models.DTO.HostingMethodDTO", b =>
                {
                    b.Property<string>("RenderingHostName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("HostingMethod")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RenderingHostName");

                    b.ToTable("HostingMethodDTO");
                });

            modelBuilder.Entity("POCRenderingHostAPI.Models.DTO.RenderingHostDTO", b =>
                {
                    b.Property<string>("RenderingHostId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EnvironmentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PlatformTenantName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RenderingHostUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RepositoryUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SiteName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceControlIntegrationName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RenderingHostId");

                    b.HasIndex("Name");

                    b.ToTable("RenderingHosts");
                });

            modelBuilder.Entity("POCRenderingHostAPI.Models.DTO.RenderingHostDTO", b =>
                {
                    b.HasOne("POCRenderingHostAPI.Models.DTO.HostingMethodDTO", "RenderingHostHostingMethod")
                        .WithMany()
                        .HasForeignKey("Name");

                    b.Navigation("RenderingHostHostingMethod");
                });
#pragma warning restore 612, 618
        }
    }
}
