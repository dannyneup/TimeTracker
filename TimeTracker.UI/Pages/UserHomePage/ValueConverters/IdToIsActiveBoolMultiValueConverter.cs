using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia;
using Avalonia.Data.Converters;

namespace TimeTracker.UI.Pages.UserHomePage.ValueConverters;

public class IdToIsActiveBoolMultiValueConverter : IMultiValueConverter
{
    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
    {
        if (values.Any(x => x is UnsetValueType)) return false;
        var inverted = ((string)parameter! == "inverted");
        if (values[0] is int itemId && values[1] is int activeItemId)
        {
            return inverted? 
                itemId != activeItemId
                : itemId == activeItemId;
        }
        return false;
    }
    
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}