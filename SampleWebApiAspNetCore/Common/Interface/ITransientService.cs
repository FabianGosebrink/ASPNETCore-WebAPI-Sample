namespace SampleWebApiAspNetCore.Common.Interface;

/// <summary>
/// Interface for a transient service in dependency injection.
/// Transient services are created anew each time they are requested.
/// 
/// If an interface inherits this interface, it will be automatically registered
/// by a common dependency injection configuration class.
/// </summary>
public interface ITransientService
{
}

