using Domain.Extensions;

namespace Domain.Models;

public enum RecordStatus
{
    Active,
    Disabled,
    Deleted
}

public static class RecordStatusName
{
    public static Locale Name(this RecordStatus enumValue) => Name(enumValue.Code());

    public static string NameString(this RecordStatus enumValue) => Name(enumValue.Code()).ToString() ?? "";

    public static Locale Name(string code) => Data().FirstOrDefault(m => m.Key.Equals(code)).Value ?? new Locale();

    public static Dictionary<string, Locale> Data()
    {
        var dict = new Dictionary<string, Locale>();

        dict.Add(RecordStatus.Active.Code(), Locale.Create("ใช้งาน", "Active", ""));
        dict.Add(RecordStatus.Disabled.Code(), Locale.Create("ไม่ใช้งาน", "Disabled", ""));
        dict.Add(RecordStatus.Deleted.Code(), Locale.Create("ลบ", "Deleted", ""));

        return dict;
    }
}
