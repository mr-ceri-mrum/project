using Microsoft.EntityFrameworkCore;
using SaveIMGAPI.Models;

namespace SaveIMGAPI;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        /*AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);*/
    }
    
    public virtual DbSet<User> Users { get; set; }
}