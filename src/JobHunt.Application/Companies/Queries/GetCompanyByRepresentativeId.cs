using JobHunt.Application.Common.CQRS;
using JobHunt.Application.Common.Interfaces;
using JobHunt.Domain.Shared;
using Microsoft.EntityFrameworkCore;

namespace JobHunt.Application.Companies.Queries;

public sealed record GetCompanyByRepresentativeId(string RepresentativeId)
    : IQuery<CompanyDto>;

public class GetCompanyByRepresentativeIdHanlder(IJobHuntDbContext dbContext) : IQueryHandler<GetCompanyByRepresentativeId, CompanyDto>
{
    private readonly IJobHuntDbContext _dbContext = dbContext;
    
    public async Task<Result<CompanyDto>> Handle(GetCompanyByRepresentativeId request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users.FindAsync(request.RepresentativeId, cancellationToken);

        if (user == null)
        {
            return Result.Failure<CompanyDto>(
                new Error("GetCompany.Failed", "Company representative not found"));
        }

        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => 
            c.CompanyRepresentativeId == request.RepresentativeId, cancellationToken: cancellationToken);
        if (company == null)
        {
            return Result.Failure<CompanyDto>(
                new Error("GetCompany.Failed", "Company not found"));
        }
        
        return Result.Success(new CompanyDto()
        {
            Id = company.Id,
            Name = company.Name,
            CompanyRepresentativeId = company.CompanyRepresentativeId,
            Description = company.Description,
            Location = company.Location,
            Logo = company.Location,
            Phone = company.Phone
        });
    }
}