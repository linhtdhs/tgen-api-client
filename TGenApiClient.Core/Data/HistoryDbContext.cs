using System.IO;
using Microsoft.EntityFrameworkCore;
using TGenApiClient.Core.Constants;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Data;

/// <summary>
/// Entity Framework Core database context for managing <see cref="HistoryItem"/> records.
/// </summary>
public class HistoryDbContext : DbContext
{
    /// <summary>
    /// Gets or sets the set of history items stored in the database.
    /// </summary>
    public DbSet<HistoryItem> HistoryItems { get; set; } = null!;

    /// <summary>
    /// Initializes a new instance of the <see cref="HistoryDbContext"/> class, ensuring the database is created.
    /// </summary>
    public HistoryDbContext()
    {
        Database.EnsureCreated();
    }

    /// <summary>
    /// Configures the database to use an SQLite file located in the application's base directory.
    /// </summary>
    /// <param name="optionsBuilder">The builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, AppConstants.DatabaseFileName);
        optionsBuilder.UseSqlite(string.Format(AppConstants.DatabaseConnectionStringFormat, dbPath));
    }
}
