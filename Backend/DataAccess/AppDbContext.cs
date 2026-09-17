using Microsoft.EntityFrameworkCore;
using ModelInterface.Companys;
using ModelInterface.Devices;
using ModelInterface.Homes;
using ModelInterface.Notifications;
using ModelInterface.Users;
using ModelInterface.Users.UserType;
using Models.Company;
using Models.Devices;
using Models.Homes;
using Models.Notifications;
using Models.Sessions;
using Models.Users;
using Models.Users.UserTypes;

namespace DataAccess;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
    public DbSet<HomeUser> HomeUsers { get; set; }
    public DbSet<CompanyOwner> CompanyOwners { get; set; }
    
    public DbSet<ADevice> Devices { get; set; }
    public DbSet<Camera> Cameras { get; set; }
    public DbSet<Sensor> Sensors { get; set; }
    public DbSet<MotionSensor> MotionSensors { get; set; }
    public DbSet<SmartLamp> SmartLamps { get; set; }
    
    public DbSet<HomeDevice> HomeDevices { get; set; }
    
    public DbSet<Home> Homes { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<HomeMember> HomeMembers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    public DbSet<Company> Companies { get; set; }
    
    public DbSet<Session> Sessions { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Admin>().ToTable("Admins");
        modelBuilder.Entity<HomeUser>().ToTable("HomeUsers");
        modelBuilder.Entity<CompanyOwner>().ToTable("CompanyOwners");

        modelBuilder.Entity<AUser>().UseTpcMappingStrategy();
        modelBuilder.Entity<User>().ToTable("Users");

        modelBuilder.Entity<ADevice>().ToTable("Devices").UseTptMappingStrategy();
        modelBuilder.Entity<Camera>().ToTable("Cameras");
        modelBuilder.Entity<Sensor>().ToTable("Sensors");
        modelBuilder.Entity<MotionSensor>().ToTable("MotionSensors");
        modelBuilder.Entity<SmartLamp>().ToTable("SmartLamps");
        
        modelBuilder.Entity<ADevice>()
            .HasOne(d => d.Company)
            .WithMany(c => c.Devices)
            .HasForeignKey(d => d.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AHome>().UseTpcMappingStrategy();
        modelBuilder.Entity<Home>().ToTable("Homes");

        modelBuilder.Entity<AHomeMember>().UseTpcMappingStrategy();
        modelBuilder.Entity<AHomeMember>()
            .HasKey(hm => new { hm.HomeId, hm.UserId });

        modelBuilder.Entity<Home>()
            .HasOne(h => h.Owner)
            .WithMany()
            .HasForeignKey(h => h.HomeOwnerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AHomeMember>()
            .HasOne(hm => hm.Home)
            .WithMany(h => h.Members)
            .HasForeignKey(hm => hm.HomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AHomeMember>()
            .HasOne(hm => hm.AHomeUser)
            .WithMany(hu => hu.Homes)
            .HasForeignKey(hm => hm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Home>()
            .Property(h => h.MaxMembers)
            .IsRequired();

        modelBuilder.Entity<ANotification>().UseTpcMappingStrategy();
        modelBuilder.Entity<Notification>().ToTable("Notifications");

        
        modelBuilder.Entity<ACompany>().UseTpcMappingStrategy();
        modelBuilder.Entity<Company>().ToTable("Companies");

        modelBuilder.Entity<Company>()
            .HasOne(c => c.CompanyOwner)
            .WithOne()
            .HasForeignKey<Company>(c => c.CompanyOwnerId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();
      
        modelBuilder.Entity<AHomeDevice>().UseTpcMappingStrategy();
        modelBuilder.Entity<HomeDevice>().ToTable("HomeDevices");

        modelBuilder.Entity<AHomeDevice>()
            .HasOne(hd => hd.Device)
            .WithMany()
            .HasForeignKey(hd => hd.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AHomeDevice>()
            .HasOne(hd => hd.Home)
            .WithMany(h => h.Devices)
            .HasForeignKey(hd => hd.HomeId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AHomeDevice>()
            .HasKey(hd => hd.HardwareId);

        modelBuilder.Entity<AHomeDevice>()
            .HasOne(hd => hd.Room)
            .WithMany()
            .HasForeignKey(hd => hd.RoomId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ANotification>().HasOne(n => n.Device).WithMany().OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<ANotification>().HasOne(n => n.User).WithMany().OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Session>().ToTable("Sessions");

        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ARoom>().UseTpcMappingStrategy();
        modelBuilder.Entity<Room>().ToTable("Rooms");
        
        modelBuilder.Entity<ARoom>()
            .HasOne(r => r.Home)
            .WithMany(h => h.Rooms)
            .HasForeignKey(r => r.HomeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AHomeDevice>()
            .HasOne(d => d.Home)
            .WithMany(h => h.Devices)
            .HasForeignKey(d => d.HomeId);

        modelBuilder.Entity<AHomeDevice>()
            .HasOne(d => d.Room)
            .WithMany(r => r.Devices)
            .HasForeignKey(d => d.RoomId);
        
        
        
        modelBuilder.Entity<AUser>()
            .HasMany(u => u.Roles)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
        
        
        modelBuilder.Entity<AUserType>().UseTpcMappingStrategy();
        
        modelBuilder.Entity<AUserType>()
            .HasOne(ut => ut.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(ut => ut.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<AUserType>()
            .HasKey(ut => ut.Id);

        modelBuilder.Entity<AUserType>()
            .Property(ut => ut.Id)
            .ValueGeneratedOnAdd();
        
        modelBuilder.Entity<DevicePhoto>()
            .HasOne(dp => dp.Device)
            .WithMany(d => d.Photos)
            .HasForeignKey(dp => dp.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
            
            
        
         
    }
}