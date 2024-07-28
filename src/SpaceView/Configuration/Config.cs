namespace SpaceView.Configuration;

public class Config : IConfig
{
    public ConnectionStrings ConnectionStrings { get; } = new();
    public ApiSettings ApiSettings { get; } = new();
}