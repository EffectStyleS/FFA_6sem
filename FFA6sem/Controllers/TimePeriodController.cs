using DAL.Entities;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TimePeriodController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly ITimePeriodService _timePeriodService;
        private readonly ILogger<TimePeriodController> _logger;

        public TimePeriodController(IDbCrud dbCrud, ITimePeriodService timePeriodService, ILogger<TimePeriodController> logger)
        {
            _dbCrud = dbCrud;
            _timePeriodService = timePeriodService;
            _logger = logger;
        }

        // GET: api/TimePeriod
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimePeriodModel>>> GetAllTimePeriods()
        {
            var timePeriods = await _dbCrud.GetAllTimePeriods();
            _logger.LogInformation("GetAllTimePeriods called");
            return timePeriods;
        }

        // GET api/TimePeriod/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimePeriodModel>> GetTimePeriod(int id)
        {
            var timePeriod = await _dbCrud.GetTimePeriodsById(id);

            if (timePeriod == null)
            {
                _logger.LogWarning("TimePeriod with id {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("TimePeriod with id {0} found", id);
            return timePeriod;
        }

        // POST api/TimePeriod
        [HttpPost]
        public async Task<ActionResult<TimePeriodModel>> PostTimePeriod([FromBody]TimePeriodModel timePeriodModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateTimePeriod(timePeriodModel);
            _logger.LogInformation("TimePeriod created");
            return CreatedAtAction("GetTimePeriod", new { id = timePeriodModel.Id }, timePeriodModel);

        }

        // PUT api/TimePeriod/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimePeriod(int id, [FromBody] TimePeriodModel timePeriodModel)
        {
            if (id != timePeriodModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdateTimePeriod(timePeriodModel);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_timePeriodService.TimePeriodExists(id))
                {
                    _logger.LogWarning("TimePeriod with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("TimePeriod with id {0} updated", id);
            return NoContent();
        }

        // DELETE api/TimePeriod/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var timePeriod = await _dbCrud.GetTimePeriodsById(id);

            if (timePeriod == null)
            {
                _logger.LogWarning("TimePeriod with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteTimePeriod(id);
            _logger.LogInformation("TimePeriod with id {0} deleted", id);
            return NoContent();
        }
    }
}
