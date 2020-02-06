using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace CompaniesPoC.Controllers
{
    [Authorize]
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
            try
            {
                var results = await _companyManager.GetAll();
                if (results.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(results.Result);
                }

                return NotFound(results.Message);
            }
            catch (Exception ex)
            {
                // Log data
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult<string>> PostCompany([FromBody]CompanyDTO company)
        {
            try
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
            catch (Exception ex)
            {
                // Log data
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("update/{id}")]
        public async Task<ActionResult<string>> PutCompany([FromBody]CompanyDTO company, long id)
        {
            try
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
            catch (Exception ex)
            {
                // Log data
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost("search")]
        public async Task<ActionResult<IEnumerable<Company>>> SearchCompanies([FromBody]CompanySearch search)
        {
            try
            {
                var results = await _companyManager.Search(search);

                if (results.StatusCode == HttpStatusCode.OK)
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
            catch (Exception ex)
            {
                // Log data
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<string>> DeleteCompany(long id)
        {
            try
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
            catch (Exception ex)
            {
                // Log data
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
