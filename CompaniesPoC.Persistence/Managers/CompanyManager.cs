using AutoMapper;
using CompaniesPoC.Core.Interfaces;
using CompaniesPoC.Core.Models;
using CompaniesPoC.Core.Models.DTO;
using CompaniesPoC.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CompaniesPoC.Persistence.Managers
{
    public class CompanyManager : ICompanyManager
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompanyManager(ICompanyRepository companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<CustomResults<IEnumerable<CompanyDTO>>> GetAll()
        {
            var results = new CustomResults<IEnumerable<CompanyDTO>>();

            try
            {
                var companies = await _companyRepository.GetAll();
                if (companies.NotNullOrEmpty())
                {
                    var mappedCompanies = _mapper.Map<List<CompanyDTO>>(companies);
                    if (mappedCompanies != null)
                    {
                        results.StatusCode = HttpStatusCode.OK;
                        results.Message = Consts.SUCCESS_MESSAGE;
                        results.Result = mappedCompanies;
                    }
                }
                else
                {
                    results.StatusCode = HttpStatusCode.InternalServerError;
                    results.Message = Consts.DATABASE_FETCH_ERROR;
                }
            }
            catch (Exception ex)
            {
                results.StatusCode = HttpStatusCode.InternalServerError;
                results.Message = ex.Message;
            }

            return results;
        }

        public async Task<CustomResults<string>> Add(CompanyDTO company)
        {
            var results = new CustomResults<string>();

            try
            {
                if (company != null)
                {
                    if (string.IsNullOrEmpty(company.Name))
                    {
                        results.StatusCode = HttpStatusCode.BadRequest;
                        results.Message = "Name cannot but null or empty!";
                        return results;
                    }

                    if (company.EstablishmentYear <= -1 || company.EstablishmentYear > DateTime.Now.Year)
                    {
                        results.StatusCode = HttpStatusCode.BadRequest;
                        results.Message = "Wrong Establishment year";
                        return results;
                    }

                    if (company.Id != 0)
                    {
                        results.StatusCode = HttpStatusCode.BadRequest;
                        results.Message = "Passed ID will be ignored since ORM is creating ID automatically.";
                        return results;
                    }

                    var isDuplicated = await _companyRepository.FindByName(company.Name);
                    if (isDuplicated != null)
                    {
                        results.StatusCode = HttpStatusCode.BadRequest;
                        results.Message = "Company with that name already exists.";
                        return results;
                    }

                    var mappedCompany = _mapper.Map<Company>(company);
                    if (mappedCompany != null)
                    {
                        var result = await _companyRepository.Add(mappedCompany);
                        if (result != -1)
                        {
                            results.StatusCode = HttpStatusCode.BadRequest;
                            results.Result = $"Company added successfully. Generated ID's {result}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                results.StatusCode = HttpStatusCode.InternalServerError;
                results.Message = ex.Message;
            }

            return results;
        }

        public async Task<CustomResults<string>> Update(CompanyDTO company, long id)
        {
            var results = new CustomResults<string>();

            try
            {
                if (id < 0)
                {
                    results.StatusCode = HttpStatusCode.BadRequest;
                    results.Message = $"There's no way that the ID is less than {id}";
                    return results;
                }

                var updateCompany = await _companyRepository.Get(id);
                if (updateCompany == null)
                {
                    results.StatusCode = HttpStatusCode.NotFound;
                    results.Message = $"Company with id: {id} not found";
                    return results;
                }

                if (string.IsNullOrEmpty(company.Name))
                {
                    results.StatusCode = HttpStatusCode.BadRequest;
                    results.Message = "Name cannot but null or empty!";
                    return results;
                }

                if (company.EstablishmentYear <= -1 || company.EstablishmentYear > DateTime.Now.Year)
                {
                    results.StatusCode = HttpStatusCode.BadRequest;
                    results.Message = "Wrong Establishment year";
                    return results;
                }

                updateCompany.Name = company.Name;
                updateCompany.EstablishmentYear = company.EstablishmentYear;
                
                if (company.Employees != null && company.Employees.Count > 0)
                {
                    var mappedEmployees = _mapper.Map<List<Employee>>(company.Employees);
                    if (mappedEmployees != null)
                    {
                        updateCompany.Employees = mappedEmployees;
                    }
                }

                await _companyRepository.Update(updateCompany);

                var updatedCompany = await _companyRepository.Get(id);
                if (updatedCompany != null && updatedCompany.Name == company.Name)
                {
                    results.StatusCode = HttpStatusCode.OK;
                    results.Result = "Company updated successfully";
                }
            }
            catch (Exception ex)
            {
                results.StatusCode = HttpStatusCode.InternalServerError;
                results.Message = ex.Message;
            }

            return results;
        }

        public async Task<CustomResults<IEnumerable<CompanyDTO>>> Search(CompanySearch search)
        {
            var results = new CustomResults<IEnumerable<CompanyDTO>>();

            try
            {
                var companies = await _companyRepository.GetAll();
                if (companies.NotNullOrEmpty())
                {
                    var filteredCompanies = new List<Company>();

                    if (!string.IsNullOrEmpty(search.Keyword))
                    {
                        filteredCompanies = companies.Where(x =>
                            x.Employees != null && x.Employees.Any(e => e.Name == search.Keyword) ||
                            x.Employees != null && x.Employees.Any(e => e.Surname == search.Keyword) ||
                            x.Name == search.Keyword).ToList();
                    }

                    if (search.EmployeeDateOfBirthFrom != null)
                    {
                        filteredCompanies = companies.Where(x => x.Employees != null && x.Employees.Any(e => e.BirthDate > search.EmployeeDateOfBirthFrom)).ToList();
                    }

                    if (search.EmployeeDateOfBirthTo != null)
                    {
                        filteredCompanies = companies.Where(x => x.Employees != null && x.Employees.Any(e => e.BirthDate < search.EmployeeDateOfBirthTo)).ToList();
                    }

                    if (!string.IsNullOrEmpty(search.EmployeeJobTitle))
                    {
                        if (Enum.TryParse(search.EmployeeJobTitle, out JobTitle jobTitle))
                        {
                            filteredCompanies = companies.Where(x => x.Employees != null && x.Employees.Any(e => e.JobTitle == jobTitle)).ToList();
                        }
                        else
                        {
                            results.StatusCode = HttpStatusCode.BadRequest;
                            results.Message = $"There's no such a job title like: {search.EmployeeJobTitle}";
                            return results;
                        }
                    }

                    filteredCompanies = filteredCompanies.Distinct().ToList();
                    if (filteredCompanies.Count == 0)
                    {
                        results.StatusCode = HttpStatusCode.OK;
                        results.Message = $"No companies are found under such criteria.";
                        return results;
                    }

                    var mappedCompanies = _mapper.Map<List<CompanyDTO>>(filteredCompanies);
                    if (mappedCompanies != null)
                    {
                        results.StatusCode = HttpStatusCode.OK;
                        results.Message = $"Found {filteredCompanies.Count} objects";
                        results.Result = mappedCompanies;
                    }
                }
                else
                {
                    results.StatusCode = HttpStatusCode.InternalServerError;
                    results.Message = Consts.DATABASE_FETCH_ERROR;
                }
            }
            catch (Exception ex)
            {
                results.StatusCode = HttpStatusCode.InternalServerError;
                results.Message = ex.Message;
            }

            return results;
        }

        public async Task<CustomResults<string>> Delete(long id)
        {
            var results = new CustomResults<string>();

            try
            {
                if (id < 0)
                {
                    results.StatusCode = HttpStatusCode.BadRequest;
                    results.Message = $"There's no way that the ID is less than {id}";
                    return results;
                }

                var company = await _companyRepository.Get(id);
                if (company == null)
                {
                    results.StatusCode = HttpStatusCode.NotFound;
                    results.Message = $"Company with id: {id} not found";
                    return results;
                }

                await _companyRepository.Delete(company);

                var deletedCompany = await _companyRepository.Get(id);
                if (deletedCompany == null)
                {
                    results.StatusCode = HttpStatusCode.OK;
                    results.Result = $"Company with id: {id} deleted successfully.";
                    return results;
                }
            }
            catch (Exception ex)
            {
                results.StatusCode = HttpStatusCode.InternalServerError;
                results.Message = ex.Message;
            }

            return results;
        }
    }
}
