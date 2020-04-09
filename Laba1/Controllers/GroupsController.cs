using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Laba1;
using System.IO;
using Microsoft.AspNetCore.Http;
using ClosedXML.Excel;

namespace Laba1.Controllers
{
    public class GroupsController : Controller
    {
        private readonly DBGroupsContext _context;

        public GroupsController(DBGroupsContext context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index()
        {
            return View(await _context.Groups.ToListAsync());
        }
        public IActionResult ErrorOccured(string errormsg)
        {
            ViewBag.Error = errormsg;
            return View();
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.GrId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Artists", new { id = groups.GrId, name = groups.GrName });
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GrId,GrName,GrCreation")] Groups group)
        {
            if (ModelState.IsValid)
            {
                if ((_context.Groups.Where(g => g.GrName == group.GrName).Count() != 0) 
                    && (_context.Groups.Where(g => g.GrCreation == group.GrCreation).Count() != 0))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _context.Add(group);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var group = await _context.Groups.FindAsync(id);
            if (group == null)
            {
                return NotFound();
            }
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("GrId,GrName,GrCreation")] Groups groups)
        {
            if (id != groups.GrId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(groups);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupsExists(groups.GrId))
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
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .FirstOrDefaultAsync(m => m.GrId == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var groups = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(int id)
        {
            return _context.Groups.Any(e => e.GrId == id);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                        await fileExcel.CopyToAsync(stream);
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                Groups newgroup;
                                var g = (from gr in _context.Groups
                                         where gr.GrName.Contains(worksheet.Name)
                                         select gr).ToList();
                                if (g.Count > 0)
                                {
                                    newgroup = g[0];
                                }
                                else
                                {
                                    newgroup = new Groups();
                                    newgroup.GrName = worksheet.Name;
                                    _context.Groups.Add(newgroup);
                                }

                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        if (row.Cell(8).Value.ToString() != "")
                                        {
                                            newgroup.GrCreation = (DateTime)row.Cell(8).Value;
                                        }

                                        Countries newcountry;
                                        var c = _context.Countries.Where(cntr => cntr.CName == row.Cell(5).Value.ToString()).ToList();
                                        if (c.Count > 0)
                                        {
                                            newcountry = c[0];
                                        }
                                        else
                                        {
                                            newcountry = new Countries();
                                            newcountry.CName = row.Cell(5).Value.ToString();
                                            newcountry.CLang = row.Cell(6).Value.ToString();
                                            newcountry.CCapital = row.Cell(7).Value.ToString();
                                            _context.Add(newcountry);
                                        }

                                        Artists newartist = new Artists();
                                        var art = _context.Artists.Where(ar => ar.AName == row.Cell(1).Value.ToString() 
                                        && ar.Country.CName == newcountry.CName
                                        && ar.ABirth == (DateTime)row.Cell(3).Value
                                        && ar.APhone == row.Cell(4).Value.ToString()).ToList();

                                        if (art.Count > 0)
                                        { 
                                            newartist = art[0];
                                        }
                                        else
                                        {
                                            newartist.AName = row.Cell(1).Value.ToString();
                                            newartist.ABirth = (DateTime)row.Cell(3).Value;
                                            newartist.APhone = row.Cell(4).Value.ToString();
                                            newartist.Group = newgroup;
                                            newartist.Country = newcountry;
                                            newartist.AGender = (Artists.Genders)((double)row.Cell(2).Value);
                                            _context.Add(newartist);
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        string exmsg = ex.Message;
                                        return RedirectToAction("ErrorOccured", "Groups", new { errormsg = exmsg});
                                    }
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook(XLEventTracking.Disabled))
            {
                var groups = _context.Groups.Include("Artists").ToList();
                foreach (var g in groups)
                {
                    var worksheet = workbook.Worksheets.Add(g.GrName);
                    worksheet.Cell("A1").Value = "Name";
                    worksheet.Cell("B1").Value = "Gender";
                    worksheet.Cell("C1").Value = "Birth";
                    worksheet.Cell("D1").Value = "Phone";
                    worksheet.Cell("E1").Value = "Country";
                    worksheet.Cell("F1").Value = "Language";
                    worksheet.Cell("G1").Value = "Capital";
                    worksheet.Cell("H1").Value = "Date of group creation";
                    worksheet.Cell(2, 8).Value = g.GrCreation;

                    worksheet.Row(1).Style.Font.Bold = true;
                    worksheet.Row(1).CellsUsed().Style.Fill.SetBackgroundColor(XLColor.EtonBlue);

                    var artists = g.Artists.ToList();

                    for (int i = 0; i < artists.Count; i++)
                    {
                        var country = _context.Countries.Where(c => c.CId == artists[i].CountryId).FirstOrDefault();
                        worksheet.Cell(i + 2, 1).Value = artists[i].AName;
                        worksheet.Cell(i + 2, 2).Value = artists[i].AGender;
                        worksheet.Cell(i + 2, 3).Value = artists[i].ABirth;
                        worksheet.Cell(i + 2, 4).Value = artists[i].APhone;
                        worksheet.Cell(i + 2, 5).Value = country.CName;
                        worksheet.Cell(i + 2, 6).Value = country.CLang;
                        worksheet.Cell(i + 2, 7).Value = country.CCapital;
                    }
                    worksheet.Columns().AdjustToContents(14.01, 25);
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                      "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                           {
                                FileDownloadName = $"groups{ DateTime.UtcNow.ToShortDateString()}.xlsx"
                           };
                }
            }
        }
    }
}
