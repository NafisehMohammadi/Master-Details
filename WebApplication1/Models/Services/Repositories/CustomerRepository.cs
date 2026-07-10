using MasterDetails.Models.DomainModels;
using MasterDetails.Models.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MasterDetails.Models.Services.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {

        #region [- Private Fields() -]
        private readonly ProjectDbContext _projectDbContext;
        #endregion

        #region [- Ctor() -]
        public CustomerRepository(ProjectDbContext context)
        {
            _projectDbContext = context;
        }
        #endregion

        #region[- SelectCustomerAllAsync() -]
        public async Task<IEnumerable<Customer>> SelectCustomerAllAsync()
        {
            return await _projectDbContext.Customers .AsNoTracking()
                                                .OrderBy(x => x.FirstName)
                                                .ThenBy(x => x.LastName) .ToListAsync();
        }
         #endregion
        public async Task<Customer> SelectCustomerById(Guid id)
        {
            return await _projectDbContext.Customers

                     .AsNoTracking()

                     .FirstOrDefaultAsync(x => x.Id == id);
        }
       
    }
}
