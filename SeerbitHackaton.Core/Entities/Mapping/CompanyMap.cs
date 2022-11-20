using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities.Mapping
{
    public class CompanyMap: IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.ToTable(nameof(Company));
            SetupCompany(builder);
        }

        private void SetupCompany(EntityTypeBuilder<Company> builder)
        {
            var company = new Company()
            {
                Id = 1,
                CAC = "RTYIUEP",
                Name = "Seerbit",
                 Location="Lagos"

            };
            var companys=new List<Company>() { company };
            builder.HasData(company);
        }

    }

    public class CompanyAdminMap: IEntityTypeConfiguration<CompanyAdmin>
    {
        public void Configure(EntityTypeBuilder<CompanyAdmin> builder)
        {
            builder.ToTable(nameof(CompanyAdmin));
            SetupCompanyAdmin(builder);
        }

        private void SetupCompanyAdmin(EntityTypeBuilder<CompanyAdmin> builder)
        {
            var companyAdmin = new CompanyAdmin()
            {
                Id = 1,
                UserId = 2,
                CompanyId = 1
            };

            var CompanyAdmins = new List<CompanyAdmin>() { companyAdmin };
            builder.HasData(CompanyAdmins);
        }
    }
}
