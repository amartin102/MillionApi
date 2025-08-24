using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public record PropertyFilterDto
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string? Address { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 12; 
        public string? SortBy { get; set; } = "price"; 
        public string? SortDir { get; set; } = "asc"; 
    }
}
