using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Interfaces
{
    public interface IRespository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object id);
        void Insert(T obj);
        T InsertWithReturnId(T obj);
        void Update(T obj);
        void Delete(object id);
        void Save();
        void RemoveEntity(T obj);
    }
}
