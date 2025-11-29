using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using global::LeaveLibrary.Models;
using Microsoft.EntityFrameworkCore;


    

    namespace LeaveLibrary.Repository
    {
        public class EmployeeRepository
        {
            private readonly DBContext _context;

            public EmployeeRepository(DBContext context)
            {
                _context = context;
            }

            public async Task<Employee?> GetById(int id)
            {
                return await _context.Employees
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            public async Task<Employee?> GetByEmail(string email)
            {
                return await _context.Employees
                    .FirstOrDefaultAsync(e => e.Email == email);
            }

            public async Task<List<Employee>> GetAll()
            {
                return await _context.Employees.ToListAsync();
            }

            public async Task Add(Employee employee)
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }

            public async Task Update(Employee employee)
            {
                _context.Employees.Update(employee);
                await _context.SaveChangesAsync();
            }

            public async Task Delete(Employee employee)
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }


