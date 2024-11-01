using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MongoVsRelational.Models
{
    public class Dish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DishId { get; set; }

        public string Name { get; set; }

        // Die Ingredients-Relation bleibt wie gehabt
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
    }

    public class Ingredient
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IngredientId { get; set; }

        public string Name { get; set; }

        public List<Dish> Dishes { get; set; } = new List<Dish>();
    }
}
