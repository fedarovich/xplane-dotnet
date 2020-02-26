using System;

namespace XP.SDK.Widgets
{
    
    /// <summary>
    /// <para>
    /// Elements are individually drawable UI things like push buttons, etc. The
    /// style defines what kind of element you are drawing. Elements can be
    /// stretched in one or two dimensions (depending on the element). Some
    /// elements can be lit.
    /// </para>
    /// <para>
    /// In X-Plane 6 some elements must be drawn over metal. Some are scalable and
    /// some are not. Any element can be drawn anywhere in X-Plane 7.
    /// </para>
    /// <para>
    /// Scalable Axis Required Background
    /// </para>
    /// </summary>
    public enum ElementStyle
    {
        TextField = 6,
        CheckBox = 9,
        CheckBoxLit = 10,
        WindowCloseBox = 14,
        WindowCloseBoxPressed = 15,
        PushButton = 16,
        PushButtonLit = 17,
        OilPlatform = 24,
        OilPlatformSmall = 25,
        Ship = 26,
        ILSGlideScope = 27,
        MarkerLeft = 28,
        Airport = 29,
        Waypoint = 30,
        NDB = 31,
        VOR = 32,
        RadioTower = 33,
        AircraftCarrier = 34,
        Fire = 35,
        MarkerRight = 36,
        CustomObject = 37,
        CoolingTower = 38,
        SmokeStack = 39,
        Building = 40,
        PowerLine = 41,
        CopyButtons = 45,
        CopyButtonsWithEditingGrid = 46,
        EditingGrid = 47,
        ScrollBar = 48,
        VORWithCompassRose = 49,
        Zoomer = 51,
        TextFieldMiddle = 52,
        LittleDownArrow = 53,
        LittleUpArrow = 54,
        WindowDragBar = 61,
        WindowDragBarSmooth = 62
    }
}