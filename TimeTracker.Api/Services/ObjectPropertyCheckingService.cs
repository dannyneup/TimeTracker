namespace TimeTracker.Api.Services;

public class ObjectPropertyCheckingService
{
    public bool HasNullOrEmptyProperties(object obj)
    {
        var properties = obj.GetType().GetProperties();

        return properties.Any(property =>
        {
            var value = property.GetValue(obj);

            if (value == null)
            {
                return true; // Property ist null
            }

            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                return true; // String-Property ist leer oder null
            }

            if (value is Array arrayValue && arrayValue.Length == 0)
            {
                return true; // Array-Property ist leer
            }

            return false; // Property ist weder null noch leer
        });
    }
}