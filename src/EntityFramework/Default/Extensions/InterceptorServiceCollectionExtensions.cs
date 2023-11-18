using Honamic.Framework.Persistence.EntityFramework.Interceptors.AggregateRootVersion;
using Honamic.Framework.Persistence.EntityFramework.Interceptors.IsDeletedInfos;
using Honamic.Framework.Persistence.EntityFramework.Interceptors.PersianYeKe;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Honamic.Framework.Persistence.EntityFramework.Extensions;

public static class InterceptorServiceCollectionExtensions
{
    public static DbContextOptionsBuilder AddPersianYeKeCommandInterceptor(this DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new PersianYeKeCommandInterceptor());

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder AddAggregateRootVersionInterceptor(this DbContextOptionsBuilder optionsBuilder,
          IServiceProvider serviceProvider)
    {
        var logger = serviceProvider.GetRequiredService<ILogger<AggregateRootVersionInterceptor>>();

        optionsBuilder.AddInterceptors(new AggregateRootVersionInterceptor(logger));

        return optionsBuilder;
    }

    public static DbContextOptionsBuilder<TContext> AddMarkAsDeletedInterceptors<TContext>(this DbContextOptionsBuilder<TContext> optionsBuilder) where TContext : DbContext
    {
        return optionsBuilder.AddInterceptors(new MarkAsDeletedSaveChangesInterceptor());
    }

    public static DbContextOptionsBuilder AddMarkAsDeletedInterceptors(this DbContextOptionsBuilder optionsBuilder)
    {
        return optionsBuilder.AddInterceptors(new MarkAsDeletedSaveChangesInterceptor());
    }
}