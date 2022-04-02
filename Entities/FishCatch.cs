namespace Api.Entities;

public class FishCatch
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime CatchDate { get; set; }
    public string NymphName { get; set; }
    public string NymphColor { get; set; }
    public double? HookSize { get; set; }
    public string NymphHead { get; set; }
    public string LakeName { get; set; }
    public double? DeepLocation { get; set; }
    public double? DeepFishCatch { get; set; }
    public double? WaterTemperature { get; set; }
    public string Weather { get; set; }
    public double? AirPressure { get; set; }
    public double? WindSpeed { get; set; }
    public double? AirTemperature { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool AllowPublic { get; set; }
}