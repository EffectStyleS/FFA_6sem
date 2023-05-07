using DAL.Entities;
using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using FFA6sem.Model.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ExpenseController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IExpenseService _expenseService;
        private readonly ILogger<ExpenseController> _logger;

        public ExpenseController(IDbCrud dbCrud, IExpenseService expenseService, ILogger<ExpenseController> logger)
        {
            _dbCrud = dbCrud;
            _expenseService = expenseService;
            _logger = logger;
        }


        // GET: api/Expense/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<ExpenseModel>>> GetAllExpneses(string userId)
        {
            var expenses = await _dbCrud.GetAllExpenses(userId);
            _logger.LogInformation("GetAllExpenses of user with id {0} called", userId);
            return expenses;
        }

        // GET: api/Expense/{expenseId}
        [HttpGet("{expenseId}")]
        public async Task<ActionResult<ExpenseModel>> GetExpense(int expenseId)
        {
            var expense = await _dbCrud.GetExpenseById(expenseId);

            if (expense == null)
            {
                _logger.LogWarning("Expense with id {0} not found", expenseId);
                return NotFound();
            }

            _logger.LogInformation("Expense with id {0} found", expenseId);
            return expense;
        }

        // POST: api/Expense
        [HttpPost]
        public async Task<ActionResult<ExpenseModel>> PostExpense([FromBody]ExpenseModel expenseModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateExpense(expenseModel);
            _logger.LogInformation("Expense created");
            return CreatedAtAction("GetExpense", new { expenseId = expenseModel.Id }, expenseModel);
        }

        // PUT: api/Expense/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, [FromBody]ExpenseModel expenseModel)
        {
            if (id != expenseModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdateExpense(expenseModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_expenseService.ExpenseExists(id))
                {
                    _logger.LogWarning("Expense with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("Expense with id {0} updated", id);
            return NoContent();
        }

        // DELETE: api/Expense/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _dbCrud.GetExpenseById(id);

            if(expense == null)
            {
                _logger.LogWarning("Expense with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteExpense(id);
            _logger.LogInformation("Expense with id {0} deleted", id);
            return NoContent();
        }
    }
}
