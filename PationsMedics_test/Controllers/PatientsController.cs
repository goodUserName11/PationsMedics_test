using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using PationsMedics_test.Data;
using PationsMedics_test.Models;
using PationsMedics_test.ViewModels;

namespace PationsMedics_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly PationsMedicsContext _context;

        public PatientsController(PationsMedicsContext context)
        {
            _context = context;
        }

        // GET: api/Patients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ListItemPatient>>> GetPatients(
            string? sortColumn,
            string? sortOrder,
            int page,
            int pageSize)
        {
            IQueryable<Patient> patients = _context.Patients
                .Include(p => p.Area);

            Expression<Func<Patient, object>> keySelector = sortColumn?.ToLower() switch
            {
                "firstname" => p => p.FirstName,
                "lastname" => p => p.LastName,
                "patronymic" => p => p.Patronymic,
                "address" => p => p.Address,
                "dob" => p => p.Dob,
                "gender" => p => p.Gender,
                "areanumber" => p => p.Area.Number,
                _ => p => p.Id,
            };

            if (sortOrder?.ToLower() == "desc")
            {
                patients = patients.OrderByDescending(keySelector);
            }
            else
            {
                patients = patients.OrderBy(keySelector);
            }

            var liPatients = patients
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p =>
                new ListItemPatient()
                {
                    Id = p.Id,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Patronymic = p.Patronymic,
                    Address = p.Address,
                    Dob = p.Dob,
                    Gender = p.Gender,
                    AreaNumber = p.Area.Number,
                }
            );

            return Ok(liPatients);
        }

        // GET: api/Patients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EditPatient>> GetPatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            EditPatient liPatient = new()
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Patronymic = patient.Patronymic,
                Address = patient.Address,
                Dob = patient.Dob,
                Gender = patient.Gender,
                AreaId = patient.AreaId,
            };

            return Ok(liPatient);
        }

        // PUT: api/Patients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPatient(int id, EditPatient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }

            var dbPatient = new Patient()
            {
                Id = patient.Id,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Patronymic = patient.Patronymic,
                Address = patient.Address,
                Dob = patient.Dob,
                Gender = patient.Gender,
                AreaId = patient.AreaId,
            };

            _context.Entry(dbPatient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        // POST: api/Patients
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(EditPatient patient)
        {
            _context.Patients.Add(new() 
            {
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                Patronymic = patient.Patronymic,
                Address = patient.Address,
                Dob = patient.Dob,
                Gender = patient.Gender,
                AreaId = patient.AreaId,
            });
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPatient", new { id = patient.Id }, patient);
        }

        // DELETE: api/Patients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.Id == id);
        }
    }
}
