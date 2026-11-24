namespace Zin.Infra.Database.Entities;

public class DeviceGroup
{
    public Guid DeviceId { get; set; }
    public Guid GroupId { get; set; }
}

public class UserGroupAccess
{
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
}

public class UserDeviceAccess
{
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
}
