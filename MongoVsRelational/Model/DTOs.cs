namespace MongoVsRelational.Models
{
    public class DishCreateDto
    {
        public string Name { get; set; }
        public List<int> IngredientIds { get; set; } = new List<int>();
    }
}
