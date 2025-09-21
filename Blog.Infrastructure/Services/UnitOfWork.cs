using Blog.Core.Interfaces;
using Blog.Core.Models;
using Blog.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        // DI For DB
        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            CategoryService = new CategoryService(context);
            Categories = new GenaricRepository<Category>(context);
            Posts = new GenaricRepository<Post>(context);
            Comments = new GenaricRepository<Comment>(context);
        }

        // Keys DI
        public ICategoryService CategoryService { get;private set; }
        public IGenaricRepository<Category> Categories { get; private set; }
        public IGenaricRepository<Post> Posts { get; private set; }
        public IGenaricRepository<Comment> Comments { get; private set; }
        // Methods
        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
