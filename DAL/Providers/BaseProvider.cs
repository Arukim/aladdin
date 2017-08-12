using System;
using System.Collections.Generic;
using System.Linq;
using Aladdin.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Aladdin.DAL.Providers
{
    internal abstract class AbstractDataProvider<T> : IRepository<T>
    {
        protected IMongoDatabase _db;
        protected string _collectionName;

        public AbstractDataProvider()
        {
            try
            {
                var mongoClient = new MongoClient();
                _db = mongoClient.GetDatabase("aladdin");
                _collectionName = typeof(T).Name.Replace("Entity", "") + "s";
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }

        protected IMongoCollection<T> Collection => _db.GetCollection<T>(_collectionName);


#region IRepository

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public T Find(int id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAll()
        {
            return Collection.AsQueryable();
        }

#endregion
    }
}