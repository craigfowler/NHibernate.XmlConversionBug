using NHibernate.Id;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.XmlConversionBug.Entities;

namespace NHibernate.XmlConversionBug.Mappings;

public class LineItemMapping : ClassMapping<LineItem>
{
    public LineItemMapping()
    {
        Id(x => x.Id, m => m.Generator(new IdentityGeneratorDef()));
        
        Property(x => x.ItemName);

        Property(x => x.Amount);
        
        ManyToOne(x => x.Order);
    }
}