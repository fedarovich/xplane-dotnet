using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// XPLM Texture IDs name well-known textures in the sim for you to use. This
    /// allows you to recycle textures from X-Plane, saving VRAM.
    /// </para>
    /// <para>
    /// *Warning*: do not use these enums.  The only remaining use they have is to
    /// access the legacy compatibility v10 UI texture; if you need this, get it
    /// via the Widgets library.
    /// </para>
    /// </summary>
    public enum TextureID
    {
        XplmTexGeneralInterface = 0
    }
}