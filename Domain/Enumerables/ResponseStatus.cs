using Domain.Extensions;

namespace Domain.Models;

public enum ResponseStatus
{
    Success,
    NotFound,
    InvalidData,
    Duplicated
}

public static class ResponseCodeName
{
    public static Locale Name(this ResponseStatus enumValue) => Name(enumValue.Code());

    public static string NameString(this ResponseStatus enumValue) => Name(enumValue.Code()).ToString() ?? "";

    public static Locale Name(string code) => Data().FirstOrDefault(m => m.Key.Equals(code)).Value ?? new Locale();

    public static Dictionary<string, Locale> Data()
    {
        var dict = new Dictionary<string, Locale>();

        dict.Add(ResponseStatus.Success.Code(), Locale.Create("การดำเนินการสำเร็จ", "Action successfully", ""));
        dict.Add(ResponseStatus.NotFound.Code(), Locale.Create("ไม่พบข้อมูล", "Data not found", ""));
        dict.Add(ResponseStatus.InvalidData.Code(), Locale.Create("ข้อมูลไม่ถูกต้อง", "Invalid data", ""));
        dict.Add(ResponseStatus.Duplicated.Code(), Locale.Create("ไม่สามารถระบุข้อมูลซ้ำกันได้", "Duplicated data", ""));

        return dict;
    }
}
