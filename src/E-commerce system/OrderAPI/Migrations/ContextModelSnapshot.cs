﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderAPI.DataBase;

#nullable disable

namespace OrderAPI.Migrations
{
    [DbContext(typeof(Context))]
    partial class ContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.2");

            modelBuilder.Entity("OrderAPI.DataBase.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<double>("TotalValue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.CartProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CartId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalValue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("CartId");

                    b.ToTable("CartProducts");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCanceled")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPaymented")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsReady")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsReceived")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.OrderProduct", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<double>("TotalValue")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.CartProduct", b =>
                {
                    b.HasOne("OrderAPI.DataBase.Entities.Cart", "Cart")
                        .WithMany("CartProducts")
                        .HasForeignKey("CartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Cart");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.OrderProduct", b =>
                {
                    b.HasOne("OrderAPI.DataBase.Entities.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.Cart", b =>
                {
                    b.Navigation("CartProducts");
                });

            modelBuilder.Entity("OrderAPI.DataBase.Entities.Order", b =>
                {
                    b.Navigation("OrderProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
