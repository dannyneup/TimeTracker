using System;

namespace TimeTracker.UI.Windows.Shared.Models;

public record Endpoint(string Url, Type RequestType, Type ResponseType);