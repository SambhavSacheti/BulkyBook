using System;
using System.Collections.Generic;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;

namespace BulkyBook.DataAccess.Repository
{
    public class StoredProc_Call : IStoredProc_Call
    {

        private readonly ApplicationDbContext _DbContext;
        private readonly string _ConnectionString;

        public StoredProc_Call(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
            _ConnectionString = dbContext.Database.GetDbConnection().ConnectionString;
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_ConnectionString);
            sqlConnection.Open();
            sqlConnection.Execute(procedureName, param,
                                commandType: CommandType.StoredProcedure);
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_ConnectionString);
            sqlConnection.Open();
            return sqlConnection.Query<T>(procedureName, param,
                                commandType: CommandType.StoredProcedure);
        }

        public (IEnumerable<T1>, IEnumerable<T2>) List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_ConnectionString);
            sqlConnection.Open();
            var result = SqlMapper.QueryMultiple(sqlConnection, procedureName, param,
            commandType: CommandType.StoredProcedure);
            var item1 = result?.Read<T1>().ToList() ?? new List<T1>();
            var item2 = result?.Read<T2>().ToList() ?? new List<T2>();
            return (item1, item2);
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_ConnectionString);
            sqlConnection.Open();
            var value = sqlConnection.Query<T>(procedureName, param, commandType: CommandType.StoredProcedure);

            return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            //return sqlConnection.QueryFirstOrDefault<T>(procedureName,param,commandType:CommandType.StoredProcedure);
        }

        public T Single<T>(string procedureName, DynamicParameters param = null)
        {
            using SqlConnection sqlConnection = new SqlConnection(_ConnectionString);
            sqlConnection.Open();
            return (T)Convert.ChangeType(sqlConnection.ExecuteScalar<T>(procedureName, param,
                                        commandType: CommandType.StoredProcedure),
                                        typeof(T));
        }
    }
}