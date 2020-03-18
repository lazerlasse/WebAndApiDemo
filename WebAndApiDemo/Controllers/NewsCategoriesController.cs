using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAndApiDemo.Data;
using WebAndApiDemo.Models;

namespace WebAndApiDemo.Controllers
{
    public class NewsCategoriesController : Controller
    {
        private readonly WebAndApiDemoContext _context;

        public NewsCategoriesController(WebAndApiDemoContext context)
        {
            _context = context;
        }

        // GET: NewsCategories
        public async Task<IActionResult> Index()
        {
            var categories = _context.NewsCategory
                .AsNoTracking();

            return View(await categories.ToListAsync());
        }

        // GET: NewsCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsCategory = await _context.NewsCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.CategoryID == id);

            if (newsCategory == null)
            {
                return NotFound();
            }

            return View(newsCategory);
        }

        // GET: NewsCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: NewsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,CategoryName")] NewsCategory newsCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsCategory);
        }

        // GET: NewsCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsCategory = await _context.NewsCategory
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.CategoryID == id);

            if (newsCategory == null)
            {
                return NotFound();
            }
            return View(newsCategory);
        }

        // POST: NewsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,CategoryName")] NewsCategory newsCategory)
        {
            if (id != newsCategory.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsCategoryExists(newsCategory.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsCategory);
        }

        // GET: NewsCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsCategory = await _context.NewsCategory
                .FirstOrDefaultAsync(m => m.CategoryID == id);

            if (newsCategory == null)
            {
                return NotFound();
            }

            return View(newsCategory);
        }

        // POST: NewsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var newsCategory = await _context.NewsCategory
                .Include(c => c.NewsCategoryAssignments)
                .SingleAsync(i => i.CategoryID == id);

            _context.NewsCategory.Remove(newsCategory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool NewsCategoryExists(int id)
        {
            return _context.NewsCategory.Any(e => e.CategoryID == id);
        }
    }
}
