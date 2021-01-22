using System;
using System.Runtime.CompilerServices;
using System.Threading;
using XP.SDK.XPLM.Interop;

namespace XP.SDK.XPLM
{
    /// <summary>
    /// <para>
    /// Allows you to locate the physical scenery mesh.
    /// This would be used to place dynamic graphics on top of the ground
    /// in a plausible way or do physics interactions.
    /// </para>
    /// <para>
    ///  Probe objects exist both to capture which algorithm you have requested
    /// (see <see cref="ProbeType"/>) and also to cache query information.
    /// </para>
    /// </summary>
    /// <remarks>
    /// <para>
    /// It is generally faster to use the same probe for nearby points  and different probes for different points.
    /// Try not to allocate more than “hundreds” of probes at most. Share probes if you need more.
    /// Generally, probing operations are expensive, and should be avoided via caching when possible.
    /// </para>
    /// <para>
    /// Y testing returns a location on the terrain, a normal vector, and a velocity vector.
    /// The normal vector tells you the slope of the terrain at that point.
    /// The velocity vector tells you if that terrain is moving (and is in meters/second).
    /// For example, if your Y test hits the aircraft carrier deck,
    /// this tells you the velocity of that point on the deck.
    /// </para>
    /// <para>
    /// Note: the Y-testing API is limited to probing the loaded scenery area,
    /// which is approximately 300x300 km in X-Plane 9.
    /// Probes outside this area will return the height of a 0 MSL sphere.
    /// </para>
    /// </remarks>
    public sealed class Probe : IDisposable
    {
        private ProbeRef _ref;
        private int _disposed;

        /// <summary>
        /// Initializes a new instance of <see cref="Probe" />.
        /// </summary>
        /// <param name="probeType"></param>
        public Probe(ProbeType probeType = ProbeType.XplmProbeY)
        {
            _ref = SceneryAPI.CreateProbe(probeType);
        }

        /// <summary>
        /// Probes the terrain.
        /// </summary>
        public unsafe ProbeResult ProbeTerrain(float x, float y, float z, out ProbeInfo probeInfo)
        {
            probeInfo = new ProbeInfo { structSize = Unsafe.SizeOf<ProbeInfo>() };
            fixed (ProbeInfo* pInfo = &probeInfo)
            {
                return SceneryAPI.ProbeTerrainXYZ(_ref, x, y, z, pInfo);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Interlocked.CompareExchange(ref _disposed, 1, 0) == 0)
            {
                SceneryAPI.DestroyProbe(_ref);
                _ref = default;
            }
        }
    }
}
