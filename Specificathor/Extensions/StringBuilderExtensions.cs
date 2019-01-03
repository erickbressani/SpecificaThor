using System.Text;

namespace SpecificaThor
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendMessage(this StringBuilder source, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (source.Length == 0)
                    source.Append(message);
                else
                    source.Append($"\n{message}");
            }

            return source;
        }
    }
}
