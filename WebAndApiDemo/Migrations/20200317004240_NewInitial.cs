﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAndApiDemo.Migrations
{
    public partial class NewInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titel = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Published = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategory",
                columns: table => new
                {
                    CategoryID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategory", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "NewsCategoryAssignment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsID = table.Column<int>(nullable: false),
                    NewsCategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsCategoryAssignment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_NewsCategoryAssignment_NewsCategory_NewsCategoryID",
                        column: x => x.NewsCategoryID,
                        principalTable: "NewsCategory",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NewsCategoryAssignment_News_NewsID",
                        column: x => x.NewsID,
                        principalTable: "News",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoryAssignment_NewsCategoryID",
                table: "NewsCategoryAssignment",
                column: "NewsCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_NewsCategoryAssignment_NewsID",
                table: "NewsCategoryAssignment",
                column: "NewsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NewsCategoryAssignment");

            migrationBuilder.DropTable(
                name: "NewsCategory");

            migrationBuilder.DropTable(
                name: "News");
        }
    }
}
