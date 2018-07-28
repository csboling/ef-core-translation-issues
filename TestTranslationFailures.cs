namespace Issues.OpsAfterProjection
{
    using System;
    using System.Linq;

    using Xunit;

    // https://github.com/aspnet/EntityFrameworkCore/issues/12728
    public class TestTranslationFailures : IDisposable
    {
        public TestTranslationFailures()
        {
            this.DbContext = new ExampleDbContext();
            this.DbContext.Database.EnsureCreated();
        }

        private ExampleDbContext DbContext { get; }

        public void Dispose()
        {
            this.DbContext.Database.EnsureDeleted();
            this.DbContext.Dispose();
        }

        public IQueryable<LibraryDto> Project(IQueryable<Library> queryable) =>
            queryable
                .Select(l => new LibraryDto
                {
                    Id = l.Id,
                    City = new CityDto
                    {
                        Id = l.City.Id,
                        Name = l.City.Name,
                    },
                    Books = l.Books
                        .Select(b => new BookDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                        })
                        .ToList(),
                });

        public IQueryable<LibraryDto> ProjectWithoutToList(IQueryable<Library> queryable) =>
            queryable
                .Select(l => new LibraryDto
                {
                    Id = l.Id,
                    City = new CityDto
                    {
                        Id = l.City.Id,
                        Name = l.City.Name,
                    },
                    Books = l.Books
                        .Select(b => new BookDto
                        {
                            Id = b.Id,
                            Title = b.Title,
                        }),
                });

        #region SuccessfulProjections
        [Fact]
        public void Where_AfterProjection_OneToOneNavigation()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.Project(dbSet)
                .Where(l => l.City.Name == "Albuquerque");
            query.ToList();
        }

        [Fact]
        public void WhereAny_BeforeProjection()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.Project(
                dbSet.Where(l => l.Books.Any(b => b.Title == "The Cat in the Hat")));
            query.ToList();
        }
        #endregion

        #region NamedTypes
        [Fact] // translation fails
        public void WhereAny_AfterProjection()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.Project(dbSet)
                .Where(l => l.Books.Any(b => b.Title == "The Cat in the Hat"));
            query.ToList();
        }

        [Fact] // translation fails
        public void WhereAny_AfterProjection_WithoutComparison()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.Project(dbSet)
                .Where(l => l.Books.Any());
            query.ToList();
        }

        [Fact] // translation fails
        public void WhereAny_AfterProjectionWithoutToList()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.ProjectWithoutToList(dbSet)
                .Where(l => l.Books.Any(b => b.Title == "The Cat in the Hat"));
            query.ToList();
        }

        [Fact]
        public void WhereAny_AfterProjectionWithoutToList_WithoutComparison()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = this.ProjectWithoutToList(dbSet)
                .Where(l => l.Books.Any());
            query.ToList();
        }
        #endregion

        #region AnonymousTypes
        [Fact] // translation fails
        public void WhereAny_AnonymousTypeProjection()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = dbSet
                .Select(l => new
                {
                    Books = l.Books
                        .Select(b => new
                        {
                            b.Title,
                        })
                        .ToList(),
                })
                .Where(l => l.Books.Any(b => b.Title == "The Cat in the Hat"));
            query.ToList();
        }

        [Fact] // translation fails
        public void WhereAny_AnonymousTypeProjection_WithoutComparison()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = dbSet
                .Select(l => new
                {
                    Books = l.Books
                        .Select(b => new
                        {
                            b.Title,
                        })
                        .ToList(),
                })
                .Where(l => l.Books.Any());
            query.ToList();
        }

        [Fact] // translation fails
        public void WhereAny_AnonymousTypeProjectionWithoutToList()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = dbSet
                .Select(l => new
                {
                    Books = l.Books
                        .Select(b => new
                        {
                            b.Title,
                        }),
                })
                .Where(l => l.Books.Any(b => b.Title == "The Cat in the Hat"));
            query.ToList();
        }

        [Fact]
        public void WhereAny_AnonymousTypeProjectionWithoutToList_WithoutComparison()
        {
            var dbSet = this.DbContext.Set<Library>();
            var query = dbSet
                .Select(l => new
                {
                    Books = l.Books
                        .Select(b => new
                        {
                            b.Title,
                        }),
                })
                .Where(l => l.Books.Any());
            query.ToList();
        }
        #endregion
    }
}
