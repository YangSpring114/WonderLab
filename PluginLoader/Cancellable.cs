namespace PluginLoader
{
    public interface ICancellable
    {
        public bool IsCancel();
        public void Cancel();
        public void SetCancel(bool IsCancel);
        public bool IsCanceled { get; set; }
    }
}
