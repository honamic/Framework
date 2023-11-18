using Microsoft.EntityFrameworkCore;

namespace Honamic.Framework.Facade.FastCrud;

public class FastCrudDbContext : DbContext
{
    public FastCrudDbContext(DbContextOptions options) : base(options)
    {

    }
}
