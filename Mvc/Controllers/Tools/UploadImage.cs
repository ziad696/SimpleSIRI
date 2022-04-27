using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using Telerik.Sitefinity.Workflow;
using Telerik.Sitefinity.Libraries.Model;
using System.IO;
using Telerik.Sitefinity.Modules.Libraries;
using Telerik.Sitefinity.GenericContent.Model;

namespace SitefinityWebApp.MVC.Controllers.Tools
{
    public class UploadImage
    {
        // NOTE: The imageExtension parameter must contain '.', for example '.jpeg'
        public static Guid? CreateImageWithNativeAPI(string albumUrl, string imageTitle, Stream imageStream, string imageFileName, string imageExtension)
        {
            try
            {
                Guid masterImageId = Guid.NewGuid();
                LibrariesManager librariesManager = LibrariesManager.GetManager();
                Image image = librariesManager.GetImages().Where(i => i.Id == masterImageId).FirstOrDefault();

                if (image == null)
                {
                    //The album post is created as master. The masterImageId is assigned to the master version.
                    image = librariesManager.CreateImage(masterImageId);

                    //Set the parent album.
                    Album album = librariesManager.GetAlbums().Where(i => i.UrlName == albumUrl && i.Status == ContentLifecycleStatus.Master).FirstOrDefault();
                    image.Parent = album;

                    //Set the properties of the album post.
                    image.Title = imageTitle;
                    image.DateCreated = DateTime.UtcNow;
                    image.PublicationDate = DateTime.UtcNow;
                    image.LastModified = DateTime.UtcNow;
                    image.UrlName = Regex.Replace(imageTitle.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");
                    image.MediaFileUrlName = Regex.Replace(imageFileName.ToLower(), @"[^\w\-\!\$\'\(\)\=\@\d_]+", "-");

                    //Upload the image file.
                    // The imageExtension parameter must contain '.', for example '.jpeg'
                    librariesManager.Upload(image, imageStream, imageExtension);

                    //Save the changes.
                    librariesManager.SaveChanges();

                    //Publish the Albums item. The live version acquires new ID.
                    var bag = new Dictionary<string, string>();
                    bag.Add("ContentType", typeof(Image).FullName);
                    WorkflowManager.MessageWorkflow(masterImageId, typeof(Image), null, "Publish", false, bag);
                }

                return image.Id;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}