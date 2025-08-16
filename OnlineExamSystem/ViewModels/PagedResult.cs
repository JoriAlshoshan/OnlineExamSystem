namespace OnlineExamSystem.ViewModels
{
    public class PagedResult<T> where T : class
    {
        public List<T> Data { get; set; } = new List<T>();
        public int TotalItem { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public IEnumerable<T> Items => Data; 
        public int CurrentPage => PageNumber; 
        public int TotalPages => (int)Math.Ceiling((double)TotalItem / PageSize);
    }
}
