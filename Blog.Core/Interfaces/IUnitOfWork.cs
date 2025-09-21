using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Keys
        IGenaricRepository<Category> Categories { get; }
        IGenaricRepository<Post> Posts { get; }
        IGenaricRepository<Comment> Comments { get; }
        ICategoryService CategoryService { get; }
        // Methods
        Task<int> SaveAsync();
    }
}
