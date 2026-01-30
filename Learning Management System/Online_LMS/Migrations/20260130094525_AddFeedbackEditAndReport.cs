using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Online_LMS.Migrations
{
    /// <inheritdoc />
    public partial class AddFeedbackEditAndReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CourseFeedbacks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReported",
                table: "CourseFeedbacks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "CourseFeedbacks",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CourseFeedbacks");

            migrationBuilder.DropColumn(
                name: "IsReported",
                table: "CourseFeedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "CourseFeedbacks");
        }
    }
}
