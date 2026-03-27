using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourWebApp.Infrastructure.Data;
using TourWebApp.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

[Authorize] // Само логнати потребители имат достъп
public class FavoriteController : Controller
{
    private readonly ApplicationDbContext _context;

    public FavoriteController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ТОВА Е МЕТОДЪТ TOGGLE
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(int productId)
    {
        // 1. Вземаме ID-то на текущия логнат потребител
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (userId == null) return Challenge();

        // 2. Проверяваме дали този запис вече съществува в базата
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (favorite != null)
        {
            // Ако го има - МАХАМЕ ГО
            _context.Favorites.Remove(favorite);
        }
        else
        {
            // Ако го няма - ДОБАВЯМЕ ГО
            _context.Favorites.Add(new Favorite
            {
                UserId = userId,
                ProductId = productId
            });
        }

        await _context.SaveChangesAsync();

        // 3. Връщаме потребителя точно там, където е бил (на същата страница)
        return Redirect(Request.Headers["Referer"].ToString());
    }

    // Метод за показване на страницата "Моите Любими"
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var myFavorites = await _context.Favorites
            .Include(f => f.Product) // Зареждаме данните за продукта
            .Where(f => f.UserId == userId)
            .Select(f => f.Product) // Вземаме само продуктите
            .ToListAsync();

        return View(myFavorites);
    }
}