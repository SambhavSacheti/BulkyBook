using System;
using System.Collections.Generic;
using Dapper;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IStoredProc_Call : IDisposable
    {

        // Returns a integer or boolean value
        T Single<T>(string procedureName, DynamicParameters param = null);

        void Execute(string procedureName, DynamicParameters param = null);

        // returns a complete row 
        T OneRecord<T>(string procedureName, DynamicParameters param = null);

        IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null);

        (IEnumerable<T1>,IEnumerable<T2>) List<T1,T2>(string procedureName, DynamicParameters param = null);

    }
}