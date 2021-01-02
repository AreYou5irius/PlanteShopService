using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassLibrary;
using PlanteShopRESTService;

namespace PlanteShopRESTService.Controllers
{
    //denne controller bliver oprette ved at vælge controllers -> add -> new scarffolded item -> API using entity framework
    //HUSK at publish til Azure via højreklik på projektet (PlanteShopService) og publish
    //Når du tester det i browser tilføjer du bare "/planterEF" på url'en

    [Route("planterEF")]
    [ApiController]
    public class PlanteEFController : ControllerBase
    {
        private readonly PlanteContext _context;

        public PlanteEFController(PlanteContext context)
        {
            _context = context;

            //har tilføjet dette, så at hvis listen er tom, bliver der tilføjet dummy data
            if (_context.Planter.Count() == 0)
            {
                _context.Planter.Add(new Plante("Test", "Test", 1, 1));
                _context.SaveChangesAsync();
            }
        }

        // GET: api/PlanteEF
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plante>>> GetPlanter()
        {
            return await _context.Planter.ToListAsync();
        }

        // GET: api/PlanteEF/5
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Plante>> GetPlante(int id)
        {
            var plante = await _context.Planter.FindAsync(id);

            if (plante == null)
            {
                return NotFound();
            }

            return plante;
        }

        //--------------------------------------------OBS bør være en liste, er ikke tjekket efter
        // GET: api/PlanteEF/PlanteType/rose
        [HttpGet]
        [Route("PlanteType/{type}")]
        public async Task<ActionResult<Plante>> GetPlante(string type)
        {
            var plante = await _context.Planter.FindAsync(type);

            if (plante == null)
            {
                return NotFound();
            }

            return plante;
        }

        // PUT: api/PlanteEF/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlante(int id, Plante plante)
        {
            if (id != plante.PlanteId)
            {
                return BadRequest();
            }

            _context.Entry(plante).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlanteExists(id))
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

        // POST: api/PlanteEF
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Plante>> PostPlante(Plante plante)
        {
            _context.Planter.Add(plante);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlante", new { id = plante.PlanteId }, plante);
        }

        // DELETE: api/PlanteEF/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Plante>> DeletePlante(int id)
        {
            var plante = await _context.Planter.FindAsync(id);
            if (plante == null)
            {
                return NotFound();
            }

            _context.Planter.Remove(plante);
            await _context.SaveChangesAsync();

            return plante;
        }

        private bool PlanteExists(int id)
        {
            return _context.Planter.Any(e => e.PlanteId == id);
        }
    }
}
