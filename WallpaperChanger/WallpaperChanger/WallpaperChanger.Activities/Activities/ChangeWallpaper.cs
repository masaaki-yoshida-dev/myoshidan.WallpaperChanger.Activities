using System;
using System.Activities;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WallpaperChanger.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;

namespace WallpaperChanger.Activities
{
    [LocalizedDisplayName(nameof(Resources.ChangeWallpaper_DisplayName))]
    [LocalizedDescription(nameof(Resources.ChangeWallpaper_Description))]
    public class ChangeWallpaper : ContinuableAsyncCodeActivity
    {
        #region Properties

        /// <summary>
        /// If set, continue executing the remaining activities even if the current activity has failed.
        /// </summary>
        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.ContinueOnError_DisplayName))]
        [LocalizedDescription(nameof(Resources.ContinueOnError_Description))]
        public override InArgument<bool> ContinueOnError { get; set; }

        [LocalizedCategory(nameof(Resources.Common_Category))]
        [LocalizedDisplayName(nameof(Resources.Timeout_DisplayName))]
        [LocalizedDescription(nameof(Resources.Timeout_Description))]
        public InArgument<int> TimeoutMS { get; set; } = 60000;

        [LocalizedDisplayName(nameof(Resources.ChangeWallpaper_ImageFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.ChangeWallpaper_ImageFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ImageFilePath { get; set; }

        [LocalizedCategory(nameof(Resources.Output_Category))]
        [LocalizedDisplayName(nameof(Resources.ChangeWallpaper_Result_DisplayName))]
        [LocalizedDescription(nameof(Resources.ChangeWallpaper_Result_Description))]
        public OutArgument<bool> Result { get; set; }

        #endregion


        #region Constructors

        public ChangeWallpaper()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (ImageFilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ImageFilePath)));
            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);
            var imageFilePath = ImageFilePath.Get(context);
            if (!File.Exists(imageFilePath))
            {
                throw new FileNotFoundException(imageFilePath);
            }

            if (!Directory.Exists(Path.GetDirectoryName(imageFilePath)))
            {
                throw new ArgumentException(string.Format(Resources.ValidationValueFullPath_Error, Resources.ChangeWallpaper_ImageFilePath_DisplayName));
            }

            // Set a timeout on the execution
            var task = ExecuteWithTimeout(context, cancellationToken);
            if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task) throw new TimeoutException(Resources.Timeout_Error);

            // Outputs
            return (ctx) => {
                Result.Set(ctx, task.Result);
            };
        }

        private async Task<bool> ExecuteWithTimeout(AsyncCodeActivityContext context, CancellationToken cancellationToken = default)
        {
            var imageFilePath = ImageFilePath.Get(context);
            return await Task.FromResult(new Models.WallpaperChanger().SetWallPaper(imageFilePath));            
        }

        #endregion
    }
}

