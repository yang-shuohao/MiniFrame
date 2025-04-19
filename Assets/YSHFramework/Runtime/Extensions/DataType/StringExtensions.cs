
public static class StringExtensions
{
    public static bool CustomEndsWith(this string a, string b)
    {
        if (a == null || b == null) return false;
        if (b.Length > a.Length) return false;

        for (int i = 1; i <= b.Length; i++)
        {
            if (a[a.Length - i] != b[b.Length - i])
                return false;
        }

        return true;
    }

    public static bool CustomStartsWith(this string a, string b)
    {
        if (a == null || b == null) return false;
        if (b.Length > a.Length) return false;

        for (int i = 0; i < b.Length; i++)
        {
            if (a[i] != b[i])
                return false;
        }

        return true;
    }
}
