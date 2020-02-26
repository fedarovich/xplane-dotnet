using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// These enums define what language the sim is running in. These enumerations
    /// do not imply that the sim can or does run in all of these languages; they
    /// simply provide a known encoding in the event that a given sim version is
    /// localized to a certain language.
    /// </para>
    /// </summary>
    public enum LanguageCode
    {
        Unknown = 0,
        English = 1,
        French = 2,
        German = 3,
        Italian = 4,
        Spanish = 5,
        Korean = 6,
        Russian = 7,
        Greek = 8,
        Japanese = 9,
        Chinese = 10
    }
}