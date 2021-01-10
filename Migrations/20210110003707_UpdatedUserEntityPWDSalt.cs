using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalCellars.API.Migrations
{
    public partial class UpdatedUserEntityPWDSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Users",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte),
                oldType: "INTEGER");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "PasswordSalt",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0,
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);
        }
    }
}
