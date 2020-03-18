using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAndApiDemo.Data;
using WebAndApiDemo.Models;

namespace WebAndApiDemo.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsCategoriesController : ControllerBase
    {
        private readonly WebAndApiDemoContext _context;

        public NewsCategoriesController(WebAndApiDemoContext context)
        {
            _context = context;
        }

        // GET: api/NewsCategories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsCategory>>> GetNewsCategory()
        {
            var categories = _context.NewsCategory
                .AsNoTracking();

            return await categories.ToListAsync();
        }

        // GET: api/NewsCategories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsCategory>> GetNewsCategory(int id)
        {
            var newsCategory = await _context.NewsCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.CategoryID == id);

            if (newsCategory == null)
            {
                return NotFound();
            }

            return newsCategory;
        }

        // PUT: api/NewsCategories/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNewsCategory(int id, NewsCategory newsCategory)
        {
            if (id != newsCategory.CategoryID)
            {
                return BadRequest();
            }

            _context.Entry(newsCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsCategoryExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NewsCategories
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<NewsCategory>> PostNewsCategory(NewsCategory newsCategory)
        {
            _context.NewsCategory.Add(newsCategory);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNewsCategory", new { id = newsCategory.CategoryID }, newsCategory);
        }

        // DELETE: api/NewsCategories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<NewsCategory>> DeleteNewsCategory(int id)
        {
            var categoryToDelete = await _context.NewsCategory
                .Include(n => n.NewsCategoryAssignments)
                    .ThenInclude(n => n.News)
                .FirstOrDefaultAsync(i => i.CategoryID == id);

            if (categoryToDelete == null)
            {
                return NotFound();
            }

            _context.NewsCategory.Remove(categoryToDelete);
            await _context.SaveChangesAsync();

            return categoryToDelete;
        }

        private bool NewsCategoryExists(int id)
        {
            return _context.NewsCategory.Any(e => e.CategoryID == id);
        }
    }
}
