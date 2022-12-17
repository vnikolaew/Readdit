using Microsoft.EntityFrameworkCore;
using Readdit.Infrastructure.Data;

namespace Readdit.Tests.Common;

public static class InMemoryDbContextProvider
{
    private static ReadditDbContext? _context;
    
    public static ReadditDbContext Instance
    {
        get
        {
            if (_context is not null) return _context;
            var options = new DbContextOptionsBuilder<ReadditDbContext>()
                .UseInMemoryDatabase("MemoryDb")
                .Options;
                
            _context = new ReadditDbContext(options);

            return _context;
        }
    }
    
}