using System.Text.Json;

using ContactsManager.Core.Domain.IdentityEntities;

using Entities;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ContactsManager.Infrastructure.DbContext;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
  public ApplicationDbContext() { }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
      base(options)
  {
  }

  // virtual 用于mock测试
  public virtual DbSet<Country>? Countries { get; set; }
  public virtual DbSet<Person>?  Persons   { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Country>().ToTable("Countries");
    modelBuilder.Entity<Person>().ToTable("Persons");

    //Seed to Countries
    var countriesJson = File.ReadAllText("countries.json");

    var countries =
        JsonSerializer.Deserialize<List<Country>>(countriesJson);

    foreach (var country in countries) modelBuilder.Entity<Country>().HasData(country);

    //Seed to Persons
    var personsJson = File.ReadAllText("persons.json");
    var persons = JsonSerializer.Deserialize<List<Person>>(personsJson);

    foreach (var person in persons) modelBuilder.Entity<Person>().HasData(person);

    //Fluent API
    modelBuilder.Entity<Person>()
                .Property(temp => temp.TIN)
                .HasColumnName("TaxIdentificationNumber")
                .HasColumnType("varchar(8)")
                .HasDefaultValue("ABC12345");

    //modelBuilder.Entity<Person>()
    //  .HasIndex(temp => temp.TIN).IsUnique();

    modelBuilder.Entity<Person>()
                .HasCheckConstraint("CHK_TIN", "len([TaxIdentificationNumber]) = 8");

    //多对一
    modelBuilder.Entity<Person>(entity =>
    {
      entity.HasOne(c => c.Country)
            .WithMany(p => p.Persons)
            .HasForeignKey(p => p.CountryID);
    });
  }

  // Stored Procedure (SP) - Get All Persons
  public List<Person> sp_GetAllPersons()
  {
    // 调用dbo schema下的GetAllPersons存储过程
    return Persons.FromSqlRaw("EXECUTE [dbo].[GetAllPersons]").ToList();
  }

  public int sp_InsertPerson(Person person)
  {
    SqlParameter[ ] parameters =
    {
        new("@PersonID", person.PersonID),
        new("@PersonName", person.PersonName),
        new("@Email", person.Email),
        new("@DateOfBirth", person.DateOfBirth),
        new("@Gender", person.Gender),
        new("@CountryID", person.CountryID),
        new("@Address", person.Address),
        new("@ReceiveNewsLetters", person.ReceiveNewsLetters)
    };

    return
        Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertPerson] @PersonID, @PersonName, @Email, @DateOfBirth, @Gender, @CountryID, @Address, @ReceiveNewsLetters",
                               parameters);
  }
}
