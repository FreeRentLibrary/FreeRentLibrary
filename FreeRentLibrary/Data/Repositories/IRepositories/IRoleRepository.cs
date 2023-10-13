using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace FreeRentLibrary.Data.Repositories.IRepositories
{
    public interface IRoleRepository: IGenericRepository<Microsoft.AspNetCore.Identity.IdentityRole>
    {
        IEnumerable<SelectListItem> GetComboRoles();
    }
}
