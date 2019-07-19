﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace NittyGritty.Uwp.Operations
{
    public class FileOperation
    {
        private readonly Func<FileActivatedEventArgs, StorageFile, Frame, Task> callback;

        /// <summary>
        /// Creates a FileOperation to handle the file that activated the app
        /// </summary>
        /// <param name="fileType">The file type this FileOperation can handle. Cannot be null, empty, or whitespace</param>
        /// <param name="callback"></param>
        public FileOperation(string fileType, Func<FileActivatedEventArgs, StorageFile, Frame, Task> callback = null)
        {
            if(string.IsNullOrWhiteSpace(fileType))
            {
                throw new ArgumentException("File type cannot be null, empty, or whitespace.", nameof(fileType));
            }

            FileType = fileType;
            this.callback = callback;
        }

        public string FileType { get; }

        public virtual async Task Run(FileActivatedEventArgs args, StorageFile file, Frame frame)
        {
            await callback?.Invoke(args, file, frame);
        }
    }
}
