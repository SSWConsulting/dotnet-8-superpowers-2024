using HeroJobs.Application.Common.Interfaces;
using HeroJobs.Infrastructure.Persistence;
using HeroJobs.Infrastructure.Persistence.Interceptors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Application.UnitTests.Common;

public static class ApplicationDbContextFactory
{
    public static ApplicationDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var currentUserService = Substitute.For<ICurrentUserService>();
        var dateTime = Substitute.For<IDateTime>();
        var mediator = Substitute.For<IMediator>();
        var entitySaveChangesInterceptor = new EntitySaveChangesInterceptor(currentUserService, dateTime);
        var dispatchDomainEventsInterceptor = new DispatchDomainEventsInterceptor(mediator);
        var dbContext =
            new ApplicationDbContext(options, entitySaveChangesInterceptor, dispatchDomainEventsInterceptor);

        dbContext.Database.EnsureCreated();

        return dbContext;
    }
}