namespace Common
{
    public interface IDbPool
    {
        void DisposeDb(string key = null);

        string GetCurrentKey(string prefix = "");

        void ChangeKey(string oldKey, string newKey);

        IDB GetDb(string key);
    }
}
