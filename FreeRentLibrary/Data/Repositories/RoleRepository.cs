using FreeRentLibrary.Data.Repositories.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRentLibrary.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DataContext _context;

        public RoleRepository(DataContext context) 
        {
            _context = context;
        }
        public Task CreateAsync(IdentityRole entity)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(IdentityRole entity)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ExistAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<IdentityRole> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityRole> GetByIdAsync(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetComboRoles()
        {
            var list = _context.Roles.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }).OrderBy(l => l.Text).ToList();
            list.Insert(0, new SelectListItem
            {
                Text = "Select a Role",
                Value = "0"
            });
            return list;
        }

        public Task UpdateAsync(IdentityRole entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
