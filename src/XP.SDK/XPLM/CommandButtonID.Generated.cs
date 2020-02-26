using System;

namespace XP.SDK.XPLM
{
    
    /// <summary>
    /// <para>
    /// These are enumerations for all of the things you can do with a joystick
    /// button in X-Plane. They currently match the buttons menu in the equipment
    /// setup dialog, but these enums will be stable even if they change in
    /// X-Plane.
    /// </para>
    /// </summary>
    public enum CommandButtonID
    {
        Nothing = 0,
        StartAll = 1,
        Start0 = 2,
        Start1 = 3,
        Start2 = 4,
        Start3 = 5,
        Start4 = 6,
        Start5 = 7,
        Start6 = 8,
        Start7 = 9,
        ThrotUp = 10,
        ThrotDn = 11,
        PropUp = 12,
        PropDn = 13,
        MixtUp = 14,
        MixtDn = 15,
        CarbTog = 16,
        CarbOn = 17,
        CarbOff = 18,
        Trev = 19,
        TrmUp = 20,
        TrmDn = 21,
        RotTrmUp = 22,
        RotTrmDn = 23,
        RudLft = 24,
        RudCntr = 25,
        RudRgt = 26,
        AilLft = 27,
        AilCntr = 28,
        AilRgt = 29,
        BRudLft = 30,
        BRudRgt = 31,
        LookUp = 32,
        LookDn = 33,
        LookLft = 34,
        LookRgt = 35,
        GlanceL = 36,
        GlanceR = 37,
        VFnh = 38,
        VFwh = 39,
        VTra = 40,
        VTwr = 41,
        VRun = 42,
        VCha = 43,
        VFr1 = 44,
        VFr2 = 45,
        VSpo = 46,
        Flapsup = 47,
        Flapsdn = 48,
        Vctswpfwd = 49,
        Vctswpaft = 50,
        GearTog = 51,
        GearUp = 52,
        GearDown = 53,
        LftBrake = 54,
        RgtBrake = 55,
        BrakesREG = 56,
        BrakesMAX = 57,
        Speedbrake = 58,
        OttDis = 59,
        OttAtr = 60,
        OttAsi = 61,
        OttHdg = 62,
        OttAlt = 63,
        OttVvi = 64,
        TimStart = 65,
        TimReset = 66,
        EcamUp = 67,
        EcamDn = 68,
        Fadec = 69,
        YawDamp = 70,
        ArtStab = 71,
        Chute = 72,
        JATO = 73,
        Arrest = 74,
        Jettison = 75,
        FuelDump = 76,
        Puffsmoke = 77,
        Prerotate = 78,
        ULPrerot = 79,
        ULCollec = 80,
        TOGA = 81,
        Shutdown = 82,
        ConAtc = 83,
        FailNow = 84,
        Pause = 85,
        RockUp = 86,
        RockDn = 87,
        RockLft = 88,
        RockRgt = 89,
        RockFor = 90,
        RockAft = 91,
        IdleHilo = 92,
        Lanlights = 93,
        Max = 94
    }
}