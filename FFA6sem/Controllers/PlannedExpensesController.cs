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
    public class PlannedExpensesController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IPlannedExpensesService _plannedExpensesService;
        private readonly ILogger<ExpenseController> _logger;

        public PlannedExpensesController(IDbCrud dbCrud, IPlannedExpensesService plannedExpensesService, ILogger<ExpenseController> logger)
        {
            _dbCrud = dbCrud;
            _plannedExpensesService = plannedExpensesService;
            _logger = logger;
        }

        // GET: api/PlannedExpenses/budget/{budgetId}
        [HttpGet("budget/{budgetId}")]
        public async Task<ActionResult<IEnumerable<PlannedExpensesModel>>> GetBudgetPlannedExpenses(int budgetId)
        {
            var budgetPlannedExpenses = await _dbCrud.GetAllBudgetsPlannedExpenses(budgetId);
            _logger.LogInformation("GetAllPlannedExpenses of budget with id {0} called", budgetId);
            return budgetPlannedExpenses;
        }

        // GET: api/PlannedExpenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlannedExpensesModel>> GetPlannedExpenses(int id)
        {
            var plannedExpenses = await _dbCrud.GetBudgetsPlannedExpensesById(id);

            if (plannedExpenses == null)
            {
                _logger.LogWarning("PlannedExpenses with id {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("PlannedExpenses with id {0} found", id);
            return plannedExpenses;
        }

        // POST: api/PlannedExpenses
        [HttpPost]
        public async Task<ActionResult<PlannedExpensesModel>> PostPlannedExpenses([FromBody]PlannedExpensesModel plannedExpensesModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreatePlannedExpenses(plannedExpensesModel);
            _logger.LogInformation("PlannedExpenses created");
            return CreatedAtAction("GetPlannedExpenses", new { id = plannedExpensesModel.Id }, plannedExpensesModel);

        }

        // POST: api/PlannedExpenses/sum
        [HttpPost("sum")]
        public async Task<ActionResult<decimal?>> PostSumOfAllPlannedExpenses([FromBody] List<PlannedExpensesModel> plannedExpenses)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            var result = _plannedExpensesService.GetSumOfAllPlannedExpenses(plannedExpenses);
            _logger.LogInformation("Sum of PlannedExpenses is taken");
            return result;
        }

        // PUT: api/PlannedExpenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlannedExpenses(int id, [FromBody] PlannedExpensesModel plannedExpensesModel)
        {
            if(id != plannedExpensesModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdatePlannedExpenses(plannedExpensesModel);
            }
            catch(DbUpdateConcurrencyException)
            {
                if(!_plannedExpensesService.PlannedExpensesExists(id))
                {
                    _logger.LogWarning("PlannedExpenses with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("PlannedExpenses with id {0} updated", id);
            return NoContent();

        }

        // DELETE: api/PlannedExpenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var plannedExpenses = await _dbCrud.GetBudgetsPlannedExpensesById(id);
            if(plannedExpenses == null)
            {
                _logger.LogWarning("PlannedExpenses with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeletePlannedExpenses(id);
            _logger.LogInformation("PlannedExpenses with id {0} deleted", id);
            return NoContent();
        }
    }
}
