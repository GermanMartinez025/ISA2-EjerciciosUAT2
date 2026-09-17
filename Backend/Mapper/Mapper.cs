using System.Linq.Expressions;
using System.Reflection;

namespace Mapper;

public class Mapper<TFrom, TTo>
    where TFrom : class
    where TTo : class, new()
{
    private readonly
        List<(PropertyInfo fromProperty, PropertyInfo toProperty, Func<object, object> transform, object defaultValue)>
        _mapValues = new List<(PropertyInfo, PropertyInfo, Func<object, object>, object)>();

    public List<string> Errors = new List<string>();

    private Func<TFrom, TTo> _customMapper;

    public Mapper<TFrom, TTo> UseCustomMapper(Func<TFrom, TTo> customMapper)
    {
        _customMapper = customMapper;
        return this;
    }

    public Mapper<TFrom, TTo> MapValues(Expression<Func<TFrom, object>> fromExpression,
        Expression<Func<TTo, object>> toExpression, Func<object, object> transform = null!, object defaultValue = null!)
    {
        try
        {
            var fromProperty = GetPropertyInfo(fromExpression);
            var toProperty = GetPropertyInfo(toExpression);

            _mapValues.Add((fromProperty, toProperty, transform, defaultValue));
        }
        catch (Exception ex)
        {
            Log($"Error configuring property mapping: {ex.Message}");
        }

        return this;
    }

    public TTo Convert(TFrom from)
    {
        try
        {
            if (_customMapper != null)
            {
                return _customMapper(from);
            }

            if (_mapValues.Count == 0) return (TTo)Activator.CreateInstance(typeof(TTo), from)!;

            TTo to = new TTo();

            foreach (var (fromProperty, toProperty, transform, defaultValue) in _mapValues)
            {
                try
                {
                    var value = fromProperty.GetValue(from);

                    if (value == null && defaultValue != null) value = defaultValue;

                    if (transform != null && value != null) value = transform(value);

                    toProperty.SetValue(to, value);
                }
                catch (Exception ex)
                {
                    Log($"Error mapping {fromProperty.Name} to {toProperty.Name}: {ex.Message}");
                }
            }

            return to;
        }
        catch (Exception ex)
        {
            Log($"General error during conversion: {ex.Message}");
            throw;
        }
    }

    private PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> expression)
    {
        try
        {
            MemberExpression member = (expression.Body as MemberExpression)!;

            if (member == null)
            {
                var unary = expression.Body as UnaryExpression;
                if (unary != null)
                {
                    member = (unary.Operand as MemberExpression)!;
                }
            }

            return (member!.Member as PropertyInfo)!;
        }
        catch (Exception ex)
        {
            Log($"Error retrieving property information: {ex.Message}");
            throw;
        }
    }

    private void Log(string message)
    {
        Errors.Add(message);
        Console.WriteLine(message);
    }
}