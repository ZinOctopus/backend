namespace Zin.Infra.Database.Entities;

public class DeviceMetric
{
    public long Id { get; set; }
    public Guid DeviceId { get; set; }
    public DateTime At { get; set; }
    public double Cpu { get; set; }
    public double Ram { get; set; }
    public double Disk { get; set; }
    public double NetIn { get; set; }
    public double NetOut { get; set; }
}
