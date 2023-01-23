namespace PluginLoader
{
    public interface ICancellable
    {
        public bool IsCancel()
        {
            return IsCanceled;
        }
        public void Cancel()
        {
            IsCanceled = true;
        }
        public void SetCancel(bool IsCancel)
        {
            IsCanceled = IsCancel;
        }
        public bool IsCanceled { get; set; }
    }
}
