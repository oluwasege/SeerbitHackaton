using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeerbitHackaton.Core.Entities.Mapping
{
    public class CompanyMap
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            SetupCompany(builder);
        }

        private void SetupCompany(EntityTypeBuilder<Company> builder)
        {
            var company = new Company()
            {
                Id = 1,
                CAC = "RTYIUEP",
                Name = "Seerbit",

            };
        }
    }

    public class CompanyAdminMap
    {
        public void Configure(EntityTypeBuilder<CompanyAdmin> builder)
        {
            SetupCompanyAdmin(builder);
        }

        private void SetupCompanyAdmin(EntityTypeBuilder<CompanyAdmin> builder)
        {
            var company = new CompanyAdmin()
            {
                Id = 1,
                UserId=2,
                 CompanyId=1
            };
        }
    }
}
