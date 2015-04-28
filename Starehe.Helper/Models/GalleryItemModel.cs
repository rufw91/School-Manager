
namespace Helper.Models
{
    public class GalleryItemModel: ModelBase
    {
        private string path;
        private long size;
        private byte[] thumbnail;
        private int galleryID;
        private byte[] data;
        private string name;

        public GalleryItemModel()
        {
            GalleryID = 0;
            Path = path;
            Thumbnail = null;
            Size = 0;
            Data = null;
        }
        
        public int GalleryID
        {
            get { return galleryID; }
            set
            {
                if (galleryID != value)
                {
                    galleryID = value;
                    NotifyPropertyChanged("GalleryID");
                }
            }
        }

        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        public byte[] Data
        {
            get { return data; }
            set
            {
                if (data != value)
                {
                    data = value;
                    NotifyPropertyChanged("Data");
                }
            }
        }

        public string Path
        {
            get { return path; }

            set
            {
                if (value != path)
                {
                    path = value;
                    NotifyPropertyChanged("Path");
                }
            }
        }

        public long Size
        {
            get { return size; }

            set
            {
                if (value != size)
                {
                    size = value;
                    NotifyPropertyChanged("Size");
                }
            }
        }

        public byte[] Thumbnail
        {
            get { return thumbnail; }

            set
            {
                if (value != thumbnail)
                {
                    thumbnail = value;
                    NotifyPropertyChanged("Thumbnail");
                }
            }
        }

        

        public override void Reset()
        {
            Name = "";
            GalleryID = 0;
            Path = path;
            Thumbnail = null;
            Size = 0;
            Data = null;
        }
    }
}
