﻿using GoReal.Dal.Entities;
using GoReal.Dal.Repository.Extensions;
using GoReal.Common.Interfaces;
using System.Linq;
using Tools.Databases;
using System.Collections.Generic;
using System.Data.SqlClient;
using GoReal.Common.Exceptions;

namespace GoReal.Dal.Repository
{
    public class StatisticRepository : IRepository<Statistic>
    {
        private readonly Connection _connection;

        public StatisticRepository(Connection connection)
        {
            _connection = connection;
        }

        public Statistic Get(int id)
        {
            Statistic statistic = new Statistic();
            Command cmd = new Command("StatisticsGet", true);
            cmd.AddParameter("UserId", id);
            try
            {
                statistic = _connection.ExecuteReader(cmd, (dr) => dr.ToStatistic()).SingleOrDefault();
            }
            catch (SqlException e)
            {
                throw new StatisticException("user not found");
            }

            return statistic;
        }

        public bool Create(Statistic entity)
        {
            throw new System.NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public bool Update(int id, Statistic entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
