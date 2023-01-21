namespace Domain.Helpers
{
    public static class RandomHelper
    {
        public static string RandomKey(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var result = new char[length];
            var rnd = new Random();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[rnd.Next(chars.Length)];
            }

            return new String(result);
        }

        public static string RandomNumber(int length)
        {
            var chars = "0123456789";
            var result = new char[length];
            var rnd = new Random();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[rnd.Next(chars.Length)];
            }

            return new String(result);
        }

        public static string RandomToLower(int length)
        {
            var chars = "abcdefghijklmnopqrstuvwxyz";
            var result = new char[length];
            var rnd = new Random();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[rnd.Next(chars.Length)];
            }

            return new String(result);
        }

        public static string RandomToUpper(int length)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var result = new char[length];
            var rnd = new Random();

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[rnd.Next(chars.Length)];
            }

            return new String(result);
        }
    }
}
