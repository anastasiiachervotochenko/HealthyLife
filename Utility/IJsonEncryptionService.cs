namespace HealthyLife.Utility
{
    public interface IJsonEncryptionService
    {
        TEntity Decrypt<TEntity>(string data);

        string Encrypt<TEntity>(TEntity entity);
    }
}