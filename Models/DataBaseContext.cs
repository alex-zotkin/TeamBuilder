using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamBuilder.Models
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<New> News { get; set; }
        public DbSet<TeamUser> TeamUsers { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }    
        public DbSet<UserMark> UserMarks { get; set; }
        public DbSet<ProjectJury> ProjectJury { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<FileModel> Files { get; set; }

        public DataBaseContext(DbContextOptions options) : base(options)
        {
            //Database.EnsureDeleted();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.Entity<Project>()
                .HasMany(t => t.Teams)
                .WithOne(p => p.Project);*/


            modelBuilder.Entity<Team>()
                .HasKey(x => x.TeamId);

            modelBuilder.Entity<User>()
                .HasKey(x => x.UserId);

            modelBuilder.Entity<New>()
                .HasKey(x => x.NewId);

            modelBuilder.Entity<ProjectUser>()
                .HasKey(x => new { x.ProjectId, x.UserId});
            modelBuilder.Entity<ProjectUser>()
                .HasOne(x => x.Project)
                .WithMany(m => m.Admins)
                .HasForeignKey(x => x.ProjectId);
            modelBuilder.Entity<ProjectUser>()
                .HasOne(x => x.User)
                .WithMany(e => e.AdminProjects)
                .HasForeignKey(x => x.UserId);


            modelBuilder.Entity<ProjectJury>()
                .HasKey(x => new { x.ProjectId, x.UserId });
            modelBuilder.Entity<ProjectJury>()
                .HasOne(x => x.Project)
                .WithMany(m => m.Jury)
                .HasForeignKey(x => x.ProjectId);
            modelBuilder.Entity<ProjectJury>()
                .HasOne(x => x.User)
                .WithMany(e => e.JuryProjects)
                .HasForeignKey(x => x.UserId);


            modelBuilder.Entity<TeamUser>()
                .HasKey(x => new { x.TeamId, x.UserId });
            modelBuilder.Entity<TeamUser>()
                .HasOne(x => x.Team)
                .WithMany(m => m.Users)
                .HasForeignKey(x => x.TeamId);
            modelBuilder.Entity<TeamUser>()
                .HasOne(x => x.User)
                .WithMany(e => e.Teams)
                .HasForeignKey(x => x.UserId);


            


            base.OnModelCreating(modelBuilder);
        }



    }
}
