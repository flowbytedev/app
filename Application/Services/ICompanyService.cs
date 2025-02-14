﻿using Application.Shared.Models;

namespace Application.Services
{
    public interface ICompanyService
    {
        Task<List<Company>> GetCompanies(string userId);

        Task<Company> GetCompany(string id);

        Task<Company> GetCompany(string id, string userId);
        Task<bool> UserIsCompanyMember(string id, string userId);

        Task<CompanyMember> AddCompanyMember(string companyId, string userId);

        Task<CompanyMember> AddCompanyMemberByDomain(string domain, string userId);
    }
}
