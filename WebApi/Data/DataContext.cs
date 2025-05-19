using Microsoft.EntityFrameworkCore;
using WebApi.Entity;

namespace WebApi.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<ProfileEntity> Profiles { get; set; }
}
