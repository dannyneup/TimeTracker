using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Project;

namespace TimeTracker.UI.Windows.Pages.ProjectOverviewPage;

public sealed class ProjectOverviewPageViewModel : NotifyPropertyChangedBase, IPageViewModel
{
    public string Title => Resources.projectOverviewPageTitle;
    public ObservableCollection<ProjectResponseModel>? Projects { 
        get => _projects; 
        set => SetField(ref _projects, value); 
    }
    
    private ObservableCollection<ProjectResponseModel>? _projects;
    
    private readonly IProjectRepository _projectRepository;

    public ProjectOverviewPageViewModel(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository; 
    }
    
    public async Task OnActivated()
    {
        var (projects, isSuccess) = await _projectRepository.GetAllAsync();
        if (!isSuccess) return;
        var projectList = projects.ToList();
        Projects = new ObservableCollection<ProjectResponseModel>(projectList);
    }

    public void OnDeactivated()
    {
        return;
    }
}