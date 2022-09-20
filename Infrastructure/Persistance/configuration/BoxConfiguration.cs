using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using StoreBackend.Domain.Entity;

namespace StoreBackend.Infrastructure.Persistance.configuration {

    public class BoxConfiguration {
        
        public static ModelBuilder onBuild(ModelBuilder modelBuilder) {

            modelBuilder.Entity<Box>(entity => {
                entity.ToTable("Box");

                entity.HasIndex(e => e.UserId, "box_creator");

                entity.HasIndex(e => e.StoreId, "box_store");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Store)
                    .WithMany(p => p.Boxes)
                    .HasForeignKey(d => d.StoreId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("box_store");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Boxes)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("box_creator");
            });

            return modelBuilder;

        }
    }
}