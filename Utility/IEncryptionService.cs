namespace HealthyLife.Utility
{
    public interface IEncryptionService
    {
        string Encrypt(string data);

        string Decrypt(string data);

        string Hash(string data);

        bool CompareWithHash(string data, string hash);
    }
}