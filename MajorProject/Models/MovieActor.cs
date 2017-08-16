namespace MajorProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MovieActor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MovieActorId { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Movie")]
        public string MovieId { get; set; }

        [ForeignKey("MovieId")]
        public virtual Movie Movie { get; set; }

        [Required]
        [StringLength(128)]
        [Display(Name = "Actor")]
        public string ActorId { get; set; }

        [ForeignKey("ActorId")]
        public virtual Actor Actor { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        public override string ToString()
        {
            return String.Format("{0} - {1}", Movie.ToString(), Actor.ToString());
        }
    }
}
