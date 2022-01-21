namespace Adverthouse.Common.Data.RocksDB
{

    public enum RocksDBStatusCodes
    {
        Error = 5000,
        Ok = 2000,
        Authenticated = 1000,
        MistakePassword = 1001,
        UserNotFound = 1002,
        IncorrectPassword = 1003,
        NotFound = 1006,
        AlreadyExists = 20001
    }
}
