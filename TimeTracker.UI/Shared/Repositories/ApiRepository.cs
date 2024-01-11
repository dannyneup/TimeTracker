using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using TimeTracker.UI.Windows.Shared.Interfaces;
using TimeTracker.UI.Windows.Shared.Interfaces.Repositories;
using TimeTracker.UI.Windows.Shared.Services;

namespace TimeTracker.UI.Windows.Shared.Repositories;

public class ApiRepository<TRequest, TResponse> : IRepository<TRequest, TResponse>
    where TResponse : IResponseModel, new()
{
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
    }

    public async Task<(IEnumerable<TResponse>, bool)> GetAllAsync()
    {
        var (response, isResponseSuccess) = await GetAllAsync(NoIdRequestUrl);
        return (response, isResponseSuccess);
    }

    public async Task<(IEnumerable<TResponse>, bool)> GetAllAsync(int id)
    {
        var requestUrl = GetIdRequestUrl(id);
        var (response, isResponseSuccess) = await GetAllAsync(requestUrl);
        return (response, isResponseSuccess);
    }

    public async Task<(TResponse, bool)> GetAsync(int id)
    {
        var requestUrl = GetIdRequestUrl(id);
        try
        {
            var response = await HttpClient.GetFromJsonAsync<TResponse>(requestUrl);
            return (response ?? new TResponse(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TResponse(), false);
        }
    }

    public async Task<(TResponse, bool)> AddAsync(TRequest request)
    {
        try
        {
            var response = await HttpClient.PostAsJsonAsync(NoIdRequestUrl, request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseString);
            return (result ?? new TResponse(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TResponse(), false);
        }
    }

    public async Task<(TResponse, bool)> UpdateAsync(int id, TRequest request)
    {
        var requestUrl = GetIdRequestUrl(id);
        try
        {
            var response = await HttpClient.PutAsJsonAsync(requestUrl, request);
            var responseString = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<TResponse>(responseString);
            return (result ?? new TResponse(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (new TResponse(), false);
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

    protected async Task<(IEnumerable<TResponse>, bool)> GetAllAsync(string requestUrl) =>
        await GetAllAsync<TResponse>(requestUrl);

    protected async Task<(IEnumerable<TRes>, bool)> GetAllAsync<TRes>(string requestUrl)
    {
        try
        {
            var response = await HttpClient.GetFromJsonAsync<List<TRes>>(requestUrl);
            return (response ?? Enumerable.Empty<TRes>(), true);
        }
        catch (Exception e) when (e is HttpRequestException or JsonException)
        {
            Debug.WriteLine(e);
            return (Enumerable.Empty<TRes>(), false);
        }
    }

    private string GetIdRequestUrl(int id)
    {
        var url = string.Format(RequestUrl, id);
        return url;
    }
}