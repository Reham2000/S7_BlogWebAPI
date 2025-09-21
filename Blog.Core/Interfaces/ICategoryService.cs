using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface ICategoryService
    {
        // get all Categories
        Task<IEnumerable<Category>> GetAllAsync();
        // get by id
        Task<Category> GetByIdAsync(int id);
        //// create
        Task CreateAsync(Category category);
        //// Update
        Task<bool> UpdateAsync(Category category);
        Task<bool> UpdateAsync(int id, Category category);
        //// delete
        Task<bool> DeleteAsync(int id);
    }
}
