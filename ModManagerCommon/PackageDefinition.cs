using System;
using System.Collections.Generic;

namespace ModManagerCommon
{
	public class PackageBase : IEquatable<PackageBase>, IComparable<PackageBase>
	{
		/// <summary>
		/// The name of the package.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// The version of the package.
		/// </summary>
		public readonly SemVer Version;

		public PackageBase(string name, SemVer version)
		{
			Name    = name;
			Version = version;
		}

		/// <inheritdoc />
		public bool Equals(PackageBase other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return Name == other.Name && Version.Equals(other.Version);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((PackageBase)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return (Name.GetHashCode() * 397) ^ Version.GetHashCode();
			}
		}

		/// <inheritdoc />
		public int CompareTo(PackageBase other)
		{
			if (ReferenceEquals(this, other))
			{
				return 0;
			}

			if (other is null)
			{
				return 1;
			}

			int nameComparison = string.Compare(Name, other.Name, StringComparison.Ordinal);

			if (nameComparison != 0)
			{
				return nameComparison;
			}

			return Comparer<SemVer>.Default.Compare(Version, other.Version);
		}
	}

	public class PackageDefinition : PackageBase, IEquatable<PackageDefinition>
	{
		/// <summary>
		/// The authors that contributed to this package.
		/// </summary>
		public readonly string[] Authors;

		/// <summary>
		/// Packages which this package depends on.
		/// The key is the package name, and the value is the package version.
		///
		/// Package versions starting with '~' indicate that the version must be
		/// >= the specified version.
		///
		/// Otherwise, the package version is treated as an exact match.
		/// </summary>
		public readonly Dictionary<string, string> Dependencies;

		public PackageDefinition(string name, SemVer version, string[] authors = null, Dictionary<string, string> dependencies = null)
			: base(name, version)
		{
			Authors      = authors ?? new string[0];
			Dependencies = dependencies ?? new Dictionary<string, string>();
		}

		/// <inheritdoc />
		public bool Equals(PackageDefinition other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return base.Equals(other) && Equals(Authors, other.Authors) && Equals(Dependencies, other.Dependencies);
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((PackageDefinition)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				int hashCode = base.GetHashCode();
				hashCode = (hashCode * 397) ^ (Authors != null ? Authors.GetHashCode() : 0);
				hashCode = (hashCode * 397) ^ (Dependencies != null ? Dependencies.GetHashCode() : 0);
				return hashCode;
			}
		}
	}

	public class InstalledPackage : PackageBase, IEquatable<InstalledPackage>
	{
		/// <summary>
		/// Indicates that this package is installed exclusively as a dependency.
		/// </summary>
		public readonly bool AsDependency;

		public InstalledPackage(string name, SemVer version, bool asDependency)
			: base(name, version)
		{
			AsDependency = asDependency;
		}

		/// <inheritdoc />
		public bool Equals(InstalledPackage other)
		{
			if (other is null)
			{
				return false;
			}

			if (ReferenceEquals(this, other))
			{
				return true;
			}

			return base.Equals(other) && AsDependency == other.AsDependency;
		}

		/// <inheritdoc />
		public override bool Equals(object obj)
		{
			if (obj is null)
			{
				return false;
			}

			if (ReferenceEquals(this, obj))
			{
				return true;
			}

			if (obj.GetType() != GetType())
			{
				return false;
			}

			return Equals((InstalledPackage)obj);
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				return (base.GetHashCode() * 397) ^ AsDependency.GetHashCode();
			}
		}
	}
}
