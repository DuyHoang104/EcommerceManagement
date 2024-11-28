using System.ComponentModel;

public static class EnumExtension
{
    public static string GetEnumDescription(this Enum value)
    {
        var fieldInfo = value.GetType().GetField(value.ToString());
        var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

        if (attributes != null && attributes.Length > 0)
        {
            return attributes[0].Description;
        }

        return value.ToString();
    }
}