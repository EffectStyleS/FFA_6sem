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
    public class IncomeTypeController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IIncomeTypeService _incomeTypeService;
        private readonly ILogger<IncomeTypeController> _logger;

        public IncomeTypeController(IDbCrud dbCrud, IIncomeTypeService incomeTypeService, ILogger<IncomeTypeController> logger)
        {
            _dbCrud = dbCrud;
            _incomeTypeService = incomeTypeService;
            _logger = logger;
        }

        // GET: api/IncomeType
        [HttpGet]
        public async Task<ActionResult<IEnumerable<IncomeTypeModel>>> GetAllIncomeTypes()
        {
            var incomeTypes = await _dbCrud.GetAllIncomeTypes();
            _logger.LogInformation("GetAllIncomeTypes called");
            return incomeTypes;
        }

        // GET: api/IncomeType/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IncomeTypeModel>> GetIncomeType(int id)
        {
            var incomeType = await _dbCrud.GetIncomeTypeById(id);

            if (incomeType == null)
            {
                _logger.LogWarning("IncomeType with id {0} not found", id);
                return NotFound();
            }

            _logger.LogInformation("IncomeType with id {0} found", id);
            return incomeType;
        }

        // POST: api/IncomeType
        [HttpPost]
        public async Task<ActionResult<IncomeTypeModel>> PostIncomeType([FromBody]IncomeTypeModel incomeTypeModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model isn't valid");
                return BadRequest(ModelState);
            }

            await _dbCrud.CreateIncomeType(incomeTypeModel);
            _logger.LogInformation("IncomeType created");
            return CreatedAtAction("GetIncomeType", new { id = incomeTypeModel.Id }, incomeTypeModel);
        }

        // PUT: api/<IncomeTypeController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncomeType(int id, [FromBody]IncomeTypeModel incomeTypeModel)
        {
            if (id != incomeTypeModel.Id)
            {
                _logger.LogWarning("Incorrect id");
                return BadRequest();            
            }

            try
            {
                await _dbCrud.UpdateIncomeType(incomeTypeModel);
            }
            catch(DbUpdateConcurrencyException)
            {
                if (!_incomeTypeService.IncomeTypeExists(id))
                {
                    _logger.LogWarning("IncomeType with id {0} not found", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            _logger.LogInformation("IncomeType with id {0} updated", id);
            return NoContent();
        }

        // DELETE: api/IncomeType/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var incomeType = await _dbCrud.GetIncomeTypeById(id);
            
            if(incomeType == null)
            {
                _logger.LogWarning("IncomeType with id {0} not found", id);
                return NotFound();
            }

            await _dbCrud.DeleteIncomeType(id);
            _logger.LogInformation("IncomeType with id {0} deleted", id);
            return NoContent();
        }
    }
}
