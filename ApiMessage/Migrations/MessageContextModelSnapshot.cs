﻿// <auto-generated />
using System;
using ApiMessage.Contexts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ApiMessage.Migrations
{
    [DbContext(typeof(MessageContext))]
    partial class MessageContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApiMessage.Contexts.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<bool>("Received")
                        .HasColumnType("boolean")
                        .HasColumnName("received");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("text");

                    b.Property<Guid>("ToUserId")
                        .HasColumnType("uuid")
                        .HasColumnName("to_userId");

                    b.Property<Guid?>("UserSenderId")
                        .HasColumnType("uuid");

                    b.HasKey("Id")
                        .HasName("message_pkey");

                    b.HasIndex("UserSenderId");

                    b.ToTable("messages", (string)null);
                });

            modelBuilder.Entity("ApiMessage.Contexts.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("user_id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_email");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("ApiMessage.Contexts.Message", b =>
                {
                    b.HasOne("ApiMessage.Contexts.Models.User", "UserSender")
                        .WithMany("SendMessages")
                        .HasForeignKey("UserSenderId");

                    b.Navigation("UserSender");
                });

            modelBuilder.Entity("ApiMessage.Contexts.Models.User", b =>
                {
                    b.Navigation("SendMessages");
                });
#pragma warning restore 612, 618
        }
    }
}