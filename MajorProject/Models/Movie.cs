namespace MajorProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Movie
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MovieId { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Movie Name")]
        public string Name { get; set; }

        [Display(Name = "HitMovie")]
        public bool HitMovie { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Actors")]
        [InverseProperty("Movie")]
        public virtual ICollection<MovieActor> Actors { get; set; } = new HashSet<MovieActor>();

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
    }
}
