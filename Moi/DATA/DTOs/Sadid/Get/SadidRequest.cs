using System.ComponentModel.DataAnnotations;

namespace GaragesStructure.DATA.DTOs.Sadid.Get;

public class SadidRequest
{
    [Required]
    public string? SecretKey { get; set; }

    [Required]
    public long? BillId { get; set; }
}