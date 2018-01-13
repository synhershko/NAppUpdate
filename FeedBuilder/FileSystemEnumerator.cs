using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace FeedBuilder
{
	/// <summary>
	///   File system enumerator.  This class provides an easy to use, efficient mechanism for searching a list of
	///   directories for files matching a list of file specifications.  The search is done incrementally as matches
	///   are consumed, so the overhead before processing the first match is always kept to a minimum.
	/// </summary>
	public sealed class FileSystemEnumerator
	{
		/// <summary>
		///   Array of paths to be searched.
		/// </summary>
		private readonly string[] m_paths;

		/// <summary>
		///   Array of regular expressions that will detect matching files.
		/// </summary>
		private readonly List<Regex> m_fileSpecs;

		/// <summary>
		///   If true, sub-directories are searched.
		/// </summary>
		private readonly bool m_includeSubDirs;

		/// <summary>
		///   Constructor.
		/// </summary>
		/// <param name="pathsToSearch"> Semicolon- or comma-delimitted list of paths to search. </param>
		/// <param name="fileTypesToMatch"> Semicolon- or comma-delimitted list of wildcard filespecs to match. </param>
		/// <param name="includeSubDirs"> If true, subdirectories are searched. </param>
		public FileSystemEnumerator(string pathsToSearch, string fileTypesToMatch, bool includeSubDirs)
		{
			// check for nulls
			if (null == pathsToSearch) throw new ArgumentNullException("pathsToSearch");
			if (null == fileTypesToMatch) throw new ArgumentNullException("fileTypesToMatch");

			// make sure spec doesn't contain invalid characters
			if (fileTypesToMatch.IndexOfAny(new[] { ':', '<', '>', '/', '\\' }) >= 0) throw new ArgumentException("Invalid characters in wildcard pattern", "fileTypesToMatch");

			m_includeSubDirs = includeSubDirs;
			m_paths = pathsToSearch.Split(new[] { ';', ',' });

			string[] specs = fileTypesToMatch.Split(new[] { ';', ',' });
			m_fileSpecs = new List<Regex>(specs.Length);
			foreach (string spec in specs)
			{
				// trim whitespace off file spec and convert Win32 wildcards to regular expressions
				string pattern = spec.Trim().Replace(".", @"\.").Replace("*", @".*").Replace("?", @".?");
				m_fileSpecs.Add(new Regex("^" + pattern + "$", RegexOptions.IgnoreCase));
			}
		}

		private IEnumerable<FileInfo> ProcessFiles(string folderPath)
		{
			foreach (var file in Directory.GetFiles(folderPath))
			{
				string fileName = Path.GetFileName(file);
				foreach (Regex fileSpec in m_fileSpecs)
				{
					// if this spec matches, return this file's info
					if (fileSpec.IsMatch(fileName))
					{
						yield return new FileInfo(file);
						break;
					}
				}
			}
		}

		void CheckSecurity(string folderPath)
		{
			new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Path.Combine(folderPath, ".")).Demand();
		}

		private IEnumerable<string> ProcessSubdirectories(string folderPath)
		{
			// check security - ensure that caller has rights to read this directory
			CheckSecurity(folderPath);
			foreach (var d in Directory.GetDirectories(folderPath))
			{
				new FileIOPermission(FileIOPermissionAccess.PathDiscovery, Path.Combine(d, ".")).Demand();
				yield return d;
				foreach (var sd in ProcessSubdirectories(d))
					yield return sd;
			}
		}

		/// <summary>
		///   Get an enumerator that returns all of the files that match the wildcards that
		///   are in any of the directories to be searched.
		/// </summary>
		/// <returns> An IEnumerable that returns all matching files one by one. </returns>
		/// <remarks>
		///   The enumerator that is returned finds files using a lazy algorithm that
		///   searches directories incrementally as matches are consumed.
		/// </remarks>
		public IEnumerable<FileInfo> Matches()
		{
			foreach (string rootPath in m_paths)
			{
				string path = rootPath.Trim();

				// check security - ensure that caller has rights to read this directory
				CheckSecurity(path);

				foreach (var fi in ProcessFiles(path))
					yield return fi;

				if (m_includeSubDirs)
				{

					foreach (var d in ProcessSubdirectories(path))
					{
						foreach (var fi in ProcessFiles(d))
							yield return fi;
					}
				}
			}
		}
	}
}

