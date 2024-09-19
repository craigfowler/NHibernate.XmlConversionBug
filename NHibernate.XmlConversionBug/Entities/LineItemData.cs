namespace NHibernate.XmlConversionBug.Entities;

public class LineItemData
{
    public virtual LineItem LineItem { get; set; }

    public virtual string Data { get; set; }
}