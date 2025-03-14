using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCSP.Models.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseEntities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(34)", maxLength: 34, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomShoeDesignTemplateId = table.Column<long>(type: "bigint", nullable: true),
                    DesignData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CustomShoeDesign_Status = table.Column<int>(type: "int", nullable: true),
                    DesignerMarkup = table.Column<float>(type: "real", nullable: true),
                    TotalAmount = table.Column<float>(type: "real", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    TextureId = table.Column<long>(type: "bigint", nullable: true),
                    CustomShoeDesignTemplate_UserId = table.Column<long>(type: "bigint", nullable: true),
                    CustomShoeDesignTemplate_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<float>(type: "real", nullable: true),
                    _2DImageUrl = table.Column<string>(name: "2DImageUrl", type: "nvarchar(max)", nullable: true),
                    _3DFileUrl = table.Column<string>(name: "3DFileUrl", type: "nvarchar(max)", nullable: true),
                    CustomShoeDesignTemplate_IsDeleted = table.Column<int>(type: "int", nullable: true),
                    CustomShoeDesignTexture_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    CustomShoeDesignTexture_TextureId = table.Column<long>(type: "bigint", nullable: true),
                    DesignPreview_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    PreviewImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DesignService_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    DesignService_Price = table.Column<float>(type: "real", nullable: true),
                    ServiceId1 = table.Column<long>(type: "bigint", nullable: true),
                    Designer_UserId = table.Column<long>(type: "bigint", nullable: true),
                    Designer_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommissionRate = table.Column<float>(type: "real", nullable: true),
                    Rating = table.Column<float>(type: "real", nullable: true),
                    Designer_Status = table.Column<int>(type: "int", nullable: true),
                    Manufacturer_UserId = table.Column<long>(type: "bigint", nullable: true),
                    Manufacturer_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer_CommissionRate = table.Column<float>(type: "real", nullable: true),
                    Manufacturer_Status = table.Column<int>(type: "int", nullable: true),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    CriteriaId = table.Column<long>(type: "bigint", nullable: true),
                    ManufacturerCriteria_Status = table.Column<int>(type: "int", nullable: true),
                    Notification_UserId = table.Column<long>(type: "bigint", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notification_IsDeleted = table.Column<int>(type: "int", nullable: true),
                    Order_UserId = table.Column<long>(type: "bigint", nullable: true),
                    VoucherId = table.Column<long>(type: "bigint", nullable: true),
                    ShippingInfoId = table.Column<long>(type: "bigint", nullable: true),
                    TotalPrice = table.Column<float>(type: "real", nullable: true),
                    AmountPaid = table.Column<float>(type: "real", nullable: true),
                    Order_Status = table.Column<int>(type: "int", nullable: true),
                    ShippingStatus = table.Column<int>(type: "int", nullable: true),
                    ShippingInfoId1 = table.Column<long>(type: "bigint", nullable: true),
                    VoucherId1 = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: true),
                    OrderDetail_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: true),
                    OrderDetail_Price = table.Column<float>(type: "real", nullable: true),
                    Payment_OrderId = table.Column<long>(type: "bigint", nullable: true),
                    Amount = table.Column<float>(type: "real", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    PaymentStatus = table.Column<int>(type: "int", nullable: true),
                    PaymentGateway_UserId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentGateway_PaymentMethod = table.Column<int>(type: "int", nullable: true),
                    PaymentGateway_Status = table.Column<int>(type: "int", nullable: true),
                    Posts_UserId = table.Column<long>(type: "bigint", nullable: true),
                    Posts_Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Posts_Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Posts_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    PostsComments_UserId = table.Column<long>(type: "bigint", nullable: true),
                    PostsId = table.Column<long>(type: "bigint", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostsComments_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Rating_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    Rating_UserId = table.Column<long>(type: "bigint", nullable: true),
                    UserRating = table.Column<int>(type: "int", nullable: true),
                    Rating_Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    ReturnedCustomShoe_CustomShoeDesignId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnedCustomShoe_Price = table.Column<float>(type: "real", nullable: true),
                    ReturnedCustomShoe_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Service_ManufacturerId = table.Column<long>(type: "bigint", nullable: true),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Service_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    SetServiceAmount_ServiceId = table.Column<long>(type: "bigint", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetServiceAmount_Amount = table.Column<float>(type: "real", nullable: true),
                    SetServiceAmount_Status = table.Column<int>(type: "int", nullable: true),
                    SetServiceAmount_ServiceId1 = table.Column<long>(type: "bigint", nullable: true),
                    ShippingInfo_UserId = table.Column<long>(type: "bigint", nullable: true),
                    StreetAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingInfo_IsDeleted = table.Column<int>(type: "int", nullable: true),
                    Texture_UserId = table.Column<long>(type: "bigint", nullable: true),
                    Prompt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Texture_Status = table.Column<int>(type: "int", nullable: true),
                    Texture_IsDeleted = table.Column<int>(type: "int", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<long>(type: "bigint", nullable: true),
                    OrderDetailId = table.Column<long>(type: "bigint", nullable: true),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    Transaction_Amount = table.Column<float>(type: "real", nullable: true),
                    User_Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    User_Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserRole = table.Column<int>(type: "int", nullable: true),
                    Balance = table.Column<float>(type: "real", nullable: true),
                    DefaultAddressId = table.Column<long>(type: "bigint", nullable: true),
                    User_Status = table.Column<int>(type: "int", nullable: true),
                    User_IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    UserActivity_UserId = table.Column<long>(type: "bigint", nullable: true),
                    ViewedDesignId = table.Column<long>(type: "bigint", nullable: true),
                    ViewAt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ViewDuration = table.Column<int>(type: "int", nullable: true),
                    UserRecommendation_UserId = table.Column<long>(type: "bigint", nullable: true),
                    RecommendDesignId = table.Column<long>(type: "bigint", nullable: true),
                    VoucherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoucherValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Voucher_Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Voucher_Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CriteriaId",
                        column: x => x.CriteriaId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CustomShoeDesignId",
                        column: x => x.CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CustomShoeDesignTemplateId",
                        column: x => x.CustomShoeDesignTemplateId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CustomShoeDesignTemplate_UserId",
                        column: x => x.CustomShoeDesignTemplate_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CustomShoeDesignTexture_CustomShoeDesignId",
                        column: x => x.CustomShoeDesignTexture_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_CustomShoeDesignTexture_TextureId",
                        column: x => x.CustomShoeDesignTexture_TextureId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_DefaultAddressId",
                        column: x => x.DefaultAddressId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_DesignPreview_CustomShoeDesignId",
                        column: x => x.DesignPreview_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_DesignService_CustomShoeDesignId",
                        column: x => x.DesignService_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Designer_UserId",
                        column: x => x.Designer_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Manufacturer_UserId",
                        column: x => x.Manufacturer_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Notification_UserId",
                        column: x => x.Notification_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_OrderDetailId",
                        column: x => x.OrderDetailId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_OrderDetail_CustomShoeDesignId",
                        column: x => x.OrderDetail_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_OrderId",
                        column: x => x.OrderId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Order_UserId",
                        column: x => x.Order_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_PaymentGateway_UserId",
                        column: x => x.PaymentGateway_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Payment_OrderId",
                        column: x => x.Payment_OrderId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_PostsComments_UserId",
                        column: x => x.PostsComments_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_PostsId",
                        column: x => x.PostsId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Posts_UserId",
                        column: x => x.Posts_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Rating_CustomShoeDesignId",
                        column: x => x.Rating_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Rating_UserId",
                        column: x => x.Rating_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_RecommendDesignId",
                        column: x => x.RecommendDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ReturnedCustomShoe_CustomShoeDesignId",
                        column: x => x.ReturnedCustomShoe_CustomShoeDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ServiceId1",
                        column: x => x.ServiceId1,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Service_ManufacturerId",
                        column: x => x.Service_ManufacturerId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_SetServiceAmount_ServiceId",
                        column: x => x.SetServiceAmount_ServiceId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_SetServiceAmount_ServiceId1",
                        column: x => x.SetServiceAmount_ServiceId1,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ShippingInfoId",
                        column: x => x.ShippingInfoId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ShippingInfoId1",
                        column: x => x.ShippingInfoId1,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ShippingInfo_UserId",
                        column: x => x.ShippingInfo_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_TextureId",
                        column: x => x.TextureId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_Texture_UserId",
                        column: x => x.Texture_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_UserActivity_UserId",
                        column: x => x.UserActivity_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_UserId",
                        column: x => x.UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_UserRecommendation_UserId",
                        column: x => x.UserRecommendation_UserId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_ViewedDesignId",
                        column: x => x.ViewedDesignId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_VoucherId",
                        column: x => x.VoucherId,
                        principalTable: "BaseEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseEntities_BaseEntities_VoucherId1",
                        column: x => x.VoucherId1,
                        principalTable: "BaseEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CriteriaId",
                table: "BaseEntities",
                column: "CriteriaId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CustomShoeDesignId",
                table: "BaseEntities",
                column: "CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CustomShoeDesignTemplate_UserId",
                table: "BaseEntities",
                column: "CustomShoeDesignTemplate_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CustomShoeDesignTemplateId",
                table: "BaseEntities",
                column: "CustomShoeDesignTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CustomShoeDesignTexture_CustomShoeDesignId",
                table: "BaseEntities",
                column: "CustomShoeDesignTexture_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_CustomShoeDesignTexture_TextureId",
                table: "BaseEntities",
                column: "CustomShoeDesignTexture_TextureId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_DefaultAddressId",
                table: "BaseEntities",
                column: "DefaultAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Designer_UserId",
                table: "BaseEntities",
                column: "Designer_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_DesignPreview_CustomShoeDesignId",
                table: "BaseEntities",
                column: "DesignPreview_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_DesignService_CustomShoeDesignId",
                table: "BaseEntities",
                column: "DesignService_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Manufacturer_UserId",
                table: "BaseEntities",
                column: "Manufacturer_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ManufacturerId",
                table: "BaseEntities",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Notification_UserId",
                table: "BaseEntities",
                column: "Notification_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Order_UserId",
                table: "BaseEntities",
                column: "Order_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_OrderDetail_CustomShoeDesignId",
                table: "BaseEntities",
                column: "OrderDetail_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_OrderDetailId",
                table: "BaseEntities",
                column: "OrderDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_OrderId",
                table: "BaseEntities",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Payment_OrderId",
                table: "BaseEntities",
                column: "Payment_OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_PaymentGateway_UserId",
                table: "BaseEntities",
                column: "PaymentGateway_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_PaymentId",
                table: "BaseEntities",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Posts_UserId",
                table: "BaseEntities",
                column: "Posts_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_PostsComments_UserId",
                table: "BaseEntities",
                column: "PostsComments_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_PostsId",
                table: "BaseEntities",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Rating_CustomShoeDesignId",
                table: "BaseEntities",
                column: "Rating_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Rating_UserId",
                table: "BaseEntities",
                column: "Rating_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ReceiverId",
                table: "BaseEntities",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_RecommendDesignId",
                table: "BaseEntities",
                column: "RecommendDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ReturnedCustomShoe_CustomShoeDesignId",
                table: "BaseEntities",
                column: "ReturnedCustomShoe_CustomShoeDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Service_ManufacturerId",
                table: "BaseEntities",
                column: "Service_ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ServiceId",
                table: "BaseEntities",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ServiceId1",
                table: "BaseEntities",
                column: "ServiceId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_SetServiceAmount_ServiceId",
                table: "BaseEntities",
                column: "SetServiceAmount_ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_SetServiceAmount_ServiceId1",
                table: "BaseEntities",
                column: "SetServiceAmount_ServiceId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ShippingInfo_UserId",
                table: "BaseEntities",
                column: "ShippingInfo_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ShippingInfoId",
                table: "BaseEntities",
                column: "ShippingInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ShippingInfoId1",
                table: "BaseEntities",
                column: "ShippingInfoId1");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_Texture_UserId",
                table: "BaseEntities",
                column: "Texture_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_TextureId",
                table: "BaseEntities",
                column: "TextureId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_UserActivity_UserId",
                table: "BaseEntities",
                column: "UserActivity_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_UserId",
                table: "BaseEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_UserRecommendation_UserId",
                table: "BaseEntities",
                column: "UserRecommendation_UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_ViewedDesignId",
                table: "BaseEntities",
                column: "ViewedDesignId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_VoucherId",
                table: "BaseEntities",
                column: "VoucherId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseEntities_VoucherId1",
                table: "BaseEntities",
                column: "VoucherId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BaseEntities");
        }
    }
}
