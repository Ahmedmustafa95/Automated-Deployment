namespace AutomatedDeployment.Core.Interfaces
{
    public interface IGenericDeleteRepository<T> where T : class
    {
        T Delete(int id);
    }
}
