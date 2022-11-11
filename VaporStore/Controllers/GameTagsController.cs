using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Data;
using Data.Models;

namespace VaporStore.Controllers
{
    public class GameTagsController : Controller
    {
        private readonly VaporStoreContext _context;

        public GameTagsController(VaporStoreContext context)
        {
            _context = context;
        }

        // GET: GameTags
        public async Task<IActionResult> Index()
        {
            var vaporStoreContext = _context.GameTags.Include(g => g.Game).Include(g => g.Tag);
            return View(await vaporStoreContext.ToListAsync());
        }

        // GET: GameTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTag = await _context.GameTags
                .Include(g => g.Game)
                .Include(g => g.Tag)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (gameTag == null)
            {
                return NotFound();
            }

            return View(gameTag);
        }

        // GET: GameTags/Create
        public IActionResult Create()
        {
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name");
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name");
            return View();
        }

        // POST: GameTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GameId,TagId")] GameTag gameTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gameTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", gameTag.GameId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", gameTag.TagId);
            return View(gameTag);
        }

        // GET: GameTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTag = await _context.GameTags.FindAsync(id);
            if (gameTag == null)
            {
                return NotFound();
            }
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", gameTag.GameId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", gameTag.TagId);
            return View(gameTag);
        }

        // POST: GameTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GameId,TagId")] GameTag gameTag)
        {
            if (id != gameTag.GameId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gameTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameTagExists(gameTag.GameId))
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
            ViewData["GameId"] = new SelectList(_context.Games, "Id", "Name", gameTag.GameId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Name", gameTag.TagId);
            return View(gameTag);
        }

        // GET: GameTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gameTag = await _context.GameTags
                .Include(g => g.Game)
                .Include(g => g.Tag)
                .FirstOrDefaultAsync(m => m.GameId == id);
            if (gameTag == null)
            {
                return NotFound();
            }

            return View(gameTag);
        }

        // POST: GameTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gameTag = await _context.GameTags.FindAsync(id);
            _context.GameTags.Remove(gameTag);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GameTagExists(int id)
        {
            return _context.GameTags.Any(e => e.GameId == id);
        }
    }
}
