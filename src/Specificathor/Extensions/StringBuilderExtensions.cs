using System.Text;

namespace SpecificaThor
{
    internal static class StringBuilderExtensions
    {
        public static StringBuilder AppendMessage(this StringBuilder source, string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (source.Length > 0)
                    source.AppendLine();

                source.Append(message);
            }

            return source;
        }
    }
}
