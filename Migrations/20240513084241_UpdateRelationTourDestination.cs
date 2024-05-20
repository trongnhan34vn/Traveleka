using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tour_Booking.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationTourDestination : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Tours_TourId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Destinations_Tours_TourId",
                table: "Destinations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tours",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "TourId",
                table: "Destinations");

            migrationBuilder.AddColumn<Guid>(
                name: "DestinationId",
                table: "Tours",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tours",
                table: "Tours",
                column: "DestinationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Tours_TourId",
                table: "Assets",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "DestinationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tours_Destinations_DestinationId",
                table: "Tours",
                column: "DestinationId",
                principalTable: "Destinations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Tours_TourId",
                table: "Assets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tours_Destinations_DestinationId",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tours",
                table: "Tours");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations");

            migrationBuilder.DropColumn(
                name: "DestinationId",
                table: "Tours");

            migrationBuilder.AddColumn<Guid>(
                name: "TourId",
                table: "Destinations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tours",
                table: "Tours",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Destinations",
                table: "Destinations",
                column: "TourId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Tours_TourId",
                table: "Assets",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Destinations_Tours_TourId",
                table: "Destinations",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
