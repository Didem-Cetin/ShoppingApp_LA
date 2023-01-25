using Shopping.Data.Repository.IRepository;
using ShoppingApp_LA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICategoryRepository Category => new CategoryRepository(_context);

        public IProductRepository Product => new ProductRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Save()
        {
          _context.SaveChanges();
        }
    }
}
