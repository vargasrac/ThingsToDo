using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThingsToDo.Migrations
{
    /// <inheritdoc />
    public partial class TimeStamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeStamp",
                table: "ToDoTask",
                type: "datetime2",
                nullable: false,
                defaultValue: DateTime.Now);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeStamp",
                table: "ToDoTask");
        }
    }
}
