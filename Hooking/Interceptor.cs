namespace EChat.Hooking
{
    using System;
    using System.Reflection;

    public class Interceptor : IDisposable
    {
        private readonly object _syncobj = new object();
        private Hooker _hooking = null;
        private bool _disposed = false;

        public delegate void CriticalHandler();

        private class NativeInterceptor : Hooker
        {
            protected override bool AdjustProtectMemoryPermissions(IntPtr address)
            {
                if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                {
                    return true;
                }
                return base.AdjustProtectMemoryPermissions(address);
            }
        }

        public Interceptor(MethodBase sources, MethodBase destination)
        {
            this.Source = sources ?? throw new ArgumentNullException(nameof(sources));
            this.Destination = destination ?? throw new ArgumentNullException(nameof(destination));
            this._hooking = new NativeInterceptor();
            this._hooking.Install(_hooking.GetProcAddress(sources), _hooking.GetProcAddress(destination));
        }

        ~Interceptor()
        {
            this.Dispose();
        }

        public virtual object SynchronizingObject { get => this._syncobj; }

        public virtual MethodBase Source { get; private set; }

        public virtual MethodBase Destination { get; private set; }

        public virtual bool Suspend()
        {
            lock (this._syncobj)
            {
                if (this._disposed)
                {
                    return false;
                }
                this._hooking.Suspend();
                return true;
            }
        }

        public virtual bool Resume()
        {
            lock (_syncobj)
            {
                if (_disposed)
                {
                    return false;
                }
                _hooking.Resume();
                return true;
            }
        }

        public virtual void Execute(CriticalHandler critical)
        {
            Exception exception = this.Invoke(critical);
            if (exception != null)
            {
                throw exception;
            }
        }

        public virtual Exception Invoke(CriticalHandler critical)
        {
            Exception exception = null;
            if (critical != null)
            {
                lock (this._syncobj)
                {
                    if (this._disposed)
                    {
                        exception = new ObjectDisposedException(typeof(Interceptor).FullName);
                    }
                    else
                    {
                        this.Suspend();
                        try
                        {
                            critical();
                        }
                        catch (Exception e)
                        {
                            exception = e;
                        }
                        finally
                        {
                            this.Resume();
                        }
                    }
                }
            }
            return exception;
        }

        public virtual void Dispose()
        {
            lock (this._syncobj)
            {
                if (!this._disposed)
                {
                    this._hooking.Uninstall();
                    this._disposed = true;
                }
            }
            GC.SuppressFinalize(this);
        }
    }
}
