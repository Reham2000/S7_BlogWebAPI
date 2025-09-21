using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        // DI
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }
        
        

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();   
        }

        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
            //return await _context.Categories.FirstAsync(c => c.Id == id);
            //return await _context.Categories.LastAsync(c => c.Id == id);
            //return await _context.Categories.SingleAsync(c => c.Id == id);
            //return await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            //return await _context.Categories.LastOrDefaultAsync(c => c.Id == id);
            //return await _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }
        public async Task CreateAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            // get old category
            var OldCategory = await GetByIdAsync(category.Id);
            if(OldCategory is null)
                return false;
            // Set New Data
            OldCategory.Name = category.Name;
            // Update
            _context.Categories.Update(OldCategory);
            // Save
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(int id, Category category)
        {
            var OldCategory = await GetByIdAsync(id);
            if (OldCategory is null)
                return false;
            OldCategory.Name = category.Name;
            _context.Categories.Update(OldCategory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var Category = await GetByIdAsync(id);
            if (Category is null) return false;
            _context.Categories.Remove(Category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
