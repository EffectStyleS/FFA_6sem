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
    public class ExpenseTypeController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IExpenseTypeService _expenseTypeService;
        private readonly ILogger<ExpenseTypeController> _logger;
        
        public ExpenseTypeController(IDbCrud dbCrud, IExpenseTypeService expenseTypeService, ILogger<ExpenseTypeController> logger)
        {
            _dbCrud = dbCrud;
            _expenseTypeService = expenseTypeService;
            _logger = logger;
        }

        // GET: api/ExpenseType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpenseTypeModel>>> GetAllExpenseTypes()
        {
            var expenseTypes = await _dbCrud.GetAllExpenseTypes();
            _logger.LogInformation("GetAllExpenseTypes called");
            return expenseTypes;
        }

        // GET: api/ExpenseType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ExpenseTypeModel>> GetExpenseType(int id)
        {
            var expenseType = await _dbCrud.GetExpenseTypeById(id);

            if (expenseType == null)
            {
                _logger.LogWarning("ExpenseType with id {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("ExpenseType with id {0} found", id);
            return expenseType;
        }

        // POST: api/ExpenseType
        [HttpPost]
        public async Task<ActionResult<ExpenseTypeModel>> PostExpenseType([FromBody]ExpenseTypeModel expenseTypeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateExpenseType(expenseTypeModel);
            _logger.LogInformation("ExpenseType created");
            return CreatedAtAction("GetExpenseType", new { id = expenseTypeModel.Id }, expenseTypeModel);

        }

        // PUT: api/ExpenseType
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseType(int id, [FromBody]ExpenseTypeModel expenseTypeModel)
        {
            if (id != expenseTypeModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();
            }

            try
            {
                await _dbCrud.UpdateExpenseType(expenseTypeModel);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_expenseTypeService.ExpenseTypeExists(id))
                {
                    _logger.LogWarning("ExpenseType with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            _logger.LogInformation("ExpenseType with id {0} updated", id);
            return NoContent();
        }

        // DELETE api/ExpenseType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expenseType = await _dbCrud.GetExpenseTypeById(id);

            if (expenseType == null)
            {
                _logger.LogWarning("ExpenseType with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteExpenseType(id);
            _logger.LogInformation("ExpenseType with id {0} deleted", id);
            return NoContent();
        }
    }
}
