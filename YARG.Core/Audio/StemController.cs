namespace YARG.Gameplay
{
    public interface StemController
    {
        /// <summary>
        /// Sets the mute state of the stem.
        /// </summary>
        /// <param name="muted">Whether the stem should be muted.</param>
        /// <param name="duration">The fade duration in milliseconds.</param>
        void SetMute(bool muted, float duration = 0.0f);

        /// <summary>
        /// Sets the reverb effect on the stem.
        /// </summary>
        /// <param name="reverb">Whether reverb should be enabled.</param>
        void SetReverb(bool reverb);

        /// <summary>
        /// Sets the whammy pitch bend percentage.
        /// </summary>
        /// <param name="percent">The whammy percentage (0-1 range).</param>
        void SetWhammyPercent(float percent);
    }
}