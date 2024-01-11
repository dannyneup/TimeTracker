using Lamar;
    
namespace TimeTracker.UI.Windows.Shell;

public class ShellRegistry : ServiceRegistry
{
    public ShellRegistry()
    {
        For<ApplicationViewModel>().Use<ApplicationViewModel>().Singleton();
    }
}