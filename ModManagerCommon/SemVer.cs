using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace ModManagerCommon
{
	/// <summary>
	/// SemVer 2.0.0-compliant class for version comparison.
	/// https://semver.org/
	/// </summary>
	public class SemVer : IComparable<SemVer>, IEquatable<SemVer>
	{
		static readonly Regex regex = new Regex(@"^(?<major>0|[1-9]\d*)\.(?<minor>0|[1-9]\d*)\.(?<patch>0|[1-9]\d*)(?:-(?<prerelease>(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*)(?:\.(?:0|[1-9]\d*|\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?(?:\+(?<buildmetadata>[0-9a-zA-Z-]+(?:\.[0-9a-zA-Z-]+)*))?$");

		public readonly long   Major;
		public readonly long   Minor;
		public readonly long   Patch;
		public readonly string PreRelease;
		public readonly string BuildMetaData;

		public SemVer(string versionString)
		{
			Match match = regex.Match(versionString);
			Trace.Assert(match.Success);

			Major         = long.Parse(match.Groups["major"].Value);
			Minor         = long.Parse(match.Groups["minor"].Value);
			Patch         = long.Parse(match.Groups["patch"].Value);
			PreRelease    = match.Groups["prerelease"].Value;
			BuildMetaData = match.Groups["buildmetadata"].Value;
		}

		public override string ToString()
		{
			var sb = new StringBuilder();

			sb.Append($"{Major}.{Minor}.{Patch}");

			if (!string.IsNullOrEmpty(PreRelease))
			{
				sb.Append($"-{PreRelease}");
			}

			if (!string.IsNullOrEmpty(BuildMetaData))
			{
				sb.Append($"+{BuildMetaData}");
			}

			return sb.ToString();
		}

		/// <inheritdoc />
		public int CompareTo(SemVer other)
		{
			int comparison = Major.CompareTo(other.Major);

			if (comparison != 0)
			{
				return comparison;
			}

			comparison = Minor.CompareTo(other.Minor);

			if (comparison != 0)
			{
				return comparison;
			}

			comparison = Patch.CompareTo(other.Patch);

			if (comparison != 0)
			{
				return comparison;
			}

			bool thisPreReleaseNull = string.IsNullOrEmpty(PreRelease);
			bool thatPreReleaseNull = string.IsNullOrEmpty(other.PreRelease);

			if (thisPreReleaseNull && !thatPreReleaseNull)
			{
				return -1;
			}

			if (thatPreReleaseNull && !thisPreReleaseNull)
			{
				return 1;
			}

			return string.Compare(PreRelease, other.PreRelease, StringComparison.Ordinal);
		}
		
		/// <inheritdoc />
		public bool Equals(SemVer other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Major == other.Major &&
			       Minor == other.Minor &&
			       Patch == other.Patch &&
			       PreRelease == other.PreRelease &&
			       BuildMetaData == other.BuildMetaData;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = Major.GetHashCode();
				hashCode = (hashCode * 397) ^ Minor.GetHashCode();
				hashCode = (hashCode * 397) ^ Patch.GetHashCode();
				hashCode = (hashCode * 397) ^ PreRelease.GetHashCode();
				hashCode = (hashCode * 397) ^ BuildMetaData.GetHashCode();
				return hashCode;
			}
		}
	}

	public class SemVerComparer : IComparer<SemVer>
	{
		/// <inheritdoc />
		public int Compare(SemVer x, SemVer y)
		{
			switch (x)
			{
				case null when y is null:
					return 0;
				case null:
					return 1;
			}

			if (y is null)
			{
				return -1;
			}

			if (x.Major == y.Major &&
			    x.Minor == y.Minor &&
			    x.Patch == y.Patch &&
			    x.PreRelease == y.PreRelease)
			{
				return 0;
			}

			return x.CompareTo(y);
		}
	}
}