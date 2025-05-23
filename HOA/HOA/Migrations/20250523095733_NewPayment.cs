using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HOA.Migrations
{
    /// <inheritdoc />
    public partial class NewPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ResidentName",
                table: "Payments",
                newName: "PaymentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "Payments",
                newName: "ResidentName");
        }
    }
}
