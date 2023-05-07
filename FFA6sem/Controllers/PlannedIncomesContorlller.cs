using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using FFA6sem.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class PlannedIncomesController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IPlannedIncomesService _plannedIncomesService;
        private readonly ILogger<PlannedIncomesController> _logger;

        public PlannedIncomesController(IDbCrud dbCrud, IPlannedIncomesService plannedIncomesService, ILogger<PlannedIncomesController> logger)
        {
            _dbCrud = dbCrud;
            _plannedIncomesService = plannedIncomesService;
            _logger = logger;
        }

        // GET: api/PlannedIncomes/budget/{budgetId}
        [HttpGet("budget/{budgetId}")]
        public async Task<ActionResult<IEnumerable<PlannedIncomesModel>>> GetBudgetPlannedIncomes(int budgetId)
        {
            var budgetPlannedIncomes = await _dbCrud.GetAllBudgetsPlannedIncomes(budgetId);
            _logger.LogInformation("GetAllPlannedIncomes of budget with id {0} called", budgetId);
            return budgetPlannedIncomes;
        }

        // GET api/PlannedIncomes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlannedIncomesModel>> GetPlannedIncomes(int id)
        {
            var plannedIncomes = await _dbCrud.GetBudgetsPlannedIncomesById(id);

            if (plannedIncomes == null)
            {
                _logger.LogWarning("PlannedIncomes with id {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("PlannedIncomes with id {0} found", id);
            return plannedIncomes;
        }

        // POST api/PlannedIncomes
        [HttpPost]
        public async Task<ActionResult<PlannedIncomesModel>> PostPlannedIncomes([FromBody]PlannedIncomesModel plannedIncomesModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
            }

            await _dbCrud.CreatePlannedIncomes(plannedIncomesModel);
            _logger.LogInformation("PlannedIncomes created");
            return CreatedAtAction("GetPlannedIncomes", new { id = plannedIncomesModel.Id }, plannedIncomesModel);
        }

        // POST: api/PlannedIncomes/sum
        [HttpPost("sum")]
        public async Task<ActionResult<decimal?>> PostSumOfAllPlannedIncomes([FromBody] List<PlannedIncomesModel> plannedIncomes)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            var result = _plannedIncomesService.GetSumOfAllPlannedIncomes(plannedIncomes);
            _logger.LogInformation("Sum of PlannedIncomes is taken");
            return result;
        }

        // PUT api/PlannedIncomes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlannedIncomes(int id, [FromBody]PlannedIncomesModel plannedIncomesModel)
        {
            if (id != plannedIncomesModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdatePlannedIncomes(plannedIncomesModel);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_plannedIncomesService.PlannedIncomesExists(id))
                {
                    _logger.LogWarning("PlannedIncomes with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("PlannedIncomes with id {0} updated", id);
            return NoContent();

        }

        // DELETE api/PlannedIncomes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var plannedIncomes = await _dbCrud.GetBudgetsPlannedIncomesById(id);

            if(plannedIncomes == null)
            {
                _logger.LogWarning("PlannedIncomes with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeletePlannedIncomes(id);
            _logger.LogInformation("PlannedIncomes with id {0} deleted", id);
            return NoContent();
        }
    }
}
