using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoVsRelational.Data;
using MongoVsRelational.Models;

[Route("api/[controller]")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly AppDbContext _context;

    public DishController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishes()
    {
        return await _context.Dishes.Include(d => d.Ingredients).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Dish>> GetDish(int id)
    {
        var dish = await _context.Dishes.Include(d => d.Ingredients).FirstOrDefaultAsync(d => d.DishId == id);

        if (dish == null) return NotFound();

        return dish;
    }

    [HttpPost]
    public async Task<ActionResult<Dish>> CreateDish(DishCreateDto dishDto)
    {
        // Neues Dish-Objekt erstellen
        var dish = new Dish { Name = dishDto.Name };

        // Finde alle Ingredients, die in IngredientIds spezifiziert sind
        if (dishDto.IngredientIds != null && dishDto.IngredientIds.Count > 0)
        {
            var ingredients = await _context.Ingredients
                .Where(i => dishDto.IngredientIds.Contains(i.IngredientId))
                .ToListAsync();

            dish.Ingredients.AddRange(ingredients);
        }

        // Füge das Dish zur Datenbank hinzu
        _context.Dishes.Add(dish);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetDish), new { id = dish.DishId }, dish);
    }

    // Diese Methode bleibt unverändert
    [HttpPost("{dishId}/ingredients/{ingredientId}")]
    public async Task<IActionResult> AddIngredientToDish(int dishId, int ingredientId)
    {
        var dish = await _context.Dishes.Include(d => d.Ingredients).FirstOrDefaultAsync(d => d.DishId == dishId);
        if (dish == null) return NotFound($"Dish with ID {dishId} not found.");

        var ingredient = await _context.Ingredients.FindAsync(ingredientId);
        if (ingredient == null) return NotFound($"Ingredient with ID {ingredientId} not found.");

        if (!dish.Ingredients.Contains(ingredient))
        {
            dish.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
        }

        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDish(int id, Dish dish)
    {
        if (id != dish.DishId) return BadRequest();

        _context.Entry(dish).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDish(int id)
    {
        var dish = await _context.Dishes.FindAsync(id);
        if (dish == null) return NotFound();

        _context.Dishes.Remove(dish);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
