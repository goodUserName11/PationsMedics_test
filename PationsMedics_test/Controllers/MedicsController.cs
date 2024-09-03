using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PationsMedics_test.Data;
using PationsMedics_test.Models;
using PationsMedics_test.ViewModels;

namespace PationsMedics_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicsController : ControllerBase
    {
        private readonly PationsMedicsContext _context;

        public MedicsController(PationsMedicsContext context)
        {
            _context = context;
        }

        // GET: api/Medics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemMedic>>> GetMedics(
            string? sortColumn, 
            string? sortOrder,
            int page,
            int pageSize)
        {
            IQueryable<Medic> medics = _context.Medics
                .Include(m => m.Area)
                .Include(m => m.Cabinet)
                .Include(m => m.Specialization);

            Expression<Func<Medic, object>> keySelector = sortColumn?.ToLower() switch
            {
                "fullname" => m => m.FullName,
                "areanumber" => m => m.Area.Number,
                "specializationname" => m => m.Specialization.Name,
                "cabinetnumber" => m => m.Cabinet.Number,
                _ => m => m.Id,
            };

            if (sortOrder?.ToLower() == "desc")
            {
                medics = medics.OrderByDescending(keySelector);
            }
            else
            {
                medics = medics.OrderBy(keySelector);
            }

            var liMedics = medics
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(m => 
                new ListItemMedic()
                {
                    Id = m.Id,
                    FullName = m.FullName,
                    AreaNumber = m.AreaId != null ? m.Area.Number : null,
                    CabinetNumber = m.Cabinet.Number,
                    SpecializationName = m.Specialization.Name
                }
            );

            return Ok(liMedics);
        }

        // GET: api/Medics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EditMedic>> GetMedic(int id)
        {
            var medic = await _context.Medics.FindAsync(id);

            if (medic == null)
            {
                return NotFound();
            }

            EditMedic liMedic = new()
            {
                Id = medic.Id,
                FullName = medic.FullName,
                AreaId = medic.AreaId,
                CabinetId = medic.CabinetId,
                SpecializationId = medic.SpecializationId
            };

            return Ok(liMedic);
        }

        // PUT: api/Medics/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedic(int id, EditMedic medic)
        {
            if (id != medic.Id)
            {
                return BadRequest();
            }

            var dbMedic = new Medic()
            {
                Id = medic.Id,
                FullName = medic.FullName,
                CabinetId = medic.CabinetId,
                SpecializationId = medic.SpecializationId,
                AreaId = medic.AreaId,
            };

            _context.Entry(dbMedic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicExists(id))
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

        // POST: api/Medics
        [HttpPost]
        public async Task<ActionResult<Medic>> PostMedic(EditMedic medic)
        {
            _context.Medics.Add(new Medic()
            {
                FullName = medic.FullName,
                CabinetId = medic.CabinetId,
                SpecializationId = medic.SpecializationId,
                AreaId = medic.AreaId,
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedic", new { id = medic.Id }, medic);
        }

        // DELETE: api/Medics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedic(int id)
        {
            var medic = await _context.Medics.FindAsync(id);
            if (medic == null)
            {
                return NotFound();
            }

            _context.Medics.Remove(medic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicExists(int id)
        {
            return _context.Medics.Any(e => e.Id == id);
        }
    }
}
