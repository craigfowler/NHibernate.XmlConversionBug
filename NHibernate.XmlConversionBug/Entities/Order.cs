namespace NHibernate.XmlConversionBug.Entities;

public class Order
{
    public virtual long Id { get; set; }

    public virtual DateTime CreatedDate { get; set; }

    public virtual ISet<LineItem> Items { get; protected set; } = new HashSet<LineItem>();
}