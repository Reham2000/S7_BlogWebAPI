using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IGenaricRepository<TableModel> where TableModel : class
    {
        //// Get All
        Task<IEnumerable<TableModel>> GetAllAsync();
        Task<IEnumerable<TableModel>> GetAllAsync(
                Expression<Func<TableModel,bool>> predicate = null,
                IEnumerable<Expression<Func<TableModel, object>>> includes = null
            );
        //// Get By Id
        Task<TableModel?> GetByIdAsync(int id);
        //// Find / search
        //// Create
        Task CreateAsync(TableModel model);
        //// Update
        void Update(TableModel model);
        //// Delete
        void Delete(TableModel model);
        //// Save
        //Task<int> SaveAsync();
    }
}
