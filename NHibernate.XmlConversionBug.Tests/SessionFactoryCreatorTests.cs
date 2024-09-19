namespace NHibernate.XmlConversionBug.Tests;

[TestFixture]
public class SessionFactoryCreatorTests
{
    [Test]
    public void CreatingASessionFactoryWithPureMbcShouldNotThrow()
    {
        Assert.That(SessionFactoryCreator.GetSessionFactoryUsingPureMbc, Throws.Nothing);
    }
    
    [Test]
    public void CreatingASessionFactoryWithMbcConvertedToXmlShouldNotThrow()
    {
        Assert.That(SessionFactoryCreator.GetSessionFactoryUsingXmlConversion, Throws.Nothing);
    }
}