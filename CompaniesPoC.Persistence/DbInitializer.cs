using CompaniesPoC.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompaniesPoC.Persistence
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // EnsureCreated will cause the database to be created
            // whenever it's needed to be. If it's already there
            // it won't do anything
            context.Database.EnsureCreated();

            // Check if specified table has any data in it
            // if not, then create some dummy data 
            if (context.Companies.Any() && context.Employees.Any())
            {
                return;
            }

            // create dummy data
            var employees = new Employee[]
            {
                new Employee() { Name = "Pawel", Surname = "Rudnicki", BirthDate = new DateTime(1991, 09, 23), JobTitle = JobTitle.Developer },
                new Employee() { Name = "John", Surname = "Don", BirthDate = new DateTime(1996, 02, 13), JobTitle = JobTitle.Architect },
                new Employee() { Name = "Amy", Surname = "Kovalsky", BirthDate = new DateTime(1988, 07, 17), JobTitle = JobTitle.Manager },
                new Employee() { Name = "Piotr", Surname = "Nowak", BirthDate = new DateTime(1989, 04, 29), JobTitle = JobTitle.Administrator }
            };

            foreach (var employee in employees)
            {
                context.Employees.Add(employee);
            }
            context.SaveChanges();

            var savedEmployees = context.Employees.ToList();
            var john = context.Employees.FirstOrDefault(x => x.Name == "John");
            var companies = new Company[]
            {
                new Company() { Name = "Pumox", EstablishmentYear = 2015, Employees = new List<Employee>(savedEmployees) },
                new Company() { Name = "Mobica", EstablishmentYear = 2002, Employees = new List<Employee>() { john } }
            };

            foreach (var company in companies)
            {
                context.Companies.Add(company);
            }
            context.SaveChanges();
        }
    }
}
