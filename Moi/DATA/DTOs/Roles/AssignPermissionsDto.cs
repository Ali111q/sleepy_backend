namespace GaragesStructure.DATA.DTOs.roles
{
    public class AssignPermissionsDto : BaseDto<Guid>
    {
        public List<Guid> Permissions { get; set; }
    }
    
    
    public class MyPermissionsListDto : BaseDto<Guid>
    {
        public List<MyPermissionsDto> Data { get; set; }
    }
    
    public class MyPermissionsDto : BaseDto<Guid>
    {
    
        public string Subject { get; set; }
        public IEnumerable<string> Actions { get; set; }
    }
}