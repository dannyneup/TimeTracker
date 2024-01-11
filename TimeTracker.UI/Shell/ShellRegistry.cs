using Lamar;

namespace TimeTracker.UI.Shell;

public class ShellRegistry : ServiceRegistry
{
    public ShellRegistry()
    {
        For<ApplicationViewModel>().Use<ApplicationViewModel>().Singleton();
    }
}