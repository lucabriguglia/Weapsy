using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Weapsy.Apps.Text.Data.Migrations
{
    [DbContext(typeof(TextDbContext))]
    partial class TextDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Weapsy.Apps.Text.Data.Entities.TextLocalisation", b =>
                {
                    b.Property<Guid>("TextVersionId");

                    b.Property<Guid>("LanguageId");

                    b.Property<string>("Content");

                    b.HasKey("TextVersionId", "LanguageId");

                    b.HasIndex("TextVersionId");

                    b.ToTable("TextLocalisation");
                });

            modelBuilder.Entity("Weapsy.Apps.Text.Data.Entities.TextModule", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("ModuleId");

                    b.Property<int>("Status");

                    b.HasKey("Id");

                    b.ToTable("TextModule");
                });

            modelBuilder.Entity("Weapsy.Apps.Text.Data.Entities.TextVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<string>("Description");

                    b.Property<int>("Status");

                    b.Property<Guid>("TextModuleId");

                    b.HasKey("Id");

                    b.HasIndex("TextModuleId");

                    b.ToTable("TextVersion");
                });

            modelBuilder.Entity("Weapsy.Apps.Text.Data.Entities.TextLocalisation", b =>
                {
                    b.HasOne("Weapsy.Apps.Text.Data.Entities.TextVersion", "TextVersion")
                        .WithMany("TextLocalisations")
                        .HasForeignKey("TextVersionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Weapsy.Apps.Text.Data.Entities.TextVersion", b =>
                {
                    b.HasOne("Weapsy.Apps.Text.Data.Entities.TextModule", "TextModule")
                        .WithMany("TextVersions")
                        .HasForeignKey("TextModuleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
