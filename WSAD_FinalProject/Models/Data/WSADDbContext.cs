using System;
using System.Data.Entity;

namespace WSAD_FinalProject.Models.Data
{
    public class WSADDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Session> Sessions { get; set; }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<SessionCart> SessionCartItems { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<UserRole> UserRoles { get; set; }

    }
}