using System;

namespace YARG.Core.Audio
{
    /// <summary>
    /// A sample channel that uses BASS to play VOX files.
    ///
    /// Unlike all the others, this one will automatically queue samples and play them sequentially,
    /// so don't be surprised when you can't play overlapping vox clips
    /// </summary>
    public abstract class VoxSampleChannel : IDisposable
    {
        private bool _disposed;

        public readonly  VoxSample Sample;
        private readonly string    _path;

        protected VoxSampleChannel(VoxSample sample)
        {
            Sample = sample;
        }

        public void Play()
        {
            lock (this)
            {
                if (!_disposed)
                {
                    Play_Internal();
                }
            }
        }

        private void SetVolume(double volume)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    SetVolume_Internal(volume);
                }
            }
        }

        public bool IsPlaying()
        {
            lock (this)
            {
                if (!_disposed)
                {
                    return IsPlaying_Internal();
                }
            }
            return false;
        }

        protected abstract void Play_Internal();
        protected abstract void SetVolume_Internal(double volume);
        protected abstract bool IsPlaying_Internal();

        protected virtual void DisposeManagedResources() { }
        protected virtual void DisposeUnmanagedResources() { }

        private void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        DisposeManagedResources();
                    }
                    DisposeUnmanagedResources();
                    _disposed = true;
                }
            }
        }

        ~VoxSampleChannel()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
