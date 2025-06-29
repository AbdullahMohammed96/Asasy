using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Asasy.Persistence.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Cities_CityId",
                table: "Regions");

            migrationBuilder.DropIndex(
                name: "IX_Regions_CityId",
                table: "Regions");

            migrationBuilder.DropColumn(
                name: "ExpireDate",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "CityId",
                table: "Regions");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Sliders",
                newName: "TitleEn");

            migrationBuilder.RenameColumn(
                name: "SliderName",
                table: "Sliders",
                newName: "TitleAr");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Sliders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Link",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Cities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageProfile",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneCode",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "Wallet",
                table: "AspNetUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Complaints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Replay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsReplay = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Complaints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Complaints_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Districts_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    CountDayes = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProhibitedGoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProhibitedGoods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRates_AspNetUsers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdvertsmentDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RegionId = table.Column<int>(type: "int", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    DistrictId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    StatusProduct = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShowPhone = table.Column<bool>(type: "bit", nullable: false),
                    IsSpecial = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertsmentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_Regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdvertsmentDetails_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AdvertsmentImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdsId = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvertsmentImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdvertsmentImages_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CommentAds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdsId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentAds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentAds_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CommentAds_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Favorites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Favorites_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Favorites_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Follows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdsId = table.Column<int>(type: "int", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Follows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Follows_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_AspNetUsers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Follows_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdsId = table.Column<int>(type: "int", nullable: true),
                    ProviderId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserPackages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AdsId = table.Column<int>(type: "int", nullable: false),
                    PackageNameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageNameEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDescriptionAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDescriptionEn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsPayed = table.Column<bool>(type: "bit", nullable: false),
                    TypePay = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    StatusReview = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPackages_AdvertsmentDetails_AdsId",
                        column: x => x.AdsId,
                        principalTable: "AdvertsmentDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPackages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_RegionId",
                table: "Cities",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_CategoryId",
                table: "AdvertsmentDetails",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_CityId",
                table: "AdvertsmentDetails",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_DistrictId",
                table: "AdvertsmentDetails",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_RegionId",
                table: "AdvertsmentDetails",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_SubCategoryId",
                table: "AdvertsmentDetails",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentDetails_UserId",
                table: "AdvertsmentDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdvertsmentImages_AdsId",
                table: "AdvertsmentImages",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAds_AdsId",
                table: "CommentAds",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentAds_UserId",
                table: "CommentAds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Complaints_UserId",
                table: "Complaints",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Districts_CityId",
                table: "Districts",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_AdsId",
                table: "Favorites",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UserId",
                table: "Favorites",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_AdsId",
                table: "Follows",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_ProviderId",
                table: "Follows",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Follows_UserId",
                table: "Follows",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AdsId",
                table: "Reports",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProviderId",
                table: "Reports",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_CategoryId",
                table: "SubCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_AdsId",
                table: "UserPackages",
                column: "AdsId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPackages_UserId",
                table: "UserPackages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_ProviderId",
                table: "UserRates",
                column: "ProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRates_UserId",
                table: "UserRates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities",
                column: "RegionId",
                principalTable: "Regions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cities_Regions_RegionId",
                table: "Cities");

            migrationBuilder.DropTable(
                name: "AdvertsmentImages");

            migrationBuilder.DropTable(
                name: "CommentAds");

            migrationBuilder.DropTable(
                name: "Complaints");

            migrationBuilder.DropTable(
                name: "Favorites");

            migrationBuilder.DropTable(
                name: "Follows");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "ProhibitedGoods");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "UserPackages");

            migrationBuilder.DropTable(
                name: "UserRates");

            migrationBuilder.DropTable(
                name: "AdvertsmentDetails");

            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Cities_RegionId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "Link",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Cities");

            migrationBuilder.DropColumn(
                name: "ImageProfile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneCode",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Wallet",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "TitleEn",
                table: "Sliders",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "TitleAr",
                table: "Sliders",
                newName: "SliderName");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpireDate",
                table: "Sliders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CityId",
                table: "Regions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Regions_CityId",
                table: "Regions",
                column: "CityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Cities_CityId",
                table: "Regions",
                column: "CityId",
                principalTable: "Cities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
