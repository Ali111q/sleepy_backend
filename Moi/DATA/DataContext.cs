using System.Reflection;
using BackEndStructuer.Entities;
using GaragesStructure.DATA.DTOs;
using GaragesStructure.Entities;
using GaragesStructure.Utils;
using Microsoft.EntityFrameworkCore;

namespace GaragesStructure.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        public DbSet<AppUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }


        // here to add
public DbSet<Music> Musics { get; set; }
public DbSet<Category> Categorys { get; set; }
        public DbSet<LoginLogger> LoginLoggers { get; set; }

        public DbSet<Country> Countries { get; set; }


        public DbSet<Notifications> Notifications { get; set; }

        // public DbSet<DriverVehicle> DriverVehicles { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RolePermission>().HasKey(rp => new { rp.RoleId, rp.PermissionId });
            modelBuilder.Entity<Music>().HasOne<Category>(e => e.Category).WithMany(e => e.Musics)
                .HasForeignKey(e => e.CategoryId);
            // new DbInitializer(modelBuilder).Seed()
            // ;
        }



        public virtual async Task<int> SaveChangesAsync(Guid? userId = null)
        {
            // await OnBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync();
            return result;
        }

        // private async Task OnBeforeSaveChanges(Guid? userId)
        // {
        //     try
        //     {
        //         ChangeTracker.DetectChanges();
        //
        //         var auditEntries = new List<AuditEntry>();
        //
        //         foreach (var entry in ChangeTracker.Entries()
        //                      .Where(e => e.Entity is not Audit &&
        //                                  (e.State is
        //                                      EntityState.Added or
        //                                      EntityState.Modified or
        //                                      EntityState.Deleted) &&
        //                                  e.Entity.GetType().GetCustomAttribute<AuditTrailAttribute>() != null))
        //         {
        //             var auditEntry = new AuditEntry(entry)
        //             {
        //                 TableName = entry.Metadata.GetTableName(),
        //                 UserId = userId
        //             };
        //
        //             foreach (var property in entry.Properties)
        //             {
        //                 if (property.Metadata.IsPrimaryKey())
        //                 {
        //                     auditEntry.KeyValues[property.Metadata.Name] = property.CurrentValue;
        //                     continue;
        //                 }
        //
        //                 if (entry.State == EntityState.Added)
        //                 {
        //                     auditEntry.AuditType = AuditType.Create;
        //                     auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
        //                 }
        //                 else if (entry.State == EntityState.Deleted)
        //                 {
        //                     auditEntry.AuditType = AuditType.Delete;
        //                     auditEntry.OldValues[property.Metadata.Name] =
        //                         entry.GetDatabaseValues()[property.Metadata] ?? DBNull.Value;
        //                 }
        //
        //                 else if (entry.State == EntityState.Modified && property.IsModified)
        //                 {
        //                     auditEntry.AuditType = AuditType.Update;
        //                     auditEntry.OldValues[property.Metadata.Name] =
        //                         entry.GetDatabaseValues()[property.Metadata] ?? DBNull.Value;
        //                     auditEntry.NewValues[property.Metadata.Name] = property.CurrentValue;
        //                 }
        //             }
        //
        //             auditEntries.Add(auditEntry);
        //         }
        //
        //         var databaseValues = await Task.WhenAll(auditEntries
        //             .Where(e => e.AuditType != AuditType.Create)
        //             .Select(async entry => new
        //             {
        //                 Entry = entry,
        //                 DatabaseValues = await entry.Entry.GetDatabaseValuesAsync()
        //             }));
        //
        //         foreach (var item in databaseValues)
        //         {
        //             var auditEntry = item.Entry;
        //             foreach (var property in auditEntry.Entry.Properties)
        //             {
        //                 var propertyName = property.Metadata.Name;
        //                 if (!auditEntry.ChangedColumns.Contains(propertyName))
        //                     auditEntry.ChangedColumns.Add(propertyName);
        //
        //                 if (auditEntry.AuditType == AuditType.Delete)
        //                 {
        //                     auditEntry.OldValues[propertyName] = item.DatabaseValues[propertyName] ?? DBNull.Value;
        //                 }
        //             }
        //         }
        //
        //         var auditLogs = auditEntries
        //             .Select(auditEntry => auditEntry.ToAudit(
        //                 auditEntry.TableName,
        //                 auditEntry.Entry.OriginalValues.GetValue<Guid>("Id"),
        //                 DateTime.Now))
        //             .ToList();
        //
        //         await AuditLogs.AddRangeAsync(auditLogs);
        //     }
        //     catch (Exception e)
        //     {
        //         // Handle exceptions gracefully, log them appropriately
        //         Console.WriteLine("Exception during audit: " + e);
        //         throw;
        //     }
        // }
    }
}
