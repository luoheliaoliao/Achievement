namespace Common
{
    public interface IDB
    {
        void BeginTransaction(string tranKey = "");

        void EndTransaction(string tranKey = "");

        void Commit(string tranKey = "");

        void Rollback(string tranKey = "");

    }

}
