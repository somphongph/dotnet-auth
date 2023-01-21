using Domain.Extensions;

namespace Domain.Models;

public enum AccountRole
{
    Admin,
    SuperUser,
    User
}

public static class AccountRoleName
{
    public static Locale Name(this AccountRole enumValue) => Name(enumValue.Code());

    public static string NameString(this AccountRole enumValue) => Name(enumValue.Code()).ToString() ?? "";

    public static Locale Name(string code) => Data().FirstOrDefault(m => m.Key.Equals(code)).Value ?? new Locale();

    public static Dictionary<string, Locale> Data()
    {
        var dict = new Dictionary<string, Locale>();

        dict.Add(AccountRole.Admin.Code(), Locale.Create("ผู้ดูแลระบบ", "Admin", ""));
        dict.Add(AccountRole.SuperUser.Code(), Locale.Create("ผู้ใช้งานขั้นสูง", "SuperUser", ""));
        dict.Add(AccountRole.User.Code(), Locale.Create("ผู้ใช้งาน", "User", ""));

        return dict;
    }
}
