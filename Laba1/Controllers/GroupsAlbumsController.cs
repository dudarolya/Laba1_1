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
    public class GroupsAlbumsController : Controller
    {
        private readonly DBGroupsContext _context;

        public GroupsAlbumsController(DBGroupsContext context)
        {
            _context = context;
        }

        // GET: GroupsAlbums
        public async Task<IActionResult> Index()
        {
            var dBGroupsContext = _context.GroupsAlbums.Include(g => g.Album).Include(g => g.Group);
            return View(await dBGroupsContext.ToListAsync());
        }

        // GET: GroupsAlbums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsAlbums = await _context.GroupsAlbums
                .Include(g => g.Album)
                .Include(g => g.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupsAlbums == null)
            {
                return NotFound();
            }

            return View(groupsAlbums);
        }

        // GET: GroupsAlbums/Create
        public IActionResult Create()
        {
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName");
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName");
            return View();
        }

        // POST: GroupsAlbums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AlbumId,GroupId,Info")] GroupsAlbums groupsAlbums)
        {
            if (ModelState.IsValid)
            {
                _context.Add(groupsAlbums);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", groupsAlbums.AlbumId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName", groupsAlbums.GroupId);
            return View(groupsAlbums);
        }

        // GET: GroupsAlbums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsAlbums = await _context.GroupsAlbums.FindAsync(id);
            if (groupsAlbums == null)
            {
                return NotFound();
            }
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", groupsAlbums.AlbumId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName", groupsAlbums.GroupId);
            return View(groupsAlbums);
        }

        // POST: GroupsAlbums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AlbumId,GroupId,Info")] GroupsAlbums groupsAlbums)
        {
            if (id != groupsAlbums.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groupsAlbums);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsAlbumsExists(groupsAlbums.Id))
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
            ViewData["AlbumId"] = new SelectList(_context.Albums, "AId", "AName", groupsAlbums.AlbumId);
            ViewData["GroupId"] = new SelectList(_context.Groups, "GrId", "GrName", groupsAlbums.GroupId);
            return View(groupsAlbums);
        }

        // GET: GroupsAlbums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groupsAlbums = await _context.GroupsAlbums
                .Include(g => g.Album)
                .Include(g => g.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groupsAlbums == null)
            {
                return NotFound();
            }

            return View(groupsAlbums);
        }

        // POST: GroupsAlbums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groupsAlbums = await _context.GroupsAlbums.FindAsync(id);
            _context.GroupsAlbums.Remove(groupsAlbums);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsAlbumsExists(int id)
        {
            return _context.GroupsAlbums.Any(e => e.Id == id);
        }
    }
}
