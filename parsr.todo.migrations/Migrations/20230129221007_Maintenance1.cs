using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace parsr.todo.migrations.Migrations
{
    /// <inheritdoc />
    public partial class Maintenance1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PublicId",
                table: "TodoTasks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NewId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PublicId",
                table: "TodoTaskLists",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NewId()",
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "PublicId",
                table: "TodoTasks",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NewId()");

            migrationBuilder.AlterColumn<Guid>(
                name: "PublicId",
                table: "TodoTaskLists",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldDefaultValueSql: "NewId()");
        }
    }
}
