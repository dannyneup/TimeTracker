using System;

namespace TimeTracker.UI.Shared.Models;

public record Endpoint(string Url, Type RequestType, Type ResponseType);