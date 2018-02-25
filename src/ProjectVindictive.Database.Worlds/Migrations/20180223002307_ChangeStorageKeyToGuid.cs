using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectVindictive.Database.Worlds.Migrations
{
    public partial class ChangeStorageKeyToGuid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StorageKey",
                table: "world_entry",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true)
                .Annotation("MySql:ValueGeneratedOnAddOrUpdate", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "StorageKey",
                table: "world_entry",
                nullable: true,
                oldClrType: typeof(Guid))
                .OldAnnotation("MySql:ValueGeneratedOnAddOrUpdate", true);
        }
    }
}
