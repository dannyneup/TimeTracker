using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using TimeTracker.UI.Windows.Pages.EmployeeOverviewPage.Records;
using TimeTracker.UI.Windows.Pages.ProjectOverviewPage.Records;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Models.Employee;
using TimeTracker.UI.Windows.Shared.Models.Project;
using TimeTracker.UI.Windows.Shared.Services;

namespace TimeTracker.UI.Windows.Shared.Repositories;

public class ApiRepository<TModel, TRequest, TResponse> : IRepository<TModel>
    where TResponse : IResponseModel
    where TModel : IModel, new()
{
    protected readonly IMapper Mapper;
    protected readonly HttpClient HttpClient;
    protected readonly EndpointService EndpointService;
    protected readonly string RequestUrl;
    protected readonly string NoIdRequestUrl;

    public ApiRepository(EndpointService endpointService)
    {
        HttpClient = new HttpClient();
        EndpointService = endpointService;
        RequestUrl = EndpointService.GetUrl<TRequest, TResponse>();
        NoIdRequestUrl = RequestUrl.Replace("/{0}", "");
        
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<EmployeeResponseModel, Employee>();
            cfg.CreateMap<Employee, EmployeeRequestModel>();
            cfg.CreateMap<ProjectResponseModel, Project>();
            cfg.CreateMap<Project, ProjectRequestModel>();
        });

        Mapper = new Mapper(configuration);
    }

    public async Task<(IEnumerable<TModel>, bool)> GetAllAsync()
    {
        var (response, isResponseSuccess) = await GetAllAsync(NoIdRequestUrl);
        var model = Mapper.Map<IEnumerable<TModel>>(response);
        return (model, isResponseSuccess);
    }

    public async Task<(IEnumerable<TModel>, bool)> GetAllAsync(int id)
    {
        var requestUrl = GetIdRequestUrl(id);
        var (response, isResponseSuccess) = await GetAllAsync(requestUrl);
        return (Mapper.Map<IEnumerable<TModel>>(response), isResponseSuccess);
    }

    public async Task<(TModel, bool)> GetAsync(int id)
    {
        var requestUrl = GetIdRequestUrl(id);
        try
        {
            var response = await HttpClient.GetFromJsonAsync<TResponse>(requestUrl);
            return (Mapper.Map<TModel>(response) ?? new TModel(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TModel(), false);
        }
    }

    public async Task<(TModel, bool)> AddAsync(TModel input)
    {
        var request = Mapper.Map<TRequest>(input);
        try
        {
            var response = await HttpClient.PostAsJsonAsync(NoIdRequestUrl, request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseString);
            var model = Mapper.Map<TModel>(result);
            return (model ?? new TModel(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TModel(), false);
        }
    }

    public async Task<(TModel, bool)> UpdateAsync(TModel input)
    {
        var request = Mapper.Map<TRequest>(input);
        var requestUrl = GetIdRequestUrl(input.Id);
        try
        {
            var response = await HttpClient.PutAsJsonAsync(requestUrl, request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseString);
            var model = Mapper.Map<TModel>(result);
            return (model ?? new TModel(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TModel(), false);
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var requestUrl = GetIdRequestUrl(id);
        try
        {
            var response = await HttpClient.DeleteAsync(requestUrl);
            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException e)
        {
            Debug.WriteLine(e);
            return false;
        }
    }

    private async Task<(IEnumerable<TResponse>, bool)> GetAllAsync(string requestUrl)
    {
        try
        {
            var response = await HttpClient.GetFromJsonAsync<List<TResponse>>(requestUrl);
            return (response ?? Enumerable.Empty<TResponse>(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (Enumerable.Empty<TResponse>(), false);
        }
    }

    private string GetIdRequestUrl(int id)
    {
        var url = string.Format(RequestUrl, id);
        return url;
    }
}