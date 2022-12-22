using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityInfo.API.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("03a051ac-484b-47c2-bdac-af461f4122ff"), "Alternative life", "Wellington" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("8c9b6082-c6fc-4e0f-99d6-904b880bba33"), "Big apple", "New York" });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[] { new Guid("fc1c76c4-3e26-4e90-9a07-35d42c9c2d74"), "Mega city", "Tokyo" });

            migrationBuilder.InsertData(
                table: "PointsOfInterests",
                columns: new[] { "Id", "CityId", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("3a0f253e-8939-412f-980e-04269e351f18"), new Guid("03a051ac-484b-47c2-bdac-af461f4122ff"), "Super windy!", "Brooklyn Windmill" },
                    { new Guid("c2e184ed-c109-4db2-a63a-d6a6fb10ad3a"), new Guid("fc1c76c4-3e26-4e90-9a07-35d42c9c2d74"), "Very tall!", "Tokyo Sky Tree" },
                    { new Guid("cf46b096-2d63-483d-9b26-c0d4223f4503"), new Guid("8c9b6082-c6fc-4e0f-99d6-904b880bba33"), "Green statue!", "Statue of Liberty" },
                    { new Guid("dbae16b3-3a18-4173-a195-f6b55b89ce74"), new Guid("fc1c76c4-3e26-4e90-9a07-35d42c9c2d74"), "So many people!", "Shibuya Crossing" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PointsOfInterests",
                keyColumn: "Id",
                keyValue: new Guid("3a0f253e-8939-412f-980e-04269e351f18"));

            migrationBuilder.DeleteData(
                table: "PointsOfInterests",
                keyColumn: "Id",
                keyValue: new Guid("c2e184ed-c109-4db2-a63a-d6a6fb10ad3a"));

            migrationBuilder.DeleteData(
                table: "PointsOfInterests",
                keyColumn: "Id",
                keyValue: new Guid("cf46b096-2d63-483d-9b26-c0d4223f4503"));

            migrationBuilder.DeleteData(
                table: "PointsOfInterests",
                keyColumn: "Id",
                keyValue: new Guid("dbae16b3-3a18-4173-a195-f6b55b89ce74"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("03a051ac-484b-47c2-bdac-af461f4122ff"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("8c9b6082-c6fc-4e0f-99d6-904b880bba33"));

            migrationBuilder.DeleteData(
                table: "Cities",
                keyColumn: "Id",
                keyValue: new Guid("fc1c76c4-3e26-4e90-9a07-35d42c9c2d74"));
        }
    }
}
