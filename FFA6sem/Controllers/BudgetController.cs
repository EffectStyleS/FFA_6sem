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
    public class BudgetController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IBudgetService _budgetService;
        private readonly ILogger<BudgetController> _logger;

        public BudgetController(IDbCrud dbCrud, IBudgetService budgetService, ILogger<BudgetController> logger)
        {
            _dbCrud = dbCrud;
            _budgetService = budgetService;
            _logger = logger;
        }

        // GET: api/Budget
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BudgetModel>>> GetAllBudgets()
        {
            var budgets = await _dbCrud.GetAllBudgets();
            _logger.LogInformation("GetAllBudgets called");
            return budgets;
        }

        // GET: api/Budget/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<BudgetModel>>> GetUserBudgets(string userId)
        {
            var userBudgets = await _dbCrud.GetAllUserBudgets(userId);

            if(userBudgets == null)
            {
                _logger.LogWarning("{0} User's budgets not found", userId);
                return NotFound();
            }
            _logger.LogInformation("GetUserBudgets called");
            return userBudgets;
        }

        // GET: api/Budget/{budgetId}
        [HttpGet("{budgetId}")]
        public async Task<ActionResult<BudgetModel>> GetBudget(int budgetId)
        {
            var budget = await _dbCrud.GetUserBudgetById(budgetId);

            if (budget == null)
            {
                _logger.LogWarning("Budget with id {0} not found", budgetId);
                return NotFound();
            }

            _logger.LogInformation("Budget with id {0} found", budgetId);
            return budget;
        }

        //// POST: api/Budget/SavePdf/{filePath}
        //[HttpPost("{filePath}")]
        //public async Task<IActionResult> PostSavePdf([FromBody]BudgetModel budget, string filePath)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _budgetService.SavePDF(budget, filePath);
        //    return Ok();
        //}
            
        // POST: api/Budget
        [HttpPost]
        public async Task<ActionResult<BudgetModel>> PostBudget([FromBody]BudgetModel budgetModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateBudget(budgetModel);
            _logger.LogInformation("Budget created");
            return CreatedAtAction("GetBudget", new { budgetId = budgetModel.Id }, budgetModel);
        }

        // PUT: api/Budget/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBudget(int id, [FromBody]BudgetModel budgetModel)
        {
            if (id != budgetModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdateBudget(budgetModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_budgetService.BudgetExists(id))
                {
                    _logger.LogWarning("Budget with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("Budget with id {0} updated", id);
            return NoContent();
        }

        // DELETE: api/Budget/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBudget(int id)
        {
            var budget = await _dbCrud.GetUserBudgetById(id);
            if (budget == null)
            {
                _logger.LogWarning("Budget with {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteBudget(id);
            _logger.LogInformation("Budget with id {0} deleted", id);
            return NoContent();         
        }
    }
}
