namespace Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Sieve.Attributes;

    [Table("Recipe")]
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Sieve(CanFilter = true, CanSort = true)]
        public int RecipeId { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Title { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Directions { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string RecipeSourceLink { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string Description { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string ImageLink { get; set; }

        // add-on property marker - Do Not Delete This Comment
    }
}