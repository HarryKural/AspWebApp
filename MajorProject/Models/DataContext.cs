namespace MajorProject.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class DataContext : DbContext
    {
        public DataContext()
            : base("name=DataContext")
        {
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Actor> Actors { get; set; }
        public virtual DbSet<MovieActor> MovieActors { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Actor>()
                .HasMany(e => e.Movies)
                .WithRequired(e => e.Actor)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Movie>()
                .HasMany(e => e.Actors)
                .WithRequired(e => e.Movie)
                .WillCascadeOnDelete(false);
        }
    }
}
