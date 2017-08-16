namespace MajorProject.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Actor
    {
        public Actor()
        {

        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ActorId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Display(Name = "Create Date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreateDate { get; set; }

        [Display(Name = "Edit Date")]
        public DateTime EditDate { get; set; } = DateTime.UtcNow;

        [Display(Name = "Movies")]
        [InverseProperty("Actor")]
        public virtual ICollection<MovieActor> Movies { get; set; } = new HashSet<MovieActor>();

        public override string ToString()
        {
            return String.Format("{0}", Name);
        }
        
    }
}
