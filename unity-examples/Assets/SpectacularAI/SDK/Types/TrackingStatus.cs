namespace SpectacularAI
{
    /// <summary>
    /// 6-DoF pose tracking status
    /// </summary>
    public enum TrackingStatus
    {
        /** Initial status when tracking starts and is still initializing */
        INIT = 0,
        /** Tracking is accurate (but not globally referenced) */
        TRACKING = 1,
        /**
         * Tracking has failed. Outputs are no longer produced until
         * the system recovers, which will be reported as another tracking state
         */
        LOST_TRACKING = 2
    }
}