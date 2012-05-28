using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Utils
{
	class ReplaceEx
	{
		static public string Replace(string original, string pattern, string replacement, StringComparison comparisonType)
		{
			return Replace(original, pattern, replacement, comparisonType, -1);
		}

		static public string Replace(string original, string pattern, string replacement, StringComparison comparisonType, int stringBuilderInitialSize)
		{
			if (original == null)
			{
				return null;
			}

			if (String.IsNullOrEmpty(pattern))
			{
				return original;
			}

			int posCurrent = 0;
			int lenPattern = pattern.Length;
			int idxNext = original.IndexOf(pattern, comparisonType);
			StringBuilder result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, original.Length) : stringBuilderInitialSize);

			while (idxNext >= 0)
			{
				result.Append(original, posCurrent, idxNext - posCurrent);
				result.Append(replacement);

				posCurrent = idxNext + lenPattern;

				idxNext = original.IndexOf(pattern, posCurrent, comparisonType);
			}

			result.Append(original, posCurrent, original.Length - posCurrent);
			return result.ToString();
		}
	}
}
