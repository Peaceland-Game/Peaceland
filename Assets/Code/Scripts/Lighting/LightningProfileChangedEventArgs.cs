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