using Application.Shared.Models;
using Application.Shared.Models.Admin;
using Application.Shared.Models.Data;
using Application.Shared.Models.Inventory;
using Application.Shared.Models.Org;
using Application.Shared.Models.RealTime;
using Application.Shared.Models.Sales;
using Application.Shared.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Application.Shared.Data
{
    //public delegate ApplicationDbContext DbContextFactory(string companyId);

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        private readonly string _companyId;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, string companyId) : this(options)
        {
            _companyId = companyId;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure the model to use snake case naming
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("application_user");
            });
            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("user_claim");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("user_login");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("token");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.ToTable("role");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("role_claim");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("user_role");
            });


            //modelBuilder.Entity<Item>(b =>
            //{
            //    b.HasMany(e => e.Variants)
            //    .WithOne(e => e.Item)
            //    .HasPrincipalKey(e => new { e.CompanyId, e.ItemNo })
            //    .HasForeignKey(e => new { e.CompanyId, e.ItemNo })
            //    .IsRequired(false);
            //});

            //modelBuilder.Entity<Item>(b =>
            //{
            //    b.HasMany(e => e.VariantOptions)
            //    .WithOne(e => e.Item)
            //    .HasPrincipalKey(e => new { e.CompanyId, e.Code })
            //    .HasForeignKey(e => new { e.CompanyId, e.ItemCode })
            //    .IsRequired(false);
            //});

            //modelBuilder.Entity<Variant>(b =>
            //{
                

            //    b.HasMany(e => e.SalesPrices)
            //    .WithOne(e => e.Variant)
            //    .HasPrincipalKey(e => new { e.CompanyId, e.ItemCode, e.VariantCode })
            //    .HasForeignKey(e => new { e.CompanyId, e.ItemCode, e.VariantCode })
            //    .IsRequired(false);

            //});

            //modelBuilder.Entity<VariantOption>(b =>
            //{
            //    b.HasMany(e => e.Variants)
            //    .WithOne(e => e.VariantOption)
            //    .HasPrincipalKey(e => new { e.CompanyId, e.ItemCode, e.Name })
            //    .HasForeignKey(e => new { e.CompanyId, e.ItemCode, e.VariantOptionName })
            //    .IsRequired(false);
            //});



            //modelBuilder.Entity<DatabaseCredential>(b =>
            //{
            //    b.HasOne<Database>()
            //        .WithOne()
            //        .HasForeignKey<DatabaseCredential>(d => new { d.Host, d.DatabaseName });
            //});




            // add builder to remove the cascading for all the foreign keys in all classes
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }


            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {


                // Convert table names to snake case
                // check if the entity has Table attribute
                if (entity.GetTableName() != null)
                {
                    entity.SetTableName(ToSnakeCase(entity.GetTableName()));
                }


                // Convert column names to snake case
                foreach (var property in entity.GetProperties())
                {
                    // get the attributes of the property
                    var attributes = property.PropertyInfo.GetCustomAttributesData();

                    // check if the column does not have Column attribute
                    if (!attributes.Any(a => a.AttributeType.Name == "ColumnAttribute"))
                    {
                        property.SetColumnName(ToSnakeCase(property.Name));
                    }
                }
            }
        }


        public DbSet<ApplicationPage> ApplicationPage { get; set; }
        public DbSet<ApplicationPageRole> ApplicationPageRole { get; set; }
        public DbSet<UserAccessRequest> UserAccessRequest { get; set; }



        public DbSet<Company> Company { get; set; }
        
        public DbSet<CompanyMember> CompanyMember { get; set; }

        public DbSet<CompanyDomain> CompanyDomain { get; set; }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<SalesChannel> SalesChannel { get; set; }
        public DbSet<SalesForecastBySalesChannel> SalesForecastBySalesChannel { get; set; }

        public DbSet<Table> Table { get; set; }

        public DbSet<Column> Column { get; set; }

        public DbSet<SalesLineRealTime> SalesLineRealTime { get; set; }

        public DbSet<DataFile> DataFile { get; set; }
        
        public DbSet<DataFileAccess> DataFileAccess { get; set; }
        public DbSet<FieldMapping> FieldMapping { get; set; }
        public DbSet<Item> Item { get; set; }
        //public DbSet<Variant> Variant { get; set; }


        public DbSet<Host> Host { get; set; }
        public DbSet<Database> Database { get; set; }
        public DbSet<DatabaseCredential> DatabaseCredential { get; set; }
        public DbSet<TableStorageUsage> TableStorageUsage { get; set; }
        public DbSet<QueryDetail> QueryDetail { get; set; }



        /// <summary>
        /// Converts a given string to snake case.
        /// </summary>
        /// <param name="input">The input string to be converted.</param>
        /// <returns>The converted string in snake case.</returns>
        private static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var stringBuilder = new StringBuilder();
            var previousCharWasUpper = false;

            foreach (var character in input)
            {
                if (char.IsUpper(character))
                {
                    if (stringBuilder.Length != 0 && !previousCharWasUpper)
                    {
                        stringBuilder.Append('_');
                    }
                    stringBuilder.Append(char.ToLowerInvariant(character));
                    previousCharWasUpper = true;
                }
                else
                {
                    stringBuilder.Append(character);
                    previousCharWasUpper = false;
                }
            }

            return stringBuilder.ToString();
        }
    }
}
