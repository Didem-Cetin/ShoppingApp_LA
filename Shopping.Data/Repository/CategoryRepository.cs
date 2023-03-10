using Shopping.Data.Repository.IRepository;
using Shopping.Model;
using ShoppingApp_LA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopping.Data.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            _context = context;
        }
    }
}
