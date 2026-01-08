using ServerModel.Database;
using ServerModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Repository
{
    public class Repository<T> : IRespository<T> where T : class
    {
        private HRMSEntities _context = null;
        private DbSet<T> table = null;

        public Repository()
        {
            this._context = new HRMSEntities();
            table = _context.Set<T>();
        }

        public Repository(HRMSEntities _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public void Delete(object id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object id)
        {
            return table.Find(id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public T InsertWithReturnId(T obj)
        {
            table.Add(obj);
            return obj;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public void RemoveEntity(T obj)
        {
            table.Remove(obj);
            _context.SaveChanges();
        }
    }
}
