using Microsoft.Extensions.Logging;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.DataAccess.Repository;
using SeerbitHackaton.Core.Entities;

namespace SeerbitHackaton.Services
{
    public class EmployeeService
    {
        private readonly IRepository<Employee, long> _employeeRepository;
        ILogger<EmployeeService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public EmployeeService(IRepository<Employee, long> employeeRepository, IUnitOfWork unitOfWork, ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

      
    }
}
