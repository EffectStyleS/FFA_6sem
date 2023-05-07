using DAL.Entities;
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
    public class IncomeController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IIncomeService _incomeService;
        private readonly ILogger<IncomeController> _logger;

        public IncomeController(IDbCrud dbCrud, IIncomeService incomeService, ILogger<IncomeController> logger)
        {
            _dbCrud = dbCrud;
            _incomeService = incomeService;
            _logger = logger;
        }

        // GET: api/Income/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<IncomeModel>>> GetAllIncomes(string userId)
        {
            var incomes = await _dbCrud.GetAllIncomes(userId);
            _logger.LogInformation("GetAllIncomes of user with id {0} called", userId);
            return incomes;
        }

        // GET: api/Income/{incomeId}
        [HttpGet("{incomeId}")]
        public async Task<ActionResult<IncomeModel>> GetIncome(int incomeId)
        {
            var income = await _dbCrud.GetIncomeById(incomeId);

            if (income == null)
            {
                _logger.LogWarning("Income with id {0} not found", incomeId);
                return NotFound();
            }

            _logger.LogInformation("Income with id {0} found", incomeId);
            return income;
        }

        // POST: api/Income
        [HttpPost]
        public async Task<ActionResult<IncomeModel>> PostIncome([FromBody]IncomeModel incomeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateIncome(incomeModel);
            _logger.LogInformation("Income created");
            return CreatedAtAction("GetIncome", new { incomeId = incomeModel.Id }, incomeModel);
        }

        // PUT: api/Income/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncome(int id, [FromBody] IncomeModel incomeModel)
        {
            if (id != incomeModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdateIncome(incomeModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_incomeService.IncomeExists(id))
                {
                    _logger.LogWarning("Income with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("Income with id {0} updated", id);
            return NoContent();
        }


        // DELETE: api/Income/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var income = await _dbCrud.GetIncomeById(id);

            if (income == null)
            {
                _logger.LogWarning("Income with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteIncome(id);
            _logger.LogInformation("Income with id {0} deleted", id);
            return NoContent();
        }

    }  
}
