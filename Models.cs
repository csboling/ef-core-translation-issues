namespace Issues.OpsAfterProjection
{
    using System.Collections.Generic;

    public class Library
    {
        public int Id { get; set; }

        public List<Book> Books { get; set; }

        public City City { get; set; }
    }

    public class City
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int LibraryId { get; set; }

        public Library Library { get; set; }
    }

    public class Book
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Library Library { get; set; }
    }
}
