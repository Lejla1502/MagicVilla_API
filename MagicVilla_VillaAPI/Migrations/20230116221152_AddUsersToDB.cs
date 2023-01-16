using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVillaVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2023, 1, 16, 23, 11, 52, 107, DateTimeKind.Local).AddTicks(380), "https://dotnetmastery.com/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2023, 1, 16, 23, 11, 52, 107, DateTimeKind.Local).AddTicks(439), "https://dotnetmastery.com/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2023, 1, 16, 23, 11, 52, 107, DateTimeKind.Local).AddTicks(443), "https://dotnetmastery.com/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2023, 1, 16, 23, 11, 52, 107, DateTimeKind.Local).AddTicks(446), "https://dotnetmastery.com/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2023, 1, 16, 23, 11, 52, 107, DateTimeKind.Local).AddTicks(450), "https://dotnetmastery.com/bluevillaimages/villa2.jpg" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2022, 12, 25, 17, 9, 50, 584, DateTimeKind.Local).AddTicks(2399), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2022, 12, 25, 17, 9, 50, 584, DateTimeKind.Local).AddTicks(2475), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2022, 12, 25, 17, 9, 50, 584, DateTimeKind.Local).AddTicks(2481), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2022, 12, 25, 17, 9, 50, 584, DateTimeKind.Local).AddTicks(2485), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa5.jpg" });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "DateCreated", "ImageUrl" },
                values: new object[] { new DateTime(2022, 12, 25, 17, 9, 50, 584, DateTimeKind.Local).AddTicks(2490), "https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa2.jpg" });
        }
    }
}
