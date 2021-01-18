using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FileUploadAspCore5._0.Models;


namespace FileUploadAspCore5._0.Models
{
    public class EmployeeDbContext:DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options):base(options)
        {
            
        }
        public DbSet<Employee> Employee { get; set; }
        
    }
}
