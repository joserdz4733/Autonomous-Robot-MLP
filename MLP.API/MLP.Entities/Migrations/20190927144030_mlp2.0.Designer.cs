﻿// <auto-generated />
using System;
using MLP.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MLP.API.Migrations
{
    [DbContext(typeof(MLPContext))]
    [Migration("20190927144030_mlp2.0")]
    partial class mlp20
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MLP.Entities.ImageProcessingConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Active");

                    b.Property<double>("BlueAvg");

                    b.Property<double>("BlueStd");

                    b.Property<string>("ConfigName")
                        .IsRequired();

                    b.Property<double>("GreenAvg");

                    b.Property<double>("GreenStd");

                    b.Property<int>("ImageFilter");

                    b.Property<int>("ImageFilterSize");

                    b.Property<Guid>("NeuralNetworkId");

                    b.Property<double>("RedAvg");

                    b.Property<double>("RedStd");

                    b.Property<double>("ValuesFactor");

                    b.Property<int>("resizeSize");

                    b.HasKey("Id");

                    b.ToTable("ImageProcessingConfigs");
                });

            modelBuilder.Entity("MLP.Entities.NeuralNetwork", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("NeuralNetworks");
                });

            modelBuilder.Entity("MLP.Entities.NeuralNetworkTrainingConfig", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Epochs");

                    b.Property<double>("Eta");

                    b.Property<int>("HiddenActivationFunction");

                    b.Property<int>("HiddenNeuronElements");

                    b.Property<int>("InputSize");

                    b.Property<Guid>("NeuralNetworkId");

                    b.Property<int>("OutputActivationFunction");

                    b.Property<int>("OutputNeuronElements");

                    b.Property<string>("TrainingDatabaseFileName")
                        .IsRequired();

                    b.Property<string>("TrainingDatabaseFileRoute")
                        .IsRequired();

                    b.Property<string>("TrainingDatabaseTestFileName")
                        .IsRequired();

                    b.Property<int>("TrainingDatabaseType");

                    b.Property<double>("WeightsFactor");

                    b.HasKey("Id");

                    b.HasIndex("NeuralNetworkId")
                        .IsUnique();

                    b.ToTable("NeuralNetworkdTrainingConfigs");
                });

            modelBuilder.Entity("MLP.Entities.Neuron", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Bias");

                    b.Property<int>("Index");

                    b.Property<Guid>("NeuralNetworkId");

                    b.Property<int>("NeuronType");

                    b.HasKey("Id");

                    b.HasIndex("NeuralNetworkId");

                    b.ToTable("Neurons");
                });

            modelBuilder.Entity("MLP.Entities.NeuronWeight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index");

                    b.Property<int>("NeuronId");

                    b.Property<double>("Weight");

                    b.HasKey("Id");

                    b.HasIndex("NeuronId");

                    b.ToTable("NeuronWeights");
                });

            modelBuilder.Entity("MLP.Entities.PredictedObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Index");

                    b.Property<int>("NeuralNetworkTrainingConfigId");

                    b.Property<string>("ObjectName")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("NeuralNetworkTrainingConfigId");

                    b.ToTable("PredictedObjects");
                });

            modelBuilder.Entity("MLP.Entities.NeuralNetworkTrainingConfig", b =>
                {
                    b.HasOne("MLP.Entities.NeuralNetwork")
                        .WithOne("TrainingConfig")
                        .HasForeignKey("MLP.Entities.NeuralNetworkTrainingConfig", "NeuralNetworkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MLP.Entities.Neuron", b =>
                {
                    b.HasOne("MLP.Entities.NeuralNetwork")
                        .WithMany("Neurons")
                        .HasForeignKey("NeuralNetworkId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MLP.Entities.NeuronWeight", b =>
                {
                    b.HasOne("MLP.Entities.Neuron")
                        .WithMany("Weights")
                        .HasForeignKey("NeuronId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MLP.Entities.PredictedObject", b =>
                {
                    b.HasOne("MLP.Entities.NeuralNetworkTrainingConfig")
                        .WithMany("PredictedObjects")
                        .HasForeignKey("NeuralNetworkTrainingConfigId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
