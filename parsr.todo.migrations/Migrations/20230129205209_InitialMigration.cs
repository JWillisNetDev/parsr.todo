using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace parsr.todo.migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TodoTaskLists",
                columns: table => new
                {
                    TodoTaskListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    DateTimeCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTaskLists", x => x.TodoTaskListId);
                });

            migrationBuilder.CreateTable(
                name: "TodoTasks",
                columns: table => new
                {
                    TodoTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(72)", maxLength: 72, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    IsDone = table.Column<bool>(type: "bit", nullable: false),
                    TodoTaskListId = table.Column<int>(type: "int", nullable: false),
                    DateTimeCreatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTimeUpdatedUtc = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoTasks", x => x.TodoTaskId);
                    table.ForeignKey(
                        name: "FK_TodoTasks_TodoTaskLists_TodoTaskListId",
                        column: x => x.TodoTaskListId,
                        principalTable: "TodoTaskLists",
                        principalColumn: "TodoTaskListId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UK_TodoTaskLists_PublicId",
                table: "TodoTaskLists",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoTasks_TodoTaskListId",
                table: "TodoTasks",
                column: "TodoTaskListId");

            migrationBuilder.CreateIndex(
                name: "UK_TodoTasks_PublicId",
                table: "TodoTasks",
                column: "PublicId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoTasks");

            migrationBuilder.DropTable(
                name: "TodoTaskLists");
        }
    }
}
