using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Laba1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DBGroupsContext _context;

        public ChartsController (DBGroupsContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var groups = _context.Groups.Include(a => a.Artists).ToList();
            List<object> groupArtist = new List<object>();
            groupArtist.Add(new[] { "Group", "Amount of artists" });

            foreach (var g in groups)
            {
                groupArtist.Add(new object[] { g.GrName, g.Artists.Count() });
            }
            return new JsonResult(groupArtist);
        }

        [HttpGet("JsonData1")]
        public JsonResult JsonData1()
        {
            var groups = _context.Albums.Include(a => a.Songs).ToList();
            List<object> groupArtist = new List<object>();
            groupArtist.Add(new[] { "Album", "Amount of songs" });

            foreach (var g in groups)
            {
                groupArtist.Add(new object[] { g.AName, g.Songs.Count() });
            }
            return new JsonResult(groupArtist);
        }
    }
}