using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ProjectVindictive;

namespace ProjectVindictive.Database.Worlds.Migrations
{
    [DbContext(typeof(WorldDatabaseContext))]
    [Migration("20180223002307_ChangeStorageKeyToGuid")]
    partial class ChangeStorageKeyToGuid
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("ProjectVindictive.WorldEntryModel", b =>
                {
                    b.Property<long>("WorldId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccountId");

                    b.Property<string>("CreationIp")
                        .IsRequired()
                        .HasMaxLength(15);

                    b.Property<DateTime>("CreationTime")
                        .ValueGeneratedOnAddOrUpdate();

                    b.Property<Guid>("StorageKey")
                        .ValueGeneratedOnAddOrUpdate();

                    b.HasKey("WorldId");

                    b.ToTable("world_entry");
                });
        }
    }
}
