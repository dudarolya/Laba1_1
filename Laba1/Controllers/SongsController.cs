using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba1;

namespace Laba1.Controllers
{
    public class SongsController : Controller
    {
        private readonly DBGroupsContext _context;
         
        public SongsController(DBGroupsContext context)
        {
            _context = context;
        }

        // GET: Songs
        public async Task<IActionResult> Index(int? id, string? name, string errormsg)
        {
            if (id == null)
                return View(await _context.Songs.Include(s => s.Album).Include(s => s.Genre).ToListAsync());
            ViewBag.AlbumId = id;
            ViewBag.AlbumName = name;
            if (_context.Albums.Any(s => s.AId == id) && _context.Albums.Any(s => s.AName == name))
            {
                var SongsByAlbum = _context.Songs.Where(s => s.AlbumId == id).Include(s => s.Album).Include(s => s.Genre);
                return View(await SongsByAlbum.ToListAsync());
            }
            else
            {
                var SongsByGenre = _context.Songs.Where(s => s.GenreId == id).Include(s => s.Album).Include(s => s.Genre);
                return View(await SongsByGenre.ToListAsync());
            }
        }

        // GET: Songs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (songs == null)
            {
                return NotFound();
            }

            return View(songs);
        }

        // GET: Songs/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName");
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenId", "GenName");
            return View();
        }

        // POST: Songs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SId,SName,SDuration,AlbumId,GenreId")] Songs song)
        {
            if (ModelState.IsValid)
            {
                if (_context.Songs.Where(s => s.SName == song.SName).Count() != 0
                    && _context.Songs.Where(s => s.SDuration == song.SDuration).Count() != 0
                    && _context.Songs.Where(s => s.AlbumId == song.AlbumId).Count() != 0
                    && _context.Songs.Where(s => s.GenreId == song.GenreId).Count() != 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Add(song);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", song.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenId", "GenName", song.GenreId);
            return View(song);
        }

        // GET: Songs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs.FindAsync(id);
            if (songs == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", songs.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenId", "GenName", songs.GenreId);
            return View(songs);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SId,SName,SDuration,AlbumId,GenreId")] Songs songs)
        {
            if (id != songs.SId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(songs);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SongsExists(songs.SId))
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
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", songs.AlbumId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "GenId", "GenName", songs.GenreId);
            return View(songs);
        }

        // GET: Songs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var songs = await _context.Songs
                .Include(s => s.Album)
                .Include(s => s.Genre)
                .FirstOrDefaultAsync(m => m.SId == id);
            if (songs == null)
            {
                return NotFound();
            }

            return View(songs);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var songs = await _context.Songs.FindAsync(id);
            _context.Songs.Remove(songs);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SongsExists(int id)
        {
            return _context.Songs.Any(e => e.SId == id);
        }
    }
}
