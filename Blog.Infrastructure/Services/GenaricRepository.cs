using Blog.Core.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class GenaricRepository<TableModel> : IGenaricRepository<TableModel>
        where TableModel : class
    {
        private readonly AppDbContext _context; // MyDatabase
        private readonly DbSet<TableModel> _myTable; // My Table
        public GenaricRepository(AppDbContext context)
        {
            _context = context;
            _myTable = _context.Set<TableModel>();
        }

        

        public async Task<IEnumerable<TableModel>> GetAllAsync()
        {
            return await _myTable.ToListAsync();
        }

        public async Task<TableModel?> GetByIdAsync(int id)
        {
            return await _myTable.FindAsync(id);
        }

        

        public async Task CreateAsync(TableModel model)
        {
            await _myTable.AddAsync(model);
        }
        public void Update(TableModel model)
        {
            _myTable.Update(model);
        }

        public void Delete(TableModel model) =>  _myTable.Remove(model);
        
        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}
    }
}
