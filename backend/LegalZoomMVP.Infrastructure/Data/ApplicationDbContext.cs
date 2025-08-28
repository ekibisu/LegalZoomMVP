using LegalZoomMVP.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegalZoomMVP.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FormTemplate> FormTemplates { get; set; }
        public DbSet<UserForm> UserForms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<AIConversation> AIConversations { get; set; }
        public DbSet<AIMessage> AIMessages { get; set; }
        public DbSet<Advocate> Advocates { get; set; }
        public DbSet<PowerOfAttorney> PowerOfAttorneys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Advocate configuration
            modelBuilder.Entity<Advocate>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.NationalId).HasMaxLength(50);
                entity.Property(e => e.PassportNumber).HasMaxLength(50);
                entity.Property(e => e.Gender).HasMaxLength(20);
                entity.Property(e => e.LskP105).HasMaxLength(50);
                entity.Property(e => e.MobileNumber).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(255);
                entity.Property(e => e.PasswordHash).HasMaxLength(255);
                entity.Property(e => e.Role).HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            {
                base.OnModelCreating(modelBuilder);

                // User configuration
                modelBuilder.Entity<User>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.HasIndex(e => e.Email).IsUnique();
                    entity.Property(e => e.Email).HasMaxLength(255);
                    entity.Property(e => e.FirstName).HasMaxLength(100);
                    entity.Property(e => e.LastName).HasMaxLength(100);
                    entity.Property(e => e.Role).HasConversion<int>();
                });

                // FormTemplate configuration
                modelBuilder.Entity<FormTemplate>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Name).HasMaxLength(255);
                    entity.Property(e => e.Category).HasMaxLength(100);
                    entity.Property(e => e.Price).HasPrecision(10, 2);
                    entity.Property(e => e.FormSchema).HasColumnType("text");
                    entity.Property(e => e.HtmlTemplate).HasColumnType("text");

                    entity.HasOne(e => e.CreatedBy)
                          .WithMany()
                          .HasForeignKey(e => e.CreatedByUserId)
                          .OnDelete(DeleteBehavior.Restrict);
                });

                // UserForm configuration
                modelBuilder.Entity<UserForm>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.FormData).HasColumnType("text");
                    entity.Property(e => e.Status).HasConversion<int>();

                    entity.HasOne(e => e.User)
                          .WithMany(u => u.UserForms)
                          .HasForeignKey(e => e.UserId)
                          .OnDelete(DeleteBehavior.Cascade);

                    entity.HasOne(e => e.FormTemplate)
                          .WithMany(f => f.UserForms)
                          .HasForeignKey(e => e.FormTemplateId)
                          .OnDelete(DeleteBehavior.Restrict);
                });

                // Payment configuration
                modelBuilder.Entity<Payment>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Amount).HasPrecision(10, 2);
                    entity.Property(e => e.Currency).HasMaxLength(3);
                    entity.Property(e => e.Status).HasConversion<int>();
                    entity.Property(e => e.Type).HasConversion<int>();

                    entity.HasOne(e => e.User)
                          .WithMany(u => u.Payments)
                          .HasForeignKey(e => e.UserId)
                          .OnDelete(DeleteBehavior.Restrict);

                    entity.HasOne(e => e.FormTemplate)
                          .WithMany()
                          .HasForeignKey(e => e.FormTemplateId)
                          .OnDelete(DeleteBehavior.SetNull);

                    entity.HasOne(e => e.Subscription)
                          .WithMany(s => s.Payments)
                          .HasForeignKey(e => e.SubscriptionId)
                          .OnDelete(DeleteBehavior.SetNull);
                });

                // Subscription configuration
                modelBuilder.Entity<Subscription>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.PlanName).HasMaxLength(100);
                    entity.Property(e => e.MonthlyPrice).HasPrecision(10, 2);
                    entity.Property(e => e.Status).HasConversion<int>();

                    entity.HasOne(e => e.User)
                          .WithOne(u => u.Subscription)
                          .HasForeignKey<Subscription>(e => e.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

                // AIConversation configuration
                modelBuilder.Entity<AIConversation>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Title).HasMaxLength(255);

                    entity.HasOne(e => e.User)
                          .WithMany(u => u.AIConversations)
                          .HasForeignKey(e => e.UserId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

                // AIMessage configuration
                modelBuilder.Entity<AIMessage>(entity =>
                {
                    entity.HasKey(e => e.Id);
                    entity.Property(e => e.Content).HasColumnType("text");
                    entity.Property(e => e.Role).HasConversion<int>();

                    entity.HasOne(e => e.Conversation)
                          .WithMany(c => c.Messages)
                          .HasForeignKey(e => e.ConversationId)
                          .OnDelete(DeleteBehavior.Cascade);
                });

                // Seed data
                //SeedData(modelBuilder);
            }
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Seed admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Email = "admin@legalzoom.com",
                    PasswordHash = "$2a$11$DkK9mPZgkQrh4cfh4V7Z2uFYCN8oHmc0PpHwnCY8oqC2JwAw2LhHC", // "Admin123!"
                    FirstName = "System",
                    LastName = "Administrator",
                    Role = UserRole.Admin,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                }
            );

            // Seed sample form templates
            modelBuilder.Entity<FormTemplate>().HasData(
                new FormTemplate
                {
                    Id = 1,
                    Name = "Non-Disclosure Agreement (NDA)",
                    Description = "Protect confidential information with our comprehensive NDA template",
                    Category = "Business",
                    Price = 29.99m,
                    IsPremium = true,
                    IsActive = true,
                    FormSchema = @"{
                        ""fields"": [
                            {""name"": ""disclosingParty"", ""label"": ""Disclosing Party"", ""type"": ""text"", ""required"": true},
                            {""name"": ""receivingParty"", ""label"": ""Receiving Party"", ""type"": ""text"", ""required"": true},
                            {""name"": ""effectiveDate"", ""label"": ""Effective Date"", ""type"": ""date"", ""required"": true},
                            {""name"": ""purpose"", ""label"": ""Purpose of Disclosure"", ""type"": ""textarea"", ""required"": true}
                        ]
                    }",
                    HtmlTemplate = @"
                        <div>
                            <h1>NON-DISCLOSURE AGREEMENT</h1>
                            <p>This Non-Disclosure Agreement is entered into on {{effectiveDate}} between {{disclosingParty}} and {{receivingParty}}.</p>
                            <p><strong>Purpose:</strong> {{purpose}}</p>
                        </div>",
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                },
                new FormTemplate
                {
                    Id = 2,
                    Name = "Simple Will",
                    Description = "Create a basic will to protect your assets and family",
                    Category = "Estate Planning",
                    Price = 49.99m,
                    IsPremium = true,
                    IsActive = true,
                    FormSchema = @"{
                        ""fields"": [
                            {""name"": ""testatorName"", ""label"": ""Your Full Name"", ""type"": ""text"", ""required"": true},
                            {""name"": ""testatorAddress"", ""label"": ""Your Address"", ""type"": ""textarea"", ""required"": true},
                            {""name"": ""executor"", ""label"": ""Executor Name"", ""type"": ""text"", ""required"": true},
                            {""name"": ""beneficiaries"", ""label"": ""Beneficiaries"", ""type"": ""textarea"", ""required"": true}
                        ]
                    }",
                    HtmlTemplate = @"
                        <div>
                            <h1>LAST WILL AND TESTAMENT</h1>
                            <p>I, {{testatorName}}, residing at {{testatorAddress}}, being of sound mind, do hereby make this my Last Will and Testament.</p>
                            <p>I appoint {{executor}} as the Executor of this Will.</p>
                            <p><strong>Beneficiaries:</strong> {{beneficiaries}}</p>
                        </div>",
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                },
                new FormTemplate
                {
                    Id = 3,
                    Name = "Basic Contract Template",
                    Description = "A free basic contract template for simple agreements",
                    Category = "Contracts",
                    Price = 0m,
                    IsPremium = false,
                    IsActive = true,
                    FormSchema = @"{
                        ""fields"": [
                            {""name"": ""party1"", ""label"": ""First Party"", ""type"": ""text"", ""required"": true},
                            {""name"": ""party2"", ""label"": ""Second Party"", ""type"": ""text"", ""required"": true},
                            {""name"": ""terms"", ""label"": ""Contract Terms"", ""type"": ""textarea"", ""required"": true},
                            {""name"": ""date"", ""label"": ""Contract Date"", ""type"": ""date"", ""required"": true}
                        ]
                    }",
                    HtmlTemplate = @"
                        <div>
                            <h1>CONTRACT AGREEMENT</h1>
                            <p>This agreement is made on {{date}} between {{party1}} and {{party2}}.</p>
                            <p><strong>Terms:</strong> {{terms}}</p>
                        </div>",
                    CreatedAt = DateTime.UtcNow,
                    CreatedByUserId = 1
                }
            );
        }
    }
}