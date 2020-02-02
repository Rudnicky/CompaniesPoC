using AutoMapper;
using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using CompaniesPoC.Core.Utils;
using CompaniesPoC.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CompaniesPoC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            try
            {
                var companies = await _companyRepository.GetAll();
                if (companies.NotNullOrEmpty())
                {
                    var mappedCompanies = _mapper.Map<List<CompanyDTO>>(companies);
                    if (mappedCompanies != null)
                    {
                        return Ok(mappedCompanies);
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
            return NotFound();
        }

        [HttpPost("create")]
        public async Task<ActionResult<CompanyDTO>> PostCompany([FromBody]CompanyDTO company)
        {
            try
            {
                // to wszystko ma pojsc do DaO managera
                if (company != null)
                {
                    var outputMessage = string.Empty;

                    if (string.IsNullOrEmpty(company.Name))
                    {
                        return BadRequest("Name cannot but null or empty!");
                    }

                    if (company.EstablishmentYear <= -1)
                    {
                        return BadRequest("Establishment year lower then zero? can't be.");
                    }

                    if (company.Id != 0)
                    {
                        outputMessage = "Passed ID will be ignored since ORM is creating ID automatically. ";
                    }

                    var isDuplicated = await _companyRepository.FindByName(company.Name);
                    if (isDuplicated != null)
                    {
                        return BadRequest("Company with that name already exists");
                    }

                    var mappedCompany = _mapper.Map<Company>(company);
                    if (mappedCompany != null)
                    {
                        var result = await _companyRepository.Add(mappedCompany);
                        if (result != -1)
                        {
                            return Ok(outputMessage + result);
                        }
                    }
                }
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }

            return BadRequest();
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Company>> GetCompany(long id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // PUT: api/Companies/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(long id, Company company)
        {
            if (id != company.Id)
            {
                return BadRequest();
            }

            _context.Entry(company).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Company>> DeleteCompany(long id)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return company;
        }

        private bool CompanyExists(long id)
        {
            return _context.Companies.Any(e => e.Id == id);
        }
    }
}
