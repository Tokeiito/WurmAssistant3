﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace AldursLab.Essentials.Synchronization
{
    public sealed class FileLock : IDisposable
    {
        readonly FileStream stream;
        bool disposed;

        private FileLock(FileStream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// Locks exclusively on existing file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="LockFailedException">
        /// Lock could not be established by file system.
        /// Note that file/directory not existing does not fall under this exception.
        /// </exception>
        /// <exception cref="Exception">
        /// There was a lock-unrelated issue.
        /// </exception>
        public static FileLock Enter(string filePath)
        {
            try
            {
                var handle = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                WriteProcessInfoToFile(handle);
                return new FileLock(handle);
            }
            catch (IOException exception)
            {
                if (IsNonLockException(exception)) throw;
                throw CreateLockFailedException(exception);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        /// <exception cref="LockFailedException">File is already locked or lock could not be established by file system.</exception>
        public static FileLock EnterWithCreate(string filePath)
        {
            try
            {
                // create dir if not exists
                var dir = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrWhiteSpace(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                var handle = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                WriteProcessInfoToFile(handle);
                return new FileLock(handle);
            }
            catch (IOException exception)
            {
                if (IsNonLockException(exception)) throw;
                throw CreateLockFailedException(exception);
            }
        }

        static void WriteProcessInfoToFile(FileStream handle)
        {
            ClearStream(handle);
            var currentProcess = Process.GetCurrentProcess();
            var writer = new StreamWriter(handle);
            writer.Write($"This file is locked by Process Id {currentProcess.Id} named {currentProcess.ProcessName}");
            writer.Flush();
            handle.Flush(true);
        }

        static void ClearStream(FileStream handle)
        {
            handle.SetLength(0);
        }

        static bool IsNonLockException(Exception exception)
        {
            return exception is FileNotFoundException
                   || exception is DirectoryNotFoundException
                   || exception is PathTooLongException;
        }

        static LockFailedException CreateLockFailedException(Exception exception)
        {
            return new LockFailedException(
                string.Format(
                    "File is already locked or lock could not be established by file system: {0} See inner exception for details.",
                    exception.Message),
                exception);
        }

        public Stream Stream { get { return stream; } }

        public void Dispose()
        {
            Cleanup();
            GC.SuppressFinalize(this);
        }

        ~FileLock()
        {
            Cleanup();
        }

        void Cleanup()
        {
            try
            {
                if (!disposed)
                {
                    ClearStream(stream);
                    stream.Flush(true);
                }
                stream.Dispose();
                disposed = true;
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
            }
        }
    }

    [Serializable]
    public class LockFailedException : Exception
    {
        public LockFailedException()
        {
        }

        public LockFailedException(string message) : base(message)
        {
        }

        public LockFailedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected LockFailedException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}