using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoVsRelational.Data;
using MongoVsRelational.Models;

[Route("api/[controller]")]
[ApiController]
public class IngredientController : ControllerBase
{
    private readonly AppDbContext _context;

    public IngredientController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
    {
        return await _context.Ingredients.Include(i => i.Dishes).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Ingredient>> GetIngredient(int id)
    {
        var ingredient = await _context.Ingredients.Include(i => i.Dishes).FirstOrDefaultAsync(i => i.IngredientId == id);

        if (ingredient == null) return NotFound();

        return ingredient;
    }

    [HttpPost]
    public async Task<ActionResult<Ingredient>> CreateIngredient(Ingredient ingredient)
    {
        _context.Ingredients.Add(ingredient);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetIngredient), new { id = ingredient.IngredientId }, ingredient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, Ingredient ingredient)
    {
        if (id != ingredient.IngredientId) return BadRequest();

        _context.Entry(ingredient).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var ingredient = await _context.Ingredients.FindAsync(id);
        if (ingredient == null) return NotFound();

        _context.Ingredients.Remove(ingredient);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
