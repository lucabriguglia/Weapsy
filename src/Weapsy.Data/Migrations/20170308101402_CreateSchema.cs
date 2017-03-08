using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Weapsy.Data.Migrations
{
    public partial class CreateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserToken",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserToken", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "App",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Folder = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_App", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DomainAggregate",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainAggregate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CultureName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Site",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AddLanguageSlug = table.Column<bool>(nullable: false),
                    Copyright = table.Column<string>(nullable: true),
                    HomePageId = table.Column<Guid>(nullable: false),
                    Logo = table.Column<string>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    ModuleTemplateId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PageTemplateId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ThemeId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Site", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Theme",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Folder = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    DisplayName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    MiddleNames = table.Column<string>(nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Prefix = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ModuleType",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AppId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    EditType = table.Column<int>(nullable: false),
                    EditUrl = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    ViewName = table.Column<string>(nullable: true),
                    ViewType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ModuleType_App_AppId",
                        column: x => x.AppId,
                        principalTable: "App",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DomainEvent",
                columns: table => new
                {
                    DomainAggregateId = table.Column<Guid>(nullable: false),
                    SequenceNumber = table.Column<int>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    TimeStamp = table.Column<DateTime>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DomainEvent", x => new { x.DomainAggregateId, x.SequenceNumber });
                    table.ForeignKey(
                        name: "FK_DomainEvent_DomainAggregate_DomainAggregateId",
                        column: x => x.DomainAggregateId,
                        principalTable: "DomainAggregate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaim_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Address = table.Column<string>(nullable: true),
                    DefaultCredentials = table.Column<bool>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    Host = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Port = table.Column<int>(nullable: false),
                    SiteId = table.Column<Guid>(nullable: false),
                    Ssl = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailAccount_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Page",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: true),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SiteId = table.Column<Guid>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Page", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Page_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SiteLocalisation",
                columns: table => new
                {
                    SiteId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteLocalisation", x => new { x.SiteId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_SiteLocalisation_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserClaim",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaim_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ModuleTypeId = table.Column<Guid>(nullable: false),
                    SiteId = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Module_ModuleType_ModuleTypeId",
                        column: x => x.ModuleTypeId,
                        principalTable: "ModuleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Module_Site_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Site",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Link = table.Column<string>(nullable: true),
                    MenuId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    ParentId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItem_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageLocalisation",
                columns: table => new
                {
                    PageId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    MetaDescription = table.Column<string>(nullable: true),
                    MetaKeywords = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageLocalisation", x => new { x.PageId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_PageLocalisation_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    InheritPermissions = table.Column<bool>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    PageId = table.Column<Guid>(nullable: false),
                    SortOrder = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Zone = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageModule_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PagePermission",
                columns: table => new
                {
                    PageId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PagePermission", x => new { x.PageId, x.RoleId, x.Type });
                    table.ForeignKey(
                        name: "FK_PagePermission_Page_PageId",
                        column: x => x.PageId,
                        principalTable: "Page",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemLocalisation",
                columns: table => new
                {
                    MenuItemId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemLocalisation", x => new { x.MenuItemId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_MenuItemLocalisation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuItemLocalisation_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItemPermission",
                columns: table => new
                {
                    MenuItemId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemPermission", x => new { x.MenuItemId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_MenuItemPermission_MenuItem_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageModuleLocalisation",
                columns: table => new
                {
                    PageModuleId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageModuleLocalisation", x => new { x.PageModuleId, x.LanguageId });
                    table.ForeignKey(
                        name: "FK_PageModuleLocalisation_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageModuleLocalisation_PageModule_PageModuleId",
                        column: x => x.PageModuleId,
                        principalTable: "PageModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageModulePermission",
                columns: table => new
                {
                    PageModuleId = table.Column<Guid>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageModulePermission", x => new { x.PageModuleId, x.RoleId, x.Type });
                    table.ForeignKey(
                        name: "FK_PageModulePermission_PageModule_PageModuleId",
                        column: x => x.PageModuleId,
                        principalTable: "PageModule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaim_RoleId",
                table: "RoleClaim",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaim_UserId",
                table: "UserClaim",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailAccount_SiteId",
                table: "EmailAccount",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_SiteId",
                table: "Menu",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItem_MenuId",
                table: "MenuItem",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemLocalisation_LanguageId",
                table: "MenuItemLocalisation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_ModuleTypeId",
                table: "Module",
                column: "ModuleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Module_SiteId",
                table: "Module",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_ModuleType_AppId",
                table: "ModuleType",
                column: "AppId");

            migrationBuilder.CreateIndex(
                name: "IX_Page_SiteId",
                table: "Page",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_PageModule_PageId",
                table: "PageModule",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageModuleLocalisation_LanguageId",
                table: "PageModuleLocalisation",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "User",
                column: "NormalizedUserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleClaim");

            migrationBuilder.DropTable(
                name: "UserClaim");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.DropTable(
                name: "UserToken");

            migrationBuilder.DropTable(
                name: "DomainEvent");

            migrationBuilder.DropTable(
                name: "EmailAccount");

            migrationBuilder.DropTable(
                name: "MenuItemLocalisation");

            migrationBuilder.DropTable(
                name: "MenuItemPermission");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "PageLocalisation");

            migrationBuilder.DropTable(
                name: "PageModuleLocalisation");

            migrationBuilder.DropTable(
                name: "PageModulePermission");

            migrationBuilder.DropTable(
                name: "PagePermission");

            migrationBuilder.DropTable(
                name: "SiteLocalisation");

            migrationBuilder.DropTable(
                name: "Theme");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "DomainAggregate");

            migrationBuilder.DropTable(
                name: "MenuItem");

            migrationBuilder.DropTable(
                name: "ModuleType");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "PageModule");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "App");

            migrationBuilder.DropTable(
                name: "Page");

            migrationBuilder.DropTable(
                name: "Site");
        }
    }
}
