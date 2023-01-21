namespace Domain.Extensions
{
    public static class StringExtension
    {
        public static string ToSnakeCase(this string str)
        {
            return string.Concat((str)
                    .Select((x, i) => i > 0 && i < str.Length - 1 && char.IsUpper(x) && !char.IsUpper(str[i - 1])
                        ? $"_{x}"
                        : x.ToString())).ToLower();
        }
    }
}
