using Helper;
using System.Collections.ObjectModel;
using System.Security.Permissions;

namespace UmanyiSMS.ViewModels
{
    [PrincipalPermission(SecurityAction.Demand, Role = "User")]
    public class GalleryVM:ParentViewModel
    {
        public GalleryVM()
        {
            Title = "GALLERY & EVENTS";

            TryAddChild(typeof(NewGalleryItemVM));
            TryAddChild(typeof(RecentGalleryItemsVM));
            TryAddChild(typeof(AllGalleryItemsVM));
            TryAddChild(typeof(NewEventVM));
            TryAddChild(typeof(UpcomingEventsVM));
            TryAddChild(typeof(AllEventsVM));
        }
    }
}
