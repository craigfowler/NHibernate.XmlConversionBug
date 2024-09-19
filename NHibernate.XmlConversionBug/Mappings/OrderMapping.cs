using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.XmlConversionBug.Entities;

namespace NHibernate.XmlConversionBug.Mappings;

public class OrderMapping : ClassMapping<Order>
{
    public OrderMapping()
    {
        Id(x => x.Id, m => m.Generator(new IdentityGeneratorDef()));
        
        Property(x => x.CreatedDate);
        
        Set(x => x.Items, m =>
        {
            m.Inverse(true);
            m.OptimisticLock(true);
        }, a => a.OneToMany());
    }
}