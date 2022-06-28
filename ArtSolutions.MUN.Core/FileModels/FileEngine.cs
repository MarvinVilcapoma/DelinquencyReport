using System.Linq;

namespace ArtSolutions.MUN.Core
{
    public class FileEngine
    {
        public MUNImagesGet_Result Get(int id)
        {
            using (FileDataModelContainer context = new Core.FileDataModelContainer())
            {
                return context.MUNImagesGet(id).FirstOrDefault();
            }
        }
        public int Insert(FileModel item)
        {
            using (FileDataModelContainer context = new FileDataModelContainer())
            {
                MUNImagesInsert_Result fileItem = context.MUNImagesInsert(item.CompanyID,
                                                                                                item.Data,
                                                                                                item.Length,
                                                                                                item.ContentType,
                                                                                                item.FileName,
                                                                                                item.FileExtension,
                                                                                                item.IsListedInFolders,
                                                                                                item.FolderID,
                                                                                                item.IsPublicImage,
                                                                                                false,
                                                                                                true,
                                                                                                item.Sort,
                                                                                                item.ImageID,
                                                                                                item.CreatedUserID,
                                                                                                System.DateTime.UtcNow,
                                                                                                item.ModifiedUserID,
                                                                                                System.DateTime.UtcNow).FirstOrDefault();
                return fileItem.ID;
            }
        }
        public int Update(FileModel item)
        {
            using (FileDataModelContainer context = new FileDataModelContainer())
            {
                MUNImagesUpdate_Result fileItem = context.MUNImagesUpdate(item.ID,
                                                                                                item.CompanyID,
                                                                                                item.Data,
                                                                                                item.Length,
                                                                                                item.ContentType,
                                                                                                 item.FileName,
                                                                                                 item.FileExtension,
                                                                                                 item.IsListedInFolders,
                                                                                                 item.FolderID,
                                                                                                 item.IsPublicImage,
                                                                                                 false,
                                                                                                 true,
                                                                                                 item.Sort,
                                                                                                 item.ImageID,
                                                                                                 item.CreatedUserID,
                                                                                                 System.DateTime.UtcNow).FirstOrDefault();
                return fileItem.ID;
            }
        }
    }
}
