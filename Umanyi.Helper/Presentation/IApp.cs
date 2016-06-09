using Helper.Models;

namespace Helper.Presentation
{
    public interface IApp
    {
        ApplicationModel AppInfo
        { get; }

        ExamSettingsModel ExamSettings
        { get; }
    }
}
