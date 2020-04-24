using System.Activities.Presentation.Metadata;
using System.ComponentModel;
using System.ComponentModel.Design;
using myoshidan.WallpaperChanger.Activities.Design.Designers;
using myoshidan.WallpaperChanger.Activities.Design.Properties;

namespace myoshidan.WallpaperChanger.Activities.Design
{
    public class DesignerMetadata : IRegisterMetadata
    {
        public void Register()
        {
            var builder = new AttributeTableBuilder();
            builder.ValidateTable();

            var categoryAttribute = new CategoryAttribute($"{Resources.Category}");

            builder.AddCustomAttributes(typeof(ChangeWallpaper), categoryAttribute);
            builder.AddCustomAttributes(typeof(ChangeWallpaper), new DesignerAttribute(typeof(ChangeWallpaperDesigner)));
            builder.AddCustomAttributes(typeof(ChangeWallpaper), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(GenerateWallpaper), categoryAttribute);
            builder.AddCustomAttributes(typeof(GenerateWallpaper), new DesignerAttribute(typeof(GenerateWallpaperDesigner)));
            builder.AddCustomAttributes(typeof(GenerateWallpaper), new HelpKeywordAttribute(""));

            builder.AddCustomAttributes(typeof(GenerateWallpaperWithImageFile), categoryAttribute);
            builder.AddCustomAttributes(typeof(GenerateWallpaperWithImageFile), new DesignerAttribute(typeof(GenerateWallpaperWithImageFileDesigner)));
            builder.AddCustomAttributes(typeof(GenerateWallpaperWithImageFile), new HelpKeywordAttribute(""));

            MetadataStore.AddAttributeTable(builder.CreateTable());
        }
    }
}
