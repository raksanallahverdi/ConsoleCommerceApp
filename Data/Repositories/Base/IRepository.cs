using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories.Base;

public interface IRepository<T> where T : BaseEntity
{

    List<T> GetAll();
    T Get(DateTime date);
    void Add(T item);
    void Delete(T item);


}
