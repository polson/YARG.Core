using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using YARG.Gameplay;

namespace YARG.Core.Audio
{
    public abstract class DrumSampleChannel : IDisposable
    {
        public const int ROUND_ROBIN_MAX_INDEX = 3;
        private bool _disposed;

        protected double _volume;

        public readonly DrumSfxSample Sample;
        protected DrumSampleChannel(DrumSfxSample sample)
        {
            Sample = sample;
        }

        public void Play(double volume)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    _volume = volume;
                    Play_Internal(volume);
                }
            }
        }

        private void SetVolume(double volume)
        {
            lock (this)
            {
                if (!_disposed)
                {
                    volume *= _volume;
                    SetVolume_Internal(volume);
                }
            }
        }

        protected abstract void Play_Internal(double volume);
        protected abstract void SetVolume_Internal(double volume);

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

        ~DrumSampleChannel()
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
