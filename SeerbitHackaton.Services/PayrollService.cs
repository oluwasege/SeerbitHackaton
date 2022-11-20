using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using SeerbitHackaton.Core.AspnetCore;
using SeerbitHackaton.Core.DataAccess.EfCore.UnitOfWork;
using SeerbitHackaton.Core.DataAccess.Repository;
using SeerbitHackaton.Core.Entities;
using SeerbitHackaton.Core.Timing;

namespace SeerbitHackaton.Services
{
    public class PayrollService:IPayrollService
    {
        private readonly IRepository<CompanyAdmin, long> _companyAdminRepository;
        private readonly IRepository<Company, long> _companyRepository;
        private readonly IRepository<Payroll, long> _payrollRepository;
        private readonly IRepository<Employee, long> _employeeRepository;
        ILogger<PayrollService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpUserService _currentUserService;
        private readonly IConfiguration _configuration;
        public PayrollService(IRepository<CompanyAdmin, long> companyAdminRepository,
                               IUnitOfWork unitOfWork,
                               IHttpUserService currentUserService,
                               ILogger<PayrollService> logger,
                               IRepository<Company, long> companyRepository,
                               IRepository<Payroll, long> payrollRepository,
                               IConfiguration configuration,
                               IRepository<Employee, long> employeeRepository)
        {
            _companyAdminRepository = companyAdminRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _companyRepository = companyRepository;
            _payrollRepository = payrollRepository;
            _configuration = configuration;
            _employeeRepository = employeeRepository;
        }

        public async Task<ResultModel<string>> BulkUpload(UploadEmployeesPayrollVM model)
        {
            var resultModel = new ResultModel<string>();
            try
            {
                var company = await _companyRepository.FirstOrDefaultAsync(model.CompanyId);
                if (company == null)
                {
                    resultModel.AddError("Company does not exist");
                    return resultModel;
                }

                if (model.File?.Length > 0)
                {
                    var stream = model.File.OpenReadStream();

                    _unitOfWork.BeginTransaction();


                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.First();
                        var rowCount = worksheet.Dimension.Rows;

                        //Gets the last row in the excel sheet
                        var validRow = GetLastRow(worksheet);

                        if (validRow == 2)
                        {
                            resultModel.AddError("Template file is empty, no record has been inputted");
                            return resultModel;
                        }

                        var istemplateValid = _configuration.GetSection("BulkUploadSettings:IsEmployeeParollValid").Value.ToLower();
                        var textInFile = worksheet.Cells[6, 1].Value.ToString().ToLower();
                        if (textInFile != istemplateValid || textInFile == null)
                        {
                            resultModel.AddError("Invalid file");
                            return resultModel;
                        }
                        var payrolls = new List<Payroll>();
                        for (var row = 3; row < validRow; row++)
                        {
                            var employeeNo = worksheet.Cells[row, 3].Value?.ToString().Trim();
                            var employeeId = await GetEmployeeId(employeeNo);
                            if (employeeId != 0)
                            {
                                var payroll = new Payroll
                                {
                                    AmountOfHours = Convert.ToDecimal(worksheet.Cells[row, 4].Value?.ToString()),
                                    AmountPerHour = Convert.ToDecimal(worksheet.Cells[row, 5].Value?.ToString()),
                                    CompanyId = model.CompanyId,
                                    EmployeeId = employeeId,
                                    CreationTime = Clock.Now,
                                    HasBeenPaid = false
                                };

                                payroll.AmountToBePaid = payroll.AmountOfHours * payroll.AmountPerHour;
                                _payrollRepository.Insert(payroll);
                            }
                        }

                        await _unitOfWork.SaveChangesAsync();
                        resultModel.Data = "Success";
                    }
                }
            }

            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _logger.LogError($"{ex.Message ?? ex.InnerException.Message}");
                resultModel.AddError(ex.Message);
                return resultModel;
            }
            return resultModel;
        }

        private int GetLastRow(ExcelWorksheet sheet)
        {
            if (sheet.Dimension == null) return 0;

            var row = sheet.Dimension.End.Row;
            while (row >= 1)
            {
                var range = sheet.Cells[row, 2, row, sheet.Dimension.End.Column];
                if (range.Any(c => !string.IsNullOrWhiteSpace(c.Text)))
                {
                    break;
                }
                row--;
            }
            return row;
        }

        private async Task<long> GetEmployeeId(string employeeNo)
        {
            var employee = await _employeeRepository.FirstOrDefaultAsync(x => x.EmployeeNO.ToLower().Trim() == employeeNo);
            if (employee == null)
            {
                return 0;
            }
            return employee.Id;
        }
    }
}
