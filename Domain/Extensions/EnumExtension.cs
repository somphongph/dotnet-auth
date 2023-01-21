namespace Domain.Extensions
{
    public static class EnumExtension
    {
        public static string Code(this Enum enumValue)
        {
            return enumValue.ToString().ToSnakeCase();
        }
    }
}