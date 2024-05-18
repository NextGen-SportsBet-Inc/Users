using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SportBetInc.Models;

public class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
    : base(options)
    {
        if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator dbCreater)
        {
            // Create Database 
            if (!dbCreater.CanConnect())
            {
                dbCreater.Create();
            }

            // Create Tables
            if (!dbCreater.HasTables())
            {
                dbCreater.CreateTables();
            }
        }
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //TODO: apagar isto depois de testar a db
        modelBuilder.Entity<User>().HasData(
            new User() { Id = "id1", FirstName = "Roberta", LastName = "Matias", Email = "robertamatias@gmail.com" },
            new User() { Id = "id2", FirstName = "Fernando", LastName = "Mendes", Email = "fernandomendes@gmail.com" },
            new User() { Id = "id3", FirstName = "Maria", LastName = "Fernandes", Email = "mariafernandes@gmail.com" }
        );
    }


}

