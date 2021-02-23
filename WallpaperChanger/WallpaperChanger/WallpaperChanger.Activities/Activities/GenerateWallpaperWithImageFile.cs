using System;
using System.Activities;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using WallpaperChanger.Activities.Properties;
using UiPath.Shared.Activities;
using UiPath.Shared.Activities.Localization;
using System.ComponentModel;
using UiPath.Shared.Activities.Utilities;
using System.IO;
using WallpaperChanger.Models;

namespace WallpaperChanger.Activities
{
    [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_DisplayName))]
    [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_Description))]
    public class GenerateWallpaperWithImageFile : ContinuableAsyncCodeActivity
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

        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_ImageFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_ImageFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> ImageFilePath { get; set; }

        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_Text_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_Text_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> Text { get; set; }

        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_FontSize_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_FontSize_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<int> FontSize { get; set; } = 48;

        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_FontName_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_FontName_Description))]
        [LocalizedCategory(nameof(Resources.Options_Category))]
        public InArgument<string> FontName { get; set; } = "Arial";

        [TypeConverter(typeof(EnumNameConverter<KnownColor>))]
        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_TextColor_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_TextColor_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public KnownColor TextColor { get; set; } = KnownColor.White;

        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_OutputFilePath_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_OutputFilePath_Description))]
        [LocalizedCategory(nameof(Resources.Input_Category))]
        public InArgument<string> OutputFilePath { get; set; }

        [LocalizedCategory(nameof(Resources.Output_Category))]
        [LocalizedDisplayName(nameof(Resources.GenerateWallpaperWithImageFile_Result_DisplayName))]
        [LocalizedDescription(nameof(Resources.GenerateWallpaperWithImageFile_Result_Description))]
        public OutArgument<bool> Result { get; set; }

        #endregion


        #region Constructors

        public GenerateWallpaperWithImageFile()
        {
        }

        #endregion


        #region Protected Methods

        protected override void CacheMetadata(CodeActivityMetadata metadata)
        {
            if (ImageFilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(ImageFilePath)));
            if (OutputFilePath == null) metadata.AddValidationError(string.Format(Resources.ValidationValue_Error, nameof(OutputFilePath)));

            base.CacheMetadata(metadata);
        }

        protected override async Task<Action<AsyncCodeActivityContext>> ExecuteAsync(AsyncCodeActivityContext context, CancellationToken cancellationToken)
        {
            // Inputs
            var timeout = TimeoutMS.Get(context);
            var imagefilepath = ImageFilePath.Get(context);
            var outputfilepath = OutputFilePath.Get(context);

            if (!File.Exists(imagefilepath))
            {
                throw new FileNotFoundException(Resources.GenerateWallpaperWithImageFile_ImageFilePath_DisplayName);
            }

            if (!Directory.Exists(Path.GetDirectoryName(imagefilepath)))
            {
                throw new ArgumentException(string.Format(Resources.ValidationValueFullPath_Error, Resources.GenerateWallpaperWithImageFile_ImageFilePath_DisplayName));
            }

            if (!Directory.Exists(Path.GetDirectoryName(outputfilepath)))
            {
                throw new ArgumentException(string.Format(Resources.ValidationValueFullPath_Error, Resources.GenerateWallpaperWithImageFile_OutputFilePath_DisplayName));
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
            var imagefilepath = ImageFilePath.Get(context);
            var outputfilepath = OutputFilePath.Get(context);
            var text = Text.Get(context);
            var fontsize = FontSize.Get(context);
            var fontname = FontName.Get(context);
            return await Task.FromResult(new WallpaperGenerater().GenerateWallPaperFromFile(imagefilepath,
                                                                                             TextColor,
                                                                                             text,
                                                                                             fontsize,
                                                                                             fontname,
                                                                                             outputfilepath));
        }

        #endregion
    }
}

