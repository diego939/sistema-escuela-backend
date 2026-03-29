namespace SistemaEscuela.DTO.Comun
{
	public class PaginatedResult<T> where T : class
	{
		public List<T> Data { get; set; } = new List<T>();
		public int TotalRecords { get; set; }
		public int TotalPages { get; set; }
		public int PageNumber { get; set; }
		public int PageSize { get; set; }
		public bool HasPreviousPage => PageNumber > 1;
		public bool HasNextPage => PageNumber < TotalPages;
	}
}
