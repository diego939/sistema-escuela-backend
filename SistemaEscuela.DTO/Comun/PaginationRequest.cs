namespace SistemaEscuela.DTO.Comun
{
	public class PaginationRequest
	{
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public string Search { get; set; } = string.Empty;
		public string SortBy { get; set; } = "nombres";
		public bool SortDescending { get; set; } = false;
	}
}
