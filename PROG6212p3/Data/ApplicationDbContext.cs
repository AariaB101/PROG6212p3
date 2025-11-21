using Microsoft.EntityFrameworkCore;
using PROG6212p3.Models;
using System.Collections.Generic;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Claim> Claims { get; set; }
    public DbSet<Lecturer> Lecturers { get; set; }



}