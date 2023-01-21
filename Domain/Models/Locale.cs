namespace Domain.Models
{
    public class Locale
    {
        public string En { get; set; } = string.Empty;

        public string Th { get; set; } = string.Empty;

        public string Cn { get; set; } = string.Empty;

        public override string ToString()
        {
            switch (Thread.CurrentThread.CurrentUICulture.Name.ToLower())
            {
                case "th":
                case "th-th":
                    return Th;

                case "cn":
                case "zh-cn":
                    return Cn;

                default:
                    return En;
            }
        }

        public static Locale Create(string Th, string En, string Cn)
        {
            return new Locale()
            {
                En = En,
                Th = Th,
                Cn = Cn
            };
        }
    }
}
