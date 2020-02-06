using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CompaniesPoC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyManager _companyManager;

        public CompaniesController(ICompanyManager companyManager)
        {
            _companyManager = companyManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            var results = await _companyManager.GetAll();

            if (results.StatusCode == HttpStatusCode.OK)
            {
                return Ok(results.Result);
            }

            return NotFound(results.Message);
        }

        [HttpPost("create")]
        public async Task<ActionResult<string>> PostCompany([FromBody]CompanyDTO company)
        {
            var results = await _companyManager.Add(company);
            if (!string.IsNullOrEmpty(results.Result))
            {
                return Ok(results.Result);
            }
            else if (results.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(results.Message);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, results.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<string>> PutCompany([FromBody]CompanyDTO company, long id)
        {
            var results = await _companyManager.Update(company, id);
            if (!string.IsNullOrEmpty(results.Result))
            {
                return Ok(results.Result);
            }
            else if (results.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(results.Message);
            }
            else if (results.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(results.Message);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, results.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> DeleteCompany(long id)
        {
            var results = await _companyManager.Delete(id);
            if (!string.IsNullOrEmpty(results.Result))
            {
                return Ok(results.Result);
            }
            else if (results.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(results.Message);
            }
            else if (results.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(results.Message);
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, results.Message);
            }
        }
    }
}
