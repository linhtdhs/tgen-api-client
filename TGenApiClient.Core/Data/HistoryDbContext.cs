using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using TGenApiClient.Core.Models;

namespace TGenApiClient.Core.Data;

public class HistoryDbContext : DbContext
{
    public DbSet<HistoryItem> HistoryItems { get; set; } = null!;

    public HistoryDbContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(AppContext.BaseDirectory, "tgenapi_history.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
}
