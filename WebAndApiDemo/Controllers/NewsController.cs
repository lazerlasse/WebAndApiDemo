using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebAndApiDemo.Data;
using WebAndApiDemo.Models;
using WebAndApiDemo.Models.ViewModels;

namespace WebAndApiDemo.Controllers
{
    public class NewsController : Controller
    {
        private readonly WebAndApiDemoContext _context;

        public NewsController(WebAndApiDemoContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            var news = _context.News.AsNoTracking();

            return View(await news.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(category => category.NewsCategoryAssignments)
                    .ThenInclude(category => category.NewsCategory)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(n => n.ID == id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Titel,Content,Published")] News news)
        {
            if (ModelState.IsValid)
            {
                _context.Add(news);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(news);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .Include(c => c.NewsCategoryAssignments)
                    .ThenInclude(c => c.NewsCategory)
                .AsNoTracking().FirstOrDefaultAsync(c => c.ID == id);

            if (news == null)
            {
                return NotFound();
            }

            // Populate categories list to choose category from.
            PopulateAssignedNewsCategoryData(news);

            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, string[] selectedCategories)
        {
            // Return not found if id == null...
            if (id == null)
            {
                return NotFound();
            }

            // load news from id to update...
            var newsToUpdate = await _context.News
                .Include(c => c.NewsCategoryAssignments)
                    .ThenInclude(c => c.NewsCategory)
                .FirstOrDefaultAsync(c => c.ID == id);

            // Try update model and save changes...
            if (await TryUpdateModelAsync<News>(
                newsToUpdate,
                "",
                n => n.Titel, n => n.Content))
            {
                // Update selected categories.
                UpdateAssignedNewsCategories(selectedCategories, newsToUpdate);

                // Try saving changes to database..
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }

                // Update model and save changes succeded and return to index...
                return RedirectToAction(nameof(Index));
            }

            // Update and populate selected news categories...
            UpdateAssignedNewsCategories(selectedCategories, newsToUpdate);
            PopulateAssignedNewsCategoryData(newsToUpdate);

            // Return view...
            return View(newsToUpdate);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.ID == id);

            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Eager Load news to delete...
            var newsToDelete = await _context.News
                .Include(c => c.NewsCategoryAssignments)    // EF automatically delete category assignments from db.
                    .ThenInclude(c => c.NewsCategory)
                .SingleAsync(n => n.ID == id);

            // Delete and save change to database..
            _context.News.Remove(newsToDelete);
            await _context.SaveChangesAsync();

            // Redirect to index page..
            return RedirectToAction(nameof(Index));
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.ID == id);
        }

        // Method for populating assigned news categories...
        private void PopulateAssignedNewsCategoryData(News news)
        {
            var allNewsCategories = _context.NewsCategory;
            var newsCategories = new HashSet<int>(news.NewsCategoryAssignments.Select(c => c.NewsCategoryID));
            var viewModel = new List<ChosenNewsCategoryData>();
            foreach (var category in allNewsCategories)
            {
                viewModel.Add(new ChosenNewsCategoryData
                {
                    CategoryID = category.ID,
                    Titel = category.CategorName,
                    IsChecked = newsCategories.Contains(category.ID)
                });
            }
            ViewData["Categories"] = viewModel;
        }

        // This method updates the assigned categories for selected news..
        private void UpdateAssignedNewsCategories(string[] selectedCategories, News newsToUpdate)
        {
            if (selectedCategories == null)
            {
                newsToUpdate.NewsCategoryAssignments = new List<NewsCategoryAssignment>();
                return;
            }

            var selectedCategoriesHS = new HashSet<string>(selectedCategories);
            var newsCategories = new HashSet<int>
                (newsToUpdate.NewsCategoryAssignments.Select(c => c.NewsCategory.ID));
            foreach (var category in _context.NewsCategory)
            {
                if (selectedCategoriesHS.Contains(category.ID.ToString()))
                {
                    if (!newsCategories.Contains(category.ID))
                    {
                        newsToUpdate.NewsCategoryAssignments.Add(new NewsCategoryAssignment { NewsID = newsToUpdate.ID, NewsCategoryID = category.ID });
                    }
                }
                else
                {

                    if (newsCategories.Contains(category.ID))
                    {
                        NewsCategoryAssignment categoryToRemove = newsToUpdate.NewsCategoryAssignments.FirstOrDefault(c => c.NewsCategoryID == category.ID);
                        _context.Remove(categoryToRemove);
                    }
                }
            }
        }
    }
}
