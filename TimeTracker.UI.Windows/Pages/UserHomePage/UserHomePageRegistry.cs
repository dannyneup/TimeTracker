using Lamar;

namespace TimeTracker.UI.Windows.Pages.UserHomePage;

public class UserHomePageRegistry : ServiceRegistry
{
    public UserHomePageRegistry()
    {
        For<UserHomePageViewModel>().Use<UserHomePageViewModel>();
    }
}