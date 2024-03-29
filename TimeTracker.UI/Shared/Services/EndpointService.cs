﻿using System;
using System.Collections.Generic;
using TimeTracker.UI.Shared.Models;
using TimeTracker.UI.Shared.Models.Employee;
using TimeTracker.UI.Shared.Models.Project;

namespace TimeTracker.UI.Shared.Services;

public class EndpointService
{
    private readonly Uri _baseUrl;
    private List<Endpoint> _endpoints = [];
    
    public EndpointService(Uri baseUrl)
    {
        _baseUrl = baseUrl;
        _endpoints.Add(new Endpoint("/employees/{0}", typeof(EmployeeRequestModel), typeof(EmployeeResponseModel)));
        _endpoints.Add(new Endpoint("/projects/{0}", typeof(ProjectRequestModel), typeof(ProjectResponseModel)));
        _endpoints.Add(new Endpoint("/employees/{0}/projects", typeof(EmployeeRequestModel), typeof(ProjectResponseModel)));
    }

    public string GetUrl<TRequest, TResponse>()
    {
        var endpoint = _endpoints.Find(endpoint =>
            endpoint.RequestType == typeof(TRequest) && endpoint.ResponseType == typeof(TResponse))
            ?.Url;
        var baseUrl = _baseUrl.ToString();
        return baseUrl.Remove(baseUrl.Length - 1) + endpoint;
    }
}