using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class newmig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackingNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDateTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedDateTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BillingAddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BillingAddressState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BillingAddressCountry = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    BillingAddressZipcode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingAddressCity = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingAddressState = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingAddressCountry = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ShippingAddressZipcode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VendorDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    VendorType = table.Column<int>(type: "int", nullable: false),
                    AddressLine1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddressLine2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelePhone1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TelePhone2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VendorEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VendorWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VendorDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "productDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(28,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productDetails_VendorDetails_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "productpurchaseorder",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    VendorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Quantity = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productpurchaseorder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_productpurchaseorder_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_productpurchaseorder_VendorDetails_VendorId",
                        column: x => x.VendorId,
                        principalTable: "VendorDetails",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_productpurchaseorder_productDetails_ProductId",
                        column: x => x.ProductId,
                        principalTable: "productDetails",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_productDetails_VendorId",
                table: "productDetails",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_productpurchaseorder_ProductId",
                table: "productpurchaseorder",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_productpurchaseorder_PurchaseOrderId",
                table: "productpurchaseorder",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_productpurchaseorder_VendorId",
                table: "productpurchaseorder",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productpurchaseorder");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "productDetails");

            migrationBuilder.DropTable(
                name: "VendorDetails");
        }
    }
}
