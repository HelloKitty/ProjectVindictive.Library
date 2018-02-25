using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectVindictive.Database.Worlds.Migrations
{
    public partial class ChangeToGuidStorage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageKey",
                table: "world_entry");

            migrationBuilder.AddColumn<Guid>(
                name: "StorageGuid",
                table: "world_entry",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageGuid",
                table: "world_entry");

            migrationBuilder.AddColumn<Guid>(
                name: "StorageKey",
                table: "world_entry",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"))
                .Annotation("MySql:ValueGeneratedOnAddOrUpdate", true);
        }
    }
}
