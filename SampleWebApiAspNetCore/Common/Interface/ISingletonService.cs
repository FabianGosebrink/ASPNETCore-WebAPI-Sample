namespace SampleWebApiAspNetCore.Common.Interface;

/// <summary>
/// Interface for a singleton service in dependency injection.
/// Singleton services are created once and shared throughout the application.
/// 
/// If an interface inherits this interface, it will be automatically registered
/// by a common dependency injection configuration class.
/// </summary>
public interface ISingletonService
{
}
