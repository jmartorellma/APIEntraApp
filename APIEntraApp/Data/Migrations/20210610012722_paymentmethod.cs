using Microsoft.EntityFrameworkCore.Migrations;

namespace APIEntraApp.Data.Migrations
{
    public partial class paymentmethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Shops_ShopId",
                table: "PaymentMethods");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "PaymentMethods",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Shops_ShopId",
                table: "PaymentMethods",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Shops_ShopId",
                table: "PaymentMethods");

            migrationBuilder.AlterColumn<int>(
                name: "ShopId",
                table: "PaymentMethods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Shops_ShopId",
                table: "PaymentMethods",
                column: "ShopId",
                principalTable: "Shops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
