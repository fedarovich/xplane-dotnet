using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using XP.SDK.XPLM.Internal;

namespace XP.SDK.XPLM
{
    public sealed class Probe : IDisposable
    {
        private ProbeRef _ref;
        private int _disposed;

        public Probe(ProbeType probeType = ProbeType.XplmProbeY)
        {
            _ref = SceneryAPI.CreateProbe(probeType);
        }

        public unsafe (ProbeResult result, ProbeInfo info) ProbeTerrain(float x, float y, float z)
        {
            var info = new ProbeInfo { structSize = Unsafe.SizeOf<ProbeInfo>() };
            var result = SceneryAPI.ProbeTerrainXYZ(_ref, x, y, x, &info);
            return (result, info);
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
