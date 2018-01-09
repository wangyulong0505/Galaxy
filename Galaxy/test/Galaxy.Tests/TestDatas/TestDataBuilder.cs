using Galaxy.EntityFrameworkCore;

namespace Galaxy.Tests.TestDatas
{
    public class TestDataBuilder
    {
        private readonly GalaxyDbContext _context;

        public TestDataBuilder(GalaxyDbContext context)
        {
            _context = context;
        }

        public void Build()
        {
            //create test data here...
        }
    }
}