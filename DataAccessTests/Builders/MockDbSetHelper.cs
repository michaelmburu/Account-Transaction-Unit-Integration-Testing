using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessTests.Builders
{
    public static class MockDbSetHelper
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(
            params T[] sourceList) where T : class
        {
            var queryable = sourceList.AsQueryable();

            var dbSet = new Mock<DbSet<T>>();
            dbSet.As<IQueryable<T>>().Setup(m => m.Provider)
                .Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression)
                .Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType)
                .Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator())
                .Returns(queryable.GetEnumerator());

            return dbSet.Object;
        }

        public static Mock<DbSet<T>> GetMockDbSet<T>(
            IQueryable<T> entities) where T : class
        {
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Provider)
                .Returns(entities.Provider);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.Expression)
                .Returns(entities.Expression);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.ElementType)
                .Returns(entities.ElementType);
            mockSet.As<IQueryable<T>>()
                .Setup(m => m.GetEnumerator())
                .Returns(entities.GetEnumerator());
            return mockSet;
        }
    }
}
