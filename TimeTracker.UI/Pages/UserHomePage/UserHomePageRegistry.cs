using Lamar;

namespace TimeTracker.UI.Pages.UserHomePage;

public class UserHomePageRegistry : ServiceRegistry
{
    public UserHomePageRegistry()
    {
        For<UserHomePageViewModel>().Use<UserHomePageViewModel>();
    }
}