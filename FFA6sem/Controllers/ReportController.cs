using FFA6sem.Model.Interfaces;
using FFA6sem.Model.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FFA6sem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReportController : ControllerBase
    {
        private readonly IDbCrud _dbCrud;
        private readonly IReportService _reportService;
        private readonly ILogger<ReportController> _logger;

        public ReportController(IDbCrud dbCrud, IReportService reportService, ILogger<ReportController> logger)
        {
            _dbCrud = dbCrud;
            _reportService = reportService;
            _logger = logger;
        }

        // POST: api/Report/difference
        [HttpPost("difference")]
        public async Task<ActionResult<List<List<decimal?>>>> PostDifferenceOfExpensesSum([FromBody]UserModel userModel)
        {
            var result = await _reportService.TakeDifferenceOfExpensesSum(userModel);

            if (result == null)
            {
                _logger.LogWarning("Result null");
                return NotFound();
            }

            _logger.LogInformation("DifferenceOfExpensesSum is taken");
            return result;
        }
    }
}
