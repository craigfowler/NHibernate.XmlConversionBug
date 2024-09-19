using NHibernate.Id;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.XmlConversionBug.Entities;

namespace NHibernate.XmlConversionBug.Mappings;

public class LineItemDataMapping : ClassMapping<LineItemData>
{
    public LineItemDataMapping()
    {
        OneToOne(x => x.LineItem, m => m.Constrained(true));
        
        Property(x => x.Data);
    }
}