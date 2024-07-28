namespace SpaceView.Configuration;

public interface IConfig
{
    ConnectionStrings ConnectionStrings { get; }
    ApiSettings ApiSettings { get; }
}