/// <summary>
/// Custom event for raising when the lighting changes in the scene
/// </summary>
public class LightingProfileChangedEventArgs : System.EventArgs
{
    public LightingProfile OldProfile { get; private set; }
    public LightingProfile NewProfile { get; private set; }
    public int NewProfileIndex { get; private set; }

    public LightingProfileChangedEventArgs(LightingProfile oldProfile, LightingProfile newProfile, int newProfileIndex)
    {
        OldProfile = oldProfile;
        NewProfile = newProfile;
        NewProfileIndex = newProfileIndex;
    }
}