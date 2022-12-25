using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNullableToFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 12, 25, 17, 7, 20, 85, DateTimeKind.Local).AddTicks(3461));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2022, 12, 25, 17, 7, 20, 85, DateTimeKind.Local).AddTicks(3526));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2022, 12, 25, 17, 7, 20, 85, DateTimeKind.Local).AddTicks(3530));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2022, 12, 25, 17, 7, 20, 85, DateTimeKind.Local).AddTicks(3533));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2022, 12, 25, 17, 7, 20, 85, DateTimeKind.Local).AddTicks(3537));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2022, 12, 22, 17, 36, 17, 966, DateTimeKind.Local).AddTicks(5202));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2022, 12, 22, 17, 36, 17, 966, DateTimeKind.Local).AddTicks(5270));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2022, 12, 22, 17, 36, 17, 966, DateTimeKind.Local).AddTicks(5275));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2022, 12, 22, 17, 36, 17, 966, DateTimeKind.Local).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2022, 12, 22, 17, 36, 17, 966, DateTimeKind.Local).AddTicks(5285));
        }
    }
}
