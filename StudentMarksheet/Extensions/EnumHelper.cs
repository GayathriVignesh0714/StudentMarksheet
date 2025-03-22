using System.ComponentModel;
using System.Reflection;

namespace StudentMarksheet.Extensions
{
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}
