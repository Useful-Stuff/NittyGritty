﻿using NittyGritty.Extensions;
using NittyGritty.Uwp.Activation.Operations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;

namespace NittyGritty.Uwp.Activation
{
    public class BackgroundActivationHandler : ActivationHandler<BackgroundActivatedEventArgs>
    {
        private static Dictionary<string, BackgroundTask> backgroundTasks;

        static BackgroundActivationHandler()
        {
            backgroundTasks = new Dictionary<string, BackgroundTask>();
        }

        public BackgroundActivationHandler() : base(ActivationStrategy.Background)
        {
            
        }

        public static ReadOnlyDictionary<string, BackgroundTask> BackgroundTasks
        {
            get { return new ReadOnlyDictionary<string, BackgroundTask>(backgroundTasks); }
        }

        public static BackgroundTaskRegistration GetBackgroundTaskRegistration(string taskName)
        {
            if (!BackgroundTaskRegistration.AllTasks.Any(t => t.Value.Name == taskName))
            {
                // This condition should not be met. If it is, it means the background task was not registered correctly.
                // Please check CreateInstances to see if the background task was properly added to the BackgroundTasks property.
                return null;
            }

            return (BackgroundTaskRegistration)BackgroundTaskRegistration.AllTasks.FirstOrDefault(t => t.Value.Name == taskName).Value;
        }

        public static void AddTask(BackgroundTask task)
        {
            if(!backgroundTasks.TryAdd(task.Name, task))
            {
                throw new ArgumentException("You only have to register for a background task once");
            }
        }

        public static async Task Register()
        {
            BackgroundExecutionManager.RemoveAccess();
            var result = await BackgroundExecutionManager.RequestAccessAsync();

            if (result == BackgroundAccessStatus.DeniedBySystemPolicy
                || result == BackgroundAccessStatus.DeniedByUser)
            {
                return;
            }

            foreach (var task in backgroundTasks)
            {
                task.Value.Register(
                    new BackgroundTaskBuilder()
                    {
                        Name = task.Value.Name
                    });
            }
        }

        protected override async Task HandleInternal(BackgroundActivatedEventArgs args)
        {
            Start(args.TaskInstance);
            await Task.CompletedTask;
        }

        private void Start(IBackgroundTaskInstance taskInstance)
        {
            if(backgroundTasks.TryGetValue(taskInstance?.Task?.Name ?? string.Empty, out var task))
            {
                task.Run(taskInstance).FireAndForget();
            }
            else
            {
                // This condition should not be met. If it is, it means the background task to start was not found in the background tasks managed by this service.
                // Please check AddTask to see if the background task was properly added to the BackgroundTasks property.
                return;
            }
        }

    }
}
