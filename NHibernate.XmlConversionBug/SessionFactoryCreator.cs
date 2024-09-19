using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Mapping.ByCode;
using NHibernate.XmlConversionBug.Mappings;

namespace NHibernate.XmlConversionBug;

public static class SessionFactoryCreator
{
    public static ISessionFactory GetSessionFactoryUsingPureMbc()
    {
        var config = GetCommonConfiguration();
        config.AddMapping(GetMappingByCodeMappings());
        return config.BuildSessionFactory();
    }
    
    public static ISessionFactory GetSessionFactoryUsingXmlConversion()
    {
        var config = GetCommonConfiguration();
        config.AddXml(GetMappingByCodeMappings().AsString());
        return config.BuildSessionFactory();
    }

    static HbmMapping GetMappingByCodeMappings()
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(new []{typeof(OrderMapping), typeof(LineItemMapping), typeof(LineItemDataMapping)});
        return mapper.CompileMappingForAllExplicitlyAddedEntities();
    }

    static Configuration GetCommonConfiguration()
    {
        var config = new Configuration();
        config.DataBaseIntegration(x =>
        {
            x.Dialect<SQLiteDialect>();
            x.ConnectionString = "Data Source=:memory:";
        });
        return config;
    }
}