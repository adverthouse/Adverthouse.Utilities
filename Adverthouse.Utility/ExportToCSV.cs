namespace Adverthouse.Utility
{
    public class ExportToCSV
    {
        public string QuoteName(string s, string quote = null, string sep = ",")
        {
            quote = quote == null ? "" : quote;
            switch (quote.Length)
            {
                case 0:
                    quote = "\"\"";
                    break;
                case 1:
                    quote += quote;
                    break;
            }
            // Fields with embedded sep are quoted
            if ((!s.StartsWith(quote.Substring(0, 1))) && (!s.EndsWith(quote.Substring(1, 1))))
                if (s.Contains(sep))
                    s = quote.Substring(0, 1) + s + quote.Substring(1, 1);
            // Fields with leading or trailing blanks are quoted
            if ((!s.StartsWith(quote.Substring(0, 1))) && (!s.EndsWith(quote.Substring(1, 1))))
                if (s.StartsWith(" ") || s.EndsWith(" "))
                    s = quote.Substring(0, 1) + s + quote.Substring(1, 1);
            // Fields with embedded CrLF are quoted
            if ((!s.StartsWith(quote.Substring(0, 1))) && (!s.EndsWith(quote.Substring(1, 1))))
                if (s.Contains(System.Environment.NewLine))
                    s = quote.Substring(0, 1) + s + quote.Substring(1, 1);
            return s;
        }

        public ExportToCSV() { }
    }
}
