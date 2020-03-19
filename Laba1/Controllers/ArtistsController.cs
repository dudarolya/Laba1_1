using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Laba1.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly DBGroupsContext _context;

        public ArtistsController(DBGroupsContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
                return View(await _context.Artists.Include(ar => ar.Group).Include(ar => ar.Country).ToListAsync());
            //finding artists due to a group
            ViewBag.GroupId = id;
            ViewBag.GroupName = name;
            ViewData["Genders"] = new SelectList(_context.Artists, "AId", "Genders");
            var ArtistsByGroup = _context.Artists.Where(ar => ar.GroupId == id).Include(ar => ar.Group).Include(ar => ar.Country);
            return View(await ArtistsByGroup.ToListAsync());
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists
                .Include(a => a.Country)
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.AId == id);
            if (artists == null)
            {
                return NotFound();
            }

            return View(artists);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            ViewData["CountryId"] = new SelectList(_context.Countries, "CId", "CName");
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName");
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int groupId, [Bind("AId,AName,AGender,ABirth,APhone,GroupId,CountryId")] Artists artist)
        {
            artist.GroupId = groupId;
            if (ModelState.IsValid)
            {
                if (   _context.Artists.Where(a => a.AName == artist.AName).Count() != 0
                    && _context.Artists.Where(a => a.AGender == artist.AGender).Count() != 0
                    && _context.Artists.Where(a => a.ABirth == artist.ABirth).Count() != 0
                    && _context.Artists.Where(a => a.APhone == artist.APhone).Count() != 0
                    && _context.Artists.Where(a => a.GroupId == artist.GroupId).Count() != 0
                    && _context.Artists.Where(a => a.CountryId == artist.CountryId).Count() != 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Add(artist);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Artists", new { id = groupId, name = _context.Groups.Where(gr => gr.GrId == groupId).FirstOrDefault().GrName });
                }
            }

            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists.FindAsync(id);
            if (artists == null)
            {
                return NotFound();
            }
            ViewData["CountryId"] = new SelectList(_context.Countries, "CId", "CName", artists.CountryId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName", artists.GroupId);
            return View(artists);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AId,AName,AGender,ABirth,APhone,GroupId,CountryId")] Artists artists)
        {
            if (id != artists.AId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artists);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistsExists(artists.AId))
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
            ViewData["CountryId"] = new SelectList(_context.Countries, "CId", "CName", artists.CountryId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName", artists.GroupId);
            return View(artists);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artists = await _context.Artists
                .Include(a => a.Country)
                .Include(a => a.Group)
                .FirstOrDefaultAsync(m => m.AId == id);
            if (artists == null)
            {
                return NotFound();
            }

            return View(artists);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artists = await _context.Artists.FindAsync(id);
            _context.Artists.Remove(artists);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistsExists(int id)
        {
            return _context.Artists.Any(e => e.AId == id);
        }
    }
}
