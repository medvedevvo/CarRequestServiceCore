using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CarRequestService.Models;
using CarRequestService.Services;

namespace CarRequestService.Controllers
{
    [Produces("application/json")]
    [Route("api/CarServices")]
    public class CarServicesController : Controller
    {
        private readonly CarServicesDbContext _context;

        private RealCarService realCarService;

        public CarServicesController(CarServicesDbContext context)//, RealCarService realCarService)
        {
            _context = context;
            //this.realCarService = realCarService;
        }

        // GET: api/CarServices
        [HttpGet]
        public IEnumerable<CarService> GetCarServices()
        {
            return _context.CarServices;
        }

        // GET: api/CarServices/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarService([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carService = await _context.CarServices.SingleOrDefaultAsync(m => m.Id == id);

            if (carService == null)
            {
                return NotFound();
            }

            return Ok(carService);
        }

        // PUT: api/CarServices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarService([FromRoute] int id, [FromBody] CarService carService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carService.Id)
            {
                return BadRequest();
            }

            _context.Entry(carService).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarServiceExists(id))
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

        // POST: api/CarServices
        [HttpPost]
        public async Task<IActionResult> PostCarService([FromBody] CarService carService)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CarServices.Add(carService);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarService", new { id = carService.Id }, carService);
        }

        // DELETE: api/CarServices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarService([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carService = await _context.CarServices.SingleOrDefaultAsync(m => m.Id == id);
            if (carService == null)
            {
                return NotFound();
            }

            _context.CarServices.Remove(carService);
            await _context.SaveChangesAsync();

            return Ok(carService);
        }

        private bool CarServiceExists(int id)
        {
            return _context.CarServices.Any(e => e.Id == id);
        }

        [HttpGet("cars/{IdCar}/accus")]
        public async Task<IActionResult> GetUserCars([FromRoute] int IdCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string link = "";
            await _context.CarServices.ForEachAsync(cs =>
            {
                if (cs.IdCar == IdCar)
                {
                    link = cs.Link;
                }
            });

            if (link == "")
            {
                return NotFound();
            }

            realCarService = new RealCarService(link);
            AccuListWithTime response = await realCarService.GetAccuState();

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet("cars/{IdCar}/accus/{IdAccu}")]
        public async Task<IActionResult> GetUserCars([FromRoute] int IdCar, [FromRoute] int IdAccu)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string link = "";
            await _context.CarServices.ForEachAsync(cs =>
            {
                if (cs.IdCar == IdCar)
                {
                    link = cs.Link;
                }
            });

            if (link == "")
            {
                return NotFound();
            }

            realCarService = new RealCarService(link);
            AccuListWithTime response = await realCarService.GetAccuState(IdAccu);

            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPut("cars/{IdCar}/init")]
        public async Task<IActionResult> PutAccuInit([FromRoute] int IdCar)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string link = "";
            await _context.CarServices.ForEachAsync(cs =>
            {
                if (cs.IdCar == IdCar)
                {
                    link = cs.Link;
                }
            });

            if (link == "")
            {
                return NotFound();
            }

            realCarService = new RealCarService(link);
            int response = await realCarService.PutAccuInit(IdCar);

            if (response != 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}