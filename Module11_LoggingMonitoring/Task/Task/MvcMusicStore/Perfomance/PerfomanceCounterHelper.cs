using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Perfomance
{
    public class PerfomanceCounterHelper
    {
        private static readonly Lazy<PerfomanceCounterHelper> lazy =
            new Lazy<PerfomanceCounterHelper>(() => new PerfomanceCounterHelper());

        private CounterHelper<Counters> CounterHelper;

        public static PerfomanceCounterHelper Instance => lazy.Value;

        private PerfomanceCounterHelper()
        {
            CounterHelper = PerformanceHelper.CreateCounterHelper<Counters>("CounterHelper");
        }

        public void Increment(Counters counter)
        {
            CounterHelper.Increment(counter);
        }
    }
}