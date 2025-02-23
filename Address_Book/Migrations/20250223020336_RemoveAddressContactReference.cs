using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Address_Book.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAddressContactReference : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_AddressId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressId",
                table: "Contacts",
                column: "AddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_AddressId",
                table: "Contacts");

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Addresses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AddressId",
                table: "Contacts",
                column: "AddressId",
                unique: true);
        }
    }
}
