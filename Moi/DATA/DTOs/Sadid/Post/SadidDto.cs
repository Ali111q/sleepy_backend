namespace GaragesStructure.DATA.DTOs.Sadid;

public class Provider
{
    public Guid? Id { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
    public string? Image { get; set; }
    public string? CompanyName { get; set; }
}

public class BillService
{
    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public decimal? Price { get; set; }
    public string? Description { get; set; }
}

public class SadidDto
{
    public Guid? Id { get; set; }
    public bool? Deleted { get; set; }
    public DateTime? CreatedAt { get; set; }
    public long? BillId { get; set; }
    public DateTime? ExpireDate { get; set; }
    public decimal? TotalPrice { get; set; }
    public Provider? Provider { get; set; }
    public List<BillService>? BillServices { get; set; }
    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }
    public DateTime? PaymentDate { get; set; }
    public int? BillStatus { get; set; }
    public int? PaymentMethod { get; set; }
    public int? BillType { get; set; }
}