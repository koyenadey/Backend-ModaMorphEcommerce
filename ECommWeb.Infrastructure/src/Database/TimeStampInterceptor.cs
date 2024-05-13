using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ECommWeb.Core.src.Entity;

namespace ECommWeb.Infrastructure.src.Database;

public class TimeStampInteceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var entries = eventData?.Context?.ChangeTracker.Entries(); // get all monitored entries

        var addedEntries = entries?.Where(e => e.State == EntityState.Added); // get all added entries
        var modifiedEntries = entries?.Where(e => e.State == EntityState.Modified); // get all modified entries

        foreach (var entry in addedEntries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                baseEntity.CreatedAt = DateTime.Now;
                baseEntity.UpdatedAt = DateTime.Now;
            }
        }

        foreach (var entry in addedEntries)
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                baseEntity.UpdatedAt = DateTime.Now;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
