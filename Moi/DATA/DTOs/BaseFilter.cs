using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GaragesStructure.DATA.DTOs
{
    public class BaseFilter
    {
        
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public bool Deleted { get; set; } = false;

    }


}