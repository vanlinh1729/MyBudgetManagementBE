﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyBudgetManagement.Persistence.Context;

#nullable disable

namespace MyBudgetManagement.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250616082043_Add Nullable into note field")]
    partial class AddNullableintonotefield
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Categories.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal?>("Budget")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Level")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Period")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DebtContactId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDebt")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DebtContactId");

                    b.ToTable("DebtAndLoans");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoanContact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankingNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Relationship")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("DebtAndLoanContacts");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalSpent")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupExpense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("ExpenseDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberId");

                    b.ToTable("GroupExpenses");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupExpenseShare", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("AmountPaid")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("PaidDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberId");

                    b.ToTable("GroupExpenseShares");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupInvitation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpiredAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InviteeId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("InviterId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RespondedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("InviteeId");

                    b.HasIndex("InviterId");

                    b.ToTable("GroupInvitations");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("InvitationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsGroupLeader")
                        .HasColumnType("bit");

                    b.Property<DateTime>("JoinDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("NetBalance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("InvitationId");

                    b.HasIndex("UserId");

                    b.ToTable("GroupMembers");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupMemberId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsPinned")
                        .HasColumnType("bit");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("ParentMessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("GroupMemberId");

                    b.HasIndex("ParentMessageId");

                    b.ToTable("GroupMessages");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Notifications.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<int>("RelatedEntityType")
                        .HasColumnType("int");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.RolePermission", b =>
                {
                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PermissionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("RoleId", "PermissionId");

                    b.HasIndex("PermissionId");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Transactions.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("DebtAndLoanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Image")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserBalanceId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DebtAndLoanId");

                    b.HasIndex("UserBalanceId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.Token", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpireAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("TokenValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Tokens");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Currency")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<DateTime?>("LastChangePassword")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.UserBalance", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("UserBalances");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.UserRole", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("UpdatedBy")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Categories.Category", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithMany("Categories")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoan", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Categories.Category", "Category")
                        .WithMany("DebtAndLoans")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoanContact", "DebtAndLoanContact")
                        .WithMany("DebtAndLoans")
                        .HasForeignKey("DebtContactId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("DebtAndLoanContact");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupExpense", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.Group", "Group")
                        .WithMany("Expenses")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.GroupMember", "Member")
                        .WithMany("Expenses")
                        .HasForeignKey("GroupMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupExpenseShare", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.GroupMember", "Member")
                        .WithMany("ExpenseShares")
                        .HasForeignKey("GroupMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupInvitation", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.Group", "Group")
                        .WithMany("Invitations")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "Invitee")
                        .WithMany("ReceivedInvitations")
                        .HasForeignKey("InviteeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "Inviter")
                        .WithMany("SentInvitations")
                        .HasForeignKey("InviterId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Invitee");

                    b.Navigation("Inviter");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupMember", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.Group", "Group")
                        .WithMany("Members")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.GroupInvitation", "Invitation")
                        .WithMany()
                        .HasForeignKey("InvitationId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithMany("GroupMemberships")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("Invitation");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupMessage", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.Group", "Group")
                        .WithMany("Messages")
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.GroupMember", "GroupMember")
                        .WithMany()
                        .HasForeignKey("GroupMemberId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Groups.GroupMessage", "ParentMessage")
                        .WithMany()
                        .HasForeignKey("ParentMessageId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Group");

                    b.Navigation("GroupMember");

                    b.Navigation("ParentMessage");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Notifications.Notification", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "Sender")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Sender");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.RolePermission", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Roles.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Roles.Role", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Transactions.Transaction", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Categories.Category", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoan", "DebtAndLoan")
                        .WithMany("Transactions")
                        .HasForeignKey("DebtAndLoanId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.UserBalance", "UserBalance")
                        .WithMany("Transactions")
                        .HasForeignKey("UserBalanceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("DebtAndLoan");

                    b.Navigation("UserBalance");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.Token", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.UserBalance", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithOne("UserBalance")
                        .HasForeignKey("MyBudgetManagement.Domain.Entities.Users.UserBalance", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.UserRole", b =>
                {
                    b.HasOne("MyBudgetManagement.Domain.Entities.Roles.Role", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyBudgetManagement.Domain.Entities.Users.User", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Categories.Category", b =>
                {
                    b.Navigation("DebtAndLoans");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoan", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Debts.DebtAndLoanContact", b =>
                {
                    b.Navigation("DebtAndLoans");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.Group", b =>
                {
                    b.Navigation("Expenses");

                    b.Navigation("Invitations");

                    b.Navigation("Members");

                    b.Navigation("Messages");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Groups.GroupMember", b =>
                {
                    b.Navigation("ExpenseShares");

                    b.Navigation("Expenses");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Roles.Role", b =>
                {
                    b.Navigation("RolePermissions");

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.User", b =>
                {
                    b.Navigation("Categories");

                    b.Navigation("GroupMemberships");

                    b.Navigation("ReceivedInvitations");

                    b.Navigation("ReceivedNotifications");

                    b.Navigation("SentInvitations");

                    b.Navigation("SentNotifications");

                    b.Navigation("Tokens");

                    b.Navigation("UserBalance")
                        .IsRequired();

                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("MyBudgetManagement.Domain.Entities.Users.UserBalance", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
