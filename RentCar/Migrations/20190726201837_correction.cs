using Microsoft.EntityFrameworkCore.Migrations;

namespace RentCar.Migrations
{
    public partial class correction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alugueis_AspNetUsers_UserId",
                table: "Alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_Contas_AspNetUsers_UserId",
                table: "Contas");

            migrationBuilder.AddForeignKey(
                name: "FK_Alugueis_AspNetUsers_UserId",
                table: "Alugueis",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_AspNetUsers_UserId",
                table: "Contas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alugueis_AspNetUsers_UserId",
                table: "Alugueis");

            migrationBuilder.DropForeignKey(
                name: "FK_Contas_AspNetUsers_UserId",
                table: "Contas");

            migrationBuilder.AddForeignKey(
                name: "FK_Alugueis_AspNetUsers_UserId",
                table: "Alugueis",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contas_AspNetUsers_UserId",
                table: "Contas",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
