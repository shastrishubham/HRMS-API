using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerModel.Data
{
    public static class DbContext
    {
        // <summary>
        /// Default database provider.
        /// </summary>
        public const string DEFAULT_PROVIDER = "System.Data.Sqiclient";

        /// <summary>
        /// Default area.
        /// </ summary>
        public const string DEFAULT_AREA = "Nalco.Data.Common";

        //private static Dictionary<string, DbContext> sDbContexts
        //    = new Dictionary<string, DbContext>();

        //private DbProviderFactory Factory { get; set; }

        //private Func<string, IDbConnection> NonProviderConnectionFactory {get; set;}

        // <summary>
        /// Database context's connection string
        /// </summary>
        public static string ConnectionString { get; set; }


        //public int CommandTimeout { get; private set; }

        //public bool UseDataTime2 { get; private set; }



        //internal DbContext(DbProviderFactory factory, string connectionString)
        //{
        //    Factory = factory;
        //    ConnectionString = connectionString;
        //    CommandTimeout = 30;
        //}


        //internal DbContext(Func<string, IDbConnection> factory, string connectionString)
        //{
        //    NonProviderConnectionFactory = factory;
        //    ConnectionString = connectionString;
        //    CommandTimeout = 30;
        //}



        //internal IDbConnection CreateConnection()
        //{
        //    if(null == Factory)
        //    {
        //        return NonProviderConnectionFactory(ConnectionString);
        //    }
        //    IDbConnection conn = Factory.CreateConnection();

        //    conn.ConnectionString = ConnectionString;

        //    return conn;
        //}



        //internal IDbCommand CreateCommand(string sql, IDbConnection conn)
        //{
        //    if (null == sql)
        //    {
        //        throw new ArgumentNullException("sql");
        //    }
        //    IDbCommand cmd = (null == Factory) ? conn.CreateCommand() : Factory.CreateCommand();

        //    cmd.CommandText = sql;
        //    cmd.Connection = conn;
        //    cmd.CommandTimeout = CommandTimeout;

        //    return cmd;
        //}


        //public IDbCommand CreateCommand(string sql, IDbTransaction tran)
        //{
        //    if (null == sql)
        //    {
        //        throw new ArgumentNullException("sql");
        //    }

        //    if (null == tran)
        //    {
        //        throw new ArgumentNullException("tran");
        //    }

        //    IDbCommand cmd = (null == Factory) 
        //        ? CreateConnection().CreateCommand() 
        //        : Factory.CreateCommand();

        //    cmd.CommandText = sql;
        //    cmd.Connection = tran.Connection;
        //    cmd.Transaction = tran;
        //    cmd.CommandTimeout = CommandTimeout;

        //    return cmd;
        //}


        //[Obsolete("CreateDataAdapter does not properly manage resources and should not be used.")]
        //internal IDbDataAdapter CreateDataAdapter()
        //{
        //    if(null == Factory)
        //    {
        //        throw new NotImplementedException();
        //    }
        //    return Factory.CreateDataAdapter();
        //}


        //public DbType DbType(Type type)
        //{
        //    if(UseDataTime2
        //        && (typeof(DateTime) == type 
        //            || typeof(DateTime?) == type))
        //    {
        //        return System.Data.DbType.DateTime2;
        //    }
        //    else if(typeof(TimeSpan) == type 
        //        || typeof(TimeSpan?) == type)
        //    {
        //        return System.Data.DbType.Time;
        //    }
        //    else if(typeof(byte[]) == type)
        //    {
        //        return System.Data.DbType.Binary;
        //    }
        //    else
        //    {
        //        return (DbType)TypeDescriptor
        //            .GetConverter(typeof(DbType))
        //            .ConvertFrom(type.IsEnum 
        //                ? Enum.GetUnderlyingType(type).Name 
        //                : type.Name);
        //    }
        //}



        //public SqlDbType SqlDbType(Type type)
        //{
        //    SqlParameter parameter = new SqlParameter();
        //    parameter.DbType = DbType(type);
        //    return parameter.SqlDbType;
        //}


        //public string DataSource
        //{
        //    get
        //    {
        //        using(IDbConnection conn = CreateConnection())
        //        {
        //            DbConnection dbconn = conn as DbConnection;
        //            if(null == dbconn)
        //            {
        //                throw new NotImplementedException();
        //            }
        //            return dbconn.DataSource;
        //        }
        //    }
        //}


        //public string Catalog
        //{
        //    get
        //    {
        //        using(IDbConnection conn = CreateConnection())
        //        {
        //            return conn.Database;
        //        }
        //    }
        //}

        //public static DbContext GetDbContext()
        //{
        //    return GetDbContext(DEFAULT_AREA);
        //}

        //private static DbContext GetDbContext(string area)
        //{
        //    DbContext context = TryGetDbContext(area);

        //    if(null == context)
        //    {
        //        throw new ArgumentNullException("context");
        //    }

        //    return context;
        //}

        //private static DbContext TryGetDbContext(string area)
        //{
        //    if (null == area)
        //    {
        //        throw new ArgumentNullException("area");
        //    }

        //    DbContext context;

        //    sDbContexts.TryGetValue(area, out context);

        //    return context;
        //}

        //public static bool DataSourceExists()
        //{
        //    return DataSourceExists(DEFAULT_AREA);
        //}

        //private static bool DataSourceExists(string area)
        //{
        //    DbContext context = sDbContexts[area];

        //    using(IDbConnection conn = context.CreateConnection())
        //    {
        //        try
        //        {
        //            return true;
        //        }
        //        catch (DbException)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}


        //public static void ClearPool()
        //{
        //    ClearPool(DEFAULT_AREA);
        //}

        //private static void ClearPool(string area)
        //{

        //    DbContext context = sDbContexts[area];

        //    using (IDbConnection conn = context.CreateConnection())
        //    {
        //        System.Data.SqlClient.SqlConnection sql 
        //            = conn as System.Data.SqlClient.SqlConnection;

        //        if(null != sql)
        //        {
        //            System.Data.SqlClient.SqlConnection.ClearPool(sql);
        //        }
        //    }
        //}


        //public static DbContext AddConnectionString(string connectionString)
        //{
        //    return SetConnectionArea(DbProviderFactories.GetFactory(DEFAULT_PROVIDER), DEFAULT_AREA, connectionString);
        //}


        //public static DbContext AddConnectionString(string area, string connectionString)
        //{
        //    return SetConnectionArea(DbProviderFactories.GetFactory(DEFAULT_PROVIDER), area, connectionString);
        //}


        //public static DbContext AddConnectionString(string provider, string area, string connectionString)
        //{
        //    return SetConnectionArea(DbProviderFactories.GetFactory(provider), area, connectionString);
        //}

        //public static DbContext AddConnectionString(DbProviderFactory factory, string area, string connectionString)
        //{
        //    return SetConnectionArea(factory, area, connectionString);
        //}

        //public static DbContext AddConnectionString(Func<string, IDbConnection> factory, string connectionString)
        //{
        //    return AddConnectionString(factory, DEFAULT_AREA, connectionString);
        //}



        //public static DbContext AddConnectionString(Func<string, IDbConnection> factory, 
        //    string area, string connectionString)
        //{
        //    return SetConnectionArea(factory, area, connectionString);
        //}


        //public static DbContext AddStandardDataSource(string dataSource, 
        //    string initialCatalog, string userID, string password)
        //{
        //    return AddStandardDataSource(DEFAULT_PROVIDER, DEFAULT_AREA, dataSource, initialCatalog, userID, password);
        //}


        //public static DbContext AddStandardDataSource(string area,
        //   string dataSource, string initialCatalog, string userID, 
        //   string password)
        //{
        //    return AddStandardDataSource(DEFAULT_PROVIDER, area, dataSource, initialCatalog, userID, password);
        //}

        //public static DbContext AddStandardDataSource(string provider,
        //  string area, string dataSource, string initialCatalog,
        //  string userID, string password)
        //{
        //    return AddStandardDataSource(DbProviderFactories.GetFactory(provider), area, dataSource, initialCatalog, userID, password);
        //}


        //public static DbContext AddStandardDataSource(DbProviderFactory factory,
        // string area, string dataSource, string initialCatalog,
        // string userID, string password)
        //{
        //    DbConnectionStringBuilder csb = factory.CreateConnectionStringBuilder();

        //    csb["Data Source"] = dataSource;
        //    csb["Initial Catalog"] = initialCatalog;
        //    csb["Application Name"] = area;
        //    csb["User Id"] = userID;
        //    csb["Password"] = password;

        //    return SetConnectionArea(factory, area, csb.ConnectionString);
        //}


        //public static DbContext AddStandardDataSourceFromContext(string fromArea, 
        //    string toArea, string userID, string password)
        //{
        //    DbContext context = sDbContexts[fromArea];
        //    DbConnectionStringBuilder csb = context.Factory.CreateConnectionStringBuilder();

        //    csb.ConnectionString = context.ConnectionString;

        //    csb.Remove("Integrated Security");

        //    csb["User Id"] = userID;
        //    csb["Password"] = password;

        //    DbContext standard = SetConnectionArea(context.Factory, toArea, csb.ConnectionString);

        //    standard.CommandTimeout = context.CommandTimeout;
        //    standard.UseDataTime2 = context.UseDataTime2;

        //    return standard;
        //}


        //public static DbContext AddTrustedDataSource(string dataSource, string initialCatalog)
        //{
        //    return AddTrustedDataSource(DEFAULT_PROVIDER, DEFAULT_AREA, dataSource, initialCatalog);
        //}

        //private static DbContext AddTrustedDataSource(string area, string dataSource, string initialCatalog)
        //{
        //    return AddTrustedDataSource(DEFAULT_PROVIDER, area, dataSource, initialCatalog);
        //}

        //private static DbContext AddTrustedDataSource(string provider, string area, string dataSource, string initialCatalog)
        //{
        //    return AddTrustedDataSource(DbProviderFactories.GetFactory(provider), area, dataSource, initialCatalog);
        //}

        //public static DbContext AddTrustedDataSource(DbProviderFactory factory, string area, string dataSource, string initialCatalog)
        //{
        //    DbConnectionStringBuilder csb = factory.CreateConnectionStringBuilder();

        //    csb["Data Source"] = dataSource;
        //    csb["Initial Catalog"] = initialCatalog;
        //    csb["Application Name"] = area;
        //    csb["Integrated Security"] = true;

        //    return SetConnectionArea(factory, area, csb.ConnectionString);
        //}


        //public static DbContext AddTrustedDataSourceFromContext(string fromArea, string toArea)
        //{
        //    DbContext context = sDbContexts[fromArea];
        //    DbConnectionStringBuilder csb = context.Factory.CreateConnectionStringBuilder();

        //    csb.ConnectionString = context.ConnectionString;

        //    csb.Remove("User Id");
        //    csb.Remove("Password");

        //    csb["Integrated Security"] = true;

        //    DbContext trusted = SetConnectionArea(context.Factory, toArea, csb.ConnectionString);

        //    trusted.CommandTimeout = context.CommandTimeout;
        //    trusted.UseDataTime2 = context.UseDataTime2;

        //    return trusted;
        //}

        //private static DbContext SetConnectionArea(DbProviderFactory factory, string area, string connectionString)
        //{
        //    if(null == connectionString)
        //    {
        //        throw new ArgumentNullException("connectionString");
        //    }

        //    if(null == area)
        //    {
        //        throw new ArgumentNullException("area");
        //    }

        //    return sDbContexts[area] = new DbContext(factory, connectionString);
        //}

        //public static DbContext SetConnectionArea(Func<string, IDbConnection> factory, string area, string connectionString)
        //{
        //    if(null == connectionString)
        //    {
        //        throw new ArgumentNullException("connectionString");
        //    }

        //    if(null == area)
        //    {
        //        throw new ArgumentNullException("area");
        //    }

        //    return sDbContexts[area] = new DbContext(factory, connectionString);
        //}
      
    }
}
