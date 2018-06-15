using PerformanceCounterHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.Perfomance
{
    [PerformanceCounterCategory("MvcMusicStore", System.Diagnostics.PerformanceCounterCategoryType.MultiInstance, "MvcMusicStore")]
    public enum Counters
    {
        [PerformanceCounter("Go Home counter", "Go Home", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        GoToHome,
        [PerformanceCounter("Successful Logout counter", "Logout counter", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogOut,
        [PerformanceCounter("Successful Login counter", "Login counter", System.Diagnostics.PerformanceCounterType.NumberOfItems32)]
        SuccessfulLogIn,
    }
}