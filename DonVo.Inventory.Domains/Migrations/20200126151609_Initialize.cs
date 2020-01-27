using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DonVo.Inventory.Domains.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryDocuments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceNo = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceType = table.Column<string>(maxLength: 255, nullable: true),
                    StorageId = table.Column<int>(maxLength: 255, nullable: false),
                    StorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    StorageName = table.Column<string>(maxLength: 255, nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDocuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryMovements",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceNo = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceType = table.Column<string>(maxLength: 255, nullable: true),
                    ProductId = table.Column<int>(maxLength: 255, nullable: false),
                    ProductCode = table.Column<string>(maxLength: 1000, nullable: true),
                    ProductName = table.Column<string>(maxLength: 4000, nullable: true),
                    UomUnit = table.Column<string>(maxLength: 255, nullable: true),
                    UomId = table.Column<int>(maxLength: 255, nullable: false),
                    StorageId = table.Column<int>(maxLength: 255, nullable: false),
                    StorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    StorageName = table.Column<string>(maxLength: 255, nullable: true),
                    StockPlanning = table.Column<double>(nullable: false),
                    Before = table.Column<double>(nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    After = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    Uid = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryMovements", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventorySummaries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(maxLength: 255, nullable: true),
                    ProductId = table.Column<int>(maxLength: 255, nullable: false),
                    ProductCode = table.Column<string>(maxLength: 1000, nullable: true),
                    ProductName = table.Column<string>(maxLength: 4000, nullable: true),
                    UomUnit = table.Column<string>(maxLength: 255, nullable: true),
                    UomId = table.Column<int>(maxLength: 255, nullable: false),
                    StorageId = table.Column<int>(maxLength: 255, nullable: false),
                    StorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    StorageName = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    StockPlanning = table.Column<double>(nullable: false),
                    Uid = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySummaries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDistributionNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(maxLength: 255, nullable: true),
                    UnitId = table.Column<string>(maxLength: 255, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 255, nullable: true),
                    UnitName = table.Column<string>(maxLength: 255, nullable: true),
                    Type = table.Column<string>(maxLength: 255, nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsDisposition = table.Column<bool>(nullable: false),
                    AutoIncrementNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDistributionNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MaterialsRequestNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    UnitId = table.Column<string>(maxLength: 100, nullable: true),
                    UnitCode = table.Column<string>(maxLength: 100, nullable: true),
                    UnitName = table.Column<string>(maxLength: 100, nullable: true),
                    Remark = table.Column<string>(maxLength: 1000, nullable: true),
                    RequestType = table.Column<string>(maxLength: 100, nullable: true),
                    Type = table.Column<string>(nullable: true),
                    IsDistributed = table.Column<bool>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    AutoIncrementNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsRequestNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegradingResultDocs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    NoBon = table.Column<string>(maxLength: 255, nullable: true),
                    NoBonId = table.Column<string>(maxLength: 128, nullable: true),
                    UnitName = table.Column<string>(nullable: true),
                    SupplierId = table.Column<string>(nullable: true),
                    SupplierName = table.Column<string>(nullable: true),
                    SupplierCode = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(maxLength: 255, nullable: true),
                    Operator = table.Column<string>(nullable: true),
                    MachineName = table.Column<string>(nullable: true),
                    MachineId = table.Column<string>(nullable: true),
                    MachineCode = table.Column<string>(nullable: true),
                    Shift = table.Column<string>(nullable: true),
                    TotalLength = table.Column<double>(nullable: false),
                    OriginalGrade = table.Column<string>(nullable: true),
                    IsReturn = table.Column<bool>(nullable: false),
                    IsReturnedToPurchasing = table.Column<bool>(nullable: false),
                    AutoIncrementNumber = table.Column<int>(nullable: false),
                    Date = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegradingResultDocs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReturnInvToPurchasings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    No = table.Column<string>(maxLength: 255, nullable: true),
                    UnitName = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierId = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierCode = table.Column<string>(maxLength: 255, nullable: true),
                    AutoIncrementNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnInvToPurchasings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceNo = table.Column<string>(maxLength: 255, nullable: true),
                    ReferenceType = table.Column<string>(maxLength: 255, nullable: true),
                    SourceStorageId = table.Column<string>(maxLength: 255, nullable: true),
                    SourceStorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    SourceStorageName = table.Column<string>(maxLength: 255, nullable: true),
                    TargetStorageId = table.Column<string>(maxLength: 255, nullable: true),
                    TargetStorageCode = table.Column<string>(maxLength: 255, nullable: true),
                    TargetStorageName = table.Column<string>(maxLength: 255, nullable: true),
                    IsApproved = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferNotes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryDocumentItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    ProductId = table.Column<int>(maxLength: 255, nullable: false),
                    ProductCode = table.Column<string>(maxLength: 1000, nullable: true),
                    ProductName = table.Column<string>(maxLength: 4000, nullable: true),
                    UomUnit = table.Column<string>(maxLength: 255, nullable: true),
                    UomId = table.Column<int>(maxLength: 255, nullable: false),
                    Quantity = table.Column<double>(nullable: false),
                    StockPlanning = table.Column<double>(nullable: false),
                    ProductRemark = table.Column<string>(nullable: true),
                    MongoIndexItem = table.Column<int>(nullable: false),
                    InventoryDocumentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryDocumentItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InventoryDocumentItems_InventoryDocuments_InventoryDocumentId",
                        column: x => x.InventoryDocumentId,
                        principalTable: "InventoryDocuments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDistributionNoteItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    MaterialDistributionNoteId = table.Column<int>(nullable: false),
                    MaterialRequestNoteId = table.Column<int>(nullable: false),
                    MaterialRequestNoteCode = table.Column<string>(maxLength: 100, nullable: true),
                    MaterialRequestNoteCreatedDateUtc = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDistributionNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDistributionNoteItems_MaterialDistributionNotes_MaterialDistributionNoteId",
                        column: x => x.MaterialDistributionNoteId,
                        principalTable: "MaterialDistributionNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialsRequestNoteItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    MaterialsRequestNoteId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 100, nullable: true),
                    ProductionOrderId = table.Column<string>(maxLength: 100, nullable: true),
                    ProductionOrderNo = table.Column<string>(maxLength: 100, nullable: true),
                    ProductionOrderIsCompleted = table.Column<bool>(nullable: false),
                    OrderQuantity = table.Column<double>(nullable: false),
                    OrderTypeId = table.Column<string>(maxLength: 100, nullable: true),
                    OrderTypeCode = table.Column<string>(maxLength: 100, nullable: true),
                    OrderTypeName = table.Column<string>(maxLength: 100, nullable: true),
                    ProductId = table.Column<string>(maxLength: 100, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 100, nullable: true),
                    ProductName = table.Column<string>(maxLength: 100, nullable: true),
                    Grade = table.Column<string>(maxLength: 500, nullable: true),
                    DistributedLength = table.Column<double>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialsRequestNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialsRequestNoteItems_MaterialsRequestNotes_MaterialsRequestNoteId",
                        column: x => x.MaterialsRequestNoteId,
                        principalTable: "MaterialsRequestNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RegradingResultDocsDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    ReturProInvDocsId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 255, nullable: true),
                    ProductName = table.Column<string>(nullable: true),
                    ProductId = table.Column<string>(nullable: true),
                    ProductCode = table.Column<string>(nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    Remark = table.Column<string>(nullable: true),
                    Grade = table.Column<string>(nullable: true),
                    Return = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegradingResultDocsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegradingResultDocsDetails_RegradingResultDocs_ReturProInvDocsId",
                        column: x => x.ReturProInvDocsId,
                        principalTable: "RegradingResultDocs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReturnInvToPurchasingDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    ReturnInvToPurchasingId = table.Column<int>(nullable: false),
                    RegradingResultDocsId = table.Column<int>(nullable: false),
                    RegradingResultDocsCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductId = table.Column<string>(maxLength: 255, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    NecessaryLength = table.Column<double>(nullable: false),
                    Length = table.Column<double>(nullable: false),
                    Description = table.Column<string>(maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReturnInvToPurchasingDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReturnInvToPurchasingDetails_ReturnInvToPurchasings_ReturnInvToPurchasingId",
                        column: x => x.ReturnInvToPurchasingId,
                        principalTable: "ReturnInvToPurchasings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferNoteItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    StockTransferNoteId = table.Column<int>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 255, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    StockQuantity = table.Column<double>(nullable: false),
                    UomUnit = table.Column<string>(maxLength: 255, nullable: true),
                    UomId = table.Column<string>(maxLength: 255, nullable: true),
                    TransferedQuantity = table.Column<double>(nullable: false),
                    StockTransferNoteItemId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferNoteItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransferNoteItems_StockTransferNotes_StockTransferNoteId",
                        column: x => x.StockTransferNoteId,
                        principalTable: "StockTransferNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTransferNoteItems_StockTransferNoteItems_StockTransferNoteItemId",
                        column: x => x.StockTransferNoteItemId,
                        principalTable: "StockTransferNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaterialDistributionNoteDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedAgent = table.Column<string>(nullable: true),
                    LastModifiedUtc = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedAgent = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true),
                    DeletedAgent = table.Column<string>(nullable: true),
                    DeletedUtc = table.Column<DateTime>(nullable: false),
                    CreatedUtc = table.Column<DateTime>(nullable: false),
                    MaterialDistributionNoteItemId = table.Column<int>(nullable: false),
                    MaterialsRequestNoteItemId = table.Column<int>(nullable: false),
                    ProductionOrderId = table.Column<string>(nullable: true),
                    ProductionOrderNo = table.Column<string>(maxLength: 255, nullable: true),
                    ProductionOrderIsCompleted = table.Column<bool>(nullable: false),
                    ProductId = table.Column<string>(maxLength: 255, nullable: true),
                    ProductCode = table.Column<string>(maxLength: 255, nullable: true),
                    ProductName = table.Column<string>(maxLength: 255, nullable: true),
                    Grade = table.Column<string>(maxLength: 255, nullable: true),
                    Quantity = table.Column<double>(nullable: false),
                    DistributedLength = table.Column<double>(nullable: false),
                    MaterialRequestNoteItemLength = table.Column<double>(nullable: false),
                    ReceivedLength = table.Column<double>(nullable: false),
                    IsDisposition = table.Column<bool>(nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    SupplierId = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierCode = table.Column<string>(maxLength: 255, nullable: true),
                    SupplierName = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialDistributionNoteDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialDistributionNoteDetails_MaterialDistributionNoteItems_MaterialDistributionNoteItemId",
                        column: x => x.MaterialDistributionNoteItemId,
                        principalTable: "MaterialDistributionNoteItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryDocumentItems_InventoryDocumentId",
                table: "InventoryDocumentItems",
                column: "InventoryDocumentId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDistributionNoteDetails_MaterialDistributionNoteItemId",
                table: "MaterialDistributionNoteDetails",
                column: "MaterialDistributionNoteItemId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialDistributionNoteItems_MaterialDistributionNoteId",
                table: "MaterialDistributionNoteItems",
                column: "MaterialDistributionNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialsRequestNoteItems_MaterialsRequestNoteId",
                table: "MaterialsRequestNoteItems",
                column: "MaterialsRequestNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_RegradingResultDocsDetails_ReturProInvDocsId",
                table: "RegradingResultDocsDetails",
                column: "ReturProInvDocsId");

            migrationBuilder.CreateIndex(
                name: "IX_ReturnInvToPurchasingDetails_ReturnInvToPurchasingId",
                table: "ReturnInvToPurchasingDetails",
                column: "ReturnInvToPurchasingId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferNoteItems_StockTransferNoteId",
                table: "StockTransferNoteItems",
                column: "StockTransferNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferNoteItems_StockTransferNoteItemId",
                table: "StockTransferNoteItems",
                column: "StockTransferNoteItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryDocumentItems");

            migrationBuilder.DropTable(
                name: "InventoryMovements");

            migrationBuilder.DropTable(
                name: "InventorySummaries");

            migrationBuilder.DropTable(
                name: "MaterialDistributionNoteDetails");

            migrationBuilder.DropTable(
                name: "MaterialsRequestNoteItems");

            migrationBuilder.DropTable(
                name: "RegradingResultDocsDetails");

            migrationBuilder.DropTable(
                name: "ReturnInvToPurchasingDetails");

            migrationBuilder.DropTable(
                name: "StockTransferNoteItems");

            migrationBuilder.DropTable(
                name: "InventoryDocuments");

            migrationBuilder.DropTable(
                name: "MaterialDistributionNoteItems");

            migrationBuilder.DropTable(
                name: "MaterialsRequestNotes");

            migrationBuilder.DropTable(
                name: "RegradingResultDocs");

            migrationBuilder.DropTable(
                name: "ReturnInvToPurchasings");

            migrationBuilder.DropTable(
                name: "StockTransferNotes");

            migrationBuilder.DropTable(
                name: "MaterialDistributionNotes");
        }
    }
}
