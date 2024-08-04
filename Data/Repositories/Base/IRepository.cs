using Core.Entities;

namespace Data.Repositories.Base;

public interface IRepository<T> where T : BaseEntity
{

    List<T> GetAll();
    T Get(DateTime date);
    void Add(T item);
    void Delete(T item);


}
