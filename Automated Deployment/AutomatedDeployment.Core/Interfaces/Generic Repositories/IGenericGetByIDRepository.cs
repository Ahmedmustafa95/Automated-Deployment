namespace AutomatedDeployment.Core.Interfaces
{
    public interface IGenericGetByIDRepository<T> where T : class
    {
        T GetById(int id);
    }
}
