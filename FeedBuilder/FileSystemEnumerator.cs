using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace FeedBuilder
{

    /// <summary>
    ///   File system enumerator.  This class provides an easy to use, efficient mechanism for searching a list of
    ///   directories for files matching a list of file specifications.  The search is done incrementally as matches
    ///   are consumed, so the overhead before processing the first match is always kept to a minimum.
    /// </summary>
    public sealed class FileSystemEnumerator : IDisposable
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
        ///   Array of regular expressions that will detect matching files to exclude.
        /// </summary>
        private readonly List<Regex> m_excludeFileSpecs;

        /// <summary>
        ///   If true, sub-directories are searched.
        /// </summary>
        private readonly bool m_includeSubDirs;

        #region IDisposable implementation

        /// <summary>
        ///   IDisposable.Dispose
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        /// <summary>
        ///   Constructor.
        /// </summary>
        /// <param name="pathsToSearch"> Semicolon- or comma-delimited list of paths to search. </param>
        /// <param name="fileTypesToMatch"> Semicolon- or comma-delimited list of wildcard filespecs to match. </param>
        /// <param name="includeSubDirs"> If true, subdirectories are searched. </param>
        public FileSystemEnumerator(string pathsToSearch, string fileTypesToMatch, string excludeFileTypesToMatch, bool includeSubDirs)
        {


            // check for nulls
            if (null == pathsToSearch) throw new ArgumentNullException("pathsToSearch");
            if (null == fileTypesToMatch) throw new ArgumentNullException("fileTypesToMatch");

            // make sure spec doesn't contain invalid characters
            if (fileTypesToMatch.IndexOfAny(new[] { ':', '<', '>', '/', '\\' }) >= 0) throw new ArgumentException("invalid characters in wildcard pattern", "fileTypesToMatch");

            m_includeSubDirs = includeSubDirs;
            m_paths = pathsToSearch.Split(new[] { ';', ',' });

            string[] specs = fileTypesToMatch.Split(new[] { ';', ',' });
            string[] exSpecs = string.IsNullOrEmpty(excludeFileTypesToMatch) ? new string[] { } : excludeFileTypesToMatch.Split(new[] { ';', ',' });
            m_fileSpecs = new List<Regex>(specs.Length);
            m_excludeFileSpecs = new List<Regex>(exSpecs.Length);
            foreach (string spec in specs)
            {
                // trim whitespace off file spec and convert Win32 wildcards to regular expressions
                string pattern = spec.Trim().Replace(".", @"\.").Replace("*", @".*").Replace("?", @".?");
                m_fileSpecs.Add(new Regex("^" + pattern + "$", RegexOptions.IgnoreCase));
            }

            foreach (string spec in exSpecs)
            {
                // trim whitespace off file spec and convert Win32 wildcards to regular expressions
                string pattern = spec.Trim().Replace(".", @"\.").Replace("*", @".*").Replace("?", @".?");
                m_excludeFileSpecs.Add(new Regex("^" + pattern + "$", RegexOptions.IgnoreCase));
            }
        }

        private IEnumerable<FileInfo> ProcessFiles(string folderPath)
        {
            foreach (var file in Directory.GetFiles(folderPath))
            {
                string tmpFile = Path.GetFileName(file);
                foreach (Regex fileSpec in m_fileSpecs)
                {
                    // if this spec matches, return this file's info
                    if (fileSpec.IsMatch(tmpFile))
                    {
                        bool showFile = true;
                        foreach (Regex exSpec in m_excludeFileSpecs)
                            if (exSpec.IsMatch(tmpFile))
                                showFile=false;
                        if (showFile)
                        {
                            yield return new FileInfo(file);
                            break;
                        }
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

        public event EventHandler<FileProcessedEventArgs> FileProcessed;

        // Invoke the Changed event; called whenever list changes:
        private void OnFileProcess(FileProcessedEventArgs e)
        {
            if (FileProcessed != null)
                FileProcessed(this, e);
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



        public Task<IEnumerable<FileInfoEx>> MatchesToFileInfoExAsync(int outputDirLength)
        {
            return Task.Run(() =>
           {
               int count = 0;
               var FileInfos = new List<FileInfoEx>();
               foreach (var m in Matches())
               {
                   FileInfos.Add(new FileInfoEx(m.FullName, outputDirLength));
                   count++;
                   OnFileProcess(new FileProcessedEventArgs(count));
               }

               return FileInfos
                .OrderBy(f => f.FileInfo.FullName).AsEnumerable();

           });
        }
    }
}
public class FileProcessedEventArgs : EventArgs
{
    public int FileProcesCount { get; private set; }

    public FileProcessedEventArgs(int FileProcesCount)
    {
        this.FileProcesCount = FileProcesCount;
    }
}