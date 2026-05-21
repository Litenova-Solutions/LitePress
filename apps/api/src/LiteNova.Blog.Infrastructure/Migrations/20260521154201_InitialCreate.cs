using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiteNova.Blog.Infrastructure.Migrations;
/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "authors",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                display_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                external_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                registered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_authors", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "posts",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                slug = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                content = table.Column<string>(type: "text", nullable: false),
                excerpt = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                cover_image_url = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                author_id = table.Column<Guid>(type: "uuid", nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                published_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                archived_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                state = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_posts", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "tags",
            columns: table => new
            {
                id = table.Column<Guid>(type: "uuid", nullable: false),
                name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_tags", x => x.id);
            });

        migrationBuilder.CreateTable(
            name: "post_tags",
            columns: table => new
            {
                post_id = table.Column<Guid>(type: "uuid", nullable: false),
                tag_id = table.Column<Guid>(type: "uuid", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_post_tags", x => new { x.post_id, x.tag_id });
                table.ForeignKey(
                    name: "FK_post_tags_posts_post_id",
                    column: x => x.post_id,
                    principalTable: "posts",
                    principalColumn: "id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "uq_authors_external_id",
            table: "authors",
            column: "external_id",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "ix_post_tags_tag_id",
            table: "post_tags",
            column: "tag_id");

        migrationBuilder.CreateIndex(
            name: "ix_posts_author_id",
            table: "posts",
            column: "author_id");

        migrationBuilder.CreateIndex(
            name: "uq_posts_slug",
            table: "posts",
            column: "slug",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "uq_tags_name",
            table: "tags",
            column: "name",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "uq_tags_slug",
            table: "tags",
            column: "slug",
            unique: true);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "authors");

        migrationBuilder.DropTable(
            name: "post_tags");

        migrationBuilder.DropTable(
            name: "tags");

        migrationBuilder.DropTable(
            name: "posts");
    }
}
