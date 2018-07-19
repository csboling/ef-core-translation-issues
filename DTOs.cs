namespace Issues.OpsAfterProjection
{
    using System;
    using System.Collections.Generic;

    public class BaseDto
    {
        public int Id { get; set; }
    }

    public class LibraryDto : BaseDto
    {
        public CityDto City { get; set; }

        public IEnumerable<BookDto> Books { get; set; }
    }

    public class CityDto : BaseDto
    {
        public string Name { get; set; }
    }

    public class BookDto : BaseDto
    {
        public string Title { get; set; }
    }
}
