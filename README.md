# NHibernate MBC to XML conversion crash bug

Reproduction case for an NHibernate crash issue when converting Mapping By Code (MBC) mappings into XML, when those MBC mappings include any **One to one** mappings.
The `optimistic-lock` element/attribute is incorrectly serialized to XML and subsequently causes an XML schema validation error.

## Versions affected

I tried this out against a variety of NHibernate versions.

* 5.3.7 is unaffected
* 5.3.20 seems unaffected
* 5.4.0 seems unaffected
* 5.4.1 _reproduces the problem_
* 5.5.0 _reproduces the problem_
* _The problem is still present in 5.5.2_, which is the latest version at the time of writing

So, it would certainly seem that **5.4.1 is the first affected version**.

## Sample reproduction case

There are two unit tests in the test project.  One demonstrates the behavior using 'pure MBC' and the other uses the 'convert MBC mappings to XML' technique.
The remainder of the project is a minimal NHibernate entity & mapping setup with 3 entities, mappings and a small helper to create a session factory for them.
It uses SQLite in-memory driver/dialect so as to be self-contained.
As far as I can tell the DB driver/dialect is irrelevant though, because we reproduced this in an MS SQL Server environment.

1. Clone out the repository
2. Build with .NET 8 or higher: `dotnet build`
3. Run the unit tests: `dotnet test`
    * Optional, to run the tests against a specific NHibernate version set the `NhVersion` property to the desired NHibernate version
    * For example: `dotnet test /p:NhVersion=5.4.9`

### Expected behaviour

The tests for both techniques should pass (should not throw an exception).

### Actual behaviour

The 'pure MBC' test passes, but the test for the technique which converts to XML fails with an exception:

```text
Expected: No Exception to be thrown
  But was:  <NHibernate.MappingException: (string)(25,8): XML validation error: The element 'one-to-one' in namespace 'urn:nhibernate-mapping-2.2' has invalid child element 'OptimisticLock' in namespace 'urn:nhibernate-mapping-2.2'. List of possible elements expected: 'meta, formula' in namespace 'urn:nhibernate-mapping-2.2'.
 ---> System.Xml.Schema.XmlSchemaValidationException: The element 'one-to-one' in namespace 'urn:nhibernate-mapping-2.2' has invalid child element 'OptimisticLock' in namespace 'urn:nhibernate-mapping-2.2'. List of possible elements expected: 'meta, formula' in namespace 'urn:nhibernate-mapping-2.2'.
   --- End of inner exception stack trace ---
   at NHibernate.Cfg.Configuration.LogAndThrow(Exception exception)
   at NHibernate.Cfg.Configuration.ValidationHandler(Object o, ValidationEventArgs args)
   at System.Xml.Schema.XmlSchemaValidator.ValidateElementContext(XmlQualifiedName elementName, Boolean& invalidElementInContext)
   at System.Xml.Schema.XmlSchemaValidator.ValidateElement(String localName, String namespaceUri, XmlSchemaInfo schemaInfo, String xsiType, String xsiNil, String xsiSchemaLocation, String xsiNoNamespaceSchemaLocation)
   at System.Xml.XsdValidatingReader.ProcessElementEvent()
   at System.Xml.XsdValidatingReader.Read()
   at System.Xml.XmlLoader.LoadNode(Boolean skipOverWhitespace)
   at System.Xml.XmlLoader.LoadDocSequence(XmlDocument parentDoc)
   at System.Xml.XmlDocument.Load(XmlReader reader)
   at NHibernate.Cfg.Configuration.LoadMappingDocument(XmlReader hbmReader, String name)
   at NHibernate.Cfg.Configuration.AddXmlReader(XmlReader hbmReader, String name)
   at NHibernate.Cfg.Configuration.AddXml(String xml, String name)
   at NHibernate.Cfg.Configuration.AddXml(String xml)
   at NHibernate.XmlConversionBug.SessionFactoryCreator.GetSessionFactoryUsingXmlConversion() in C:\repos\NHibernate.XmlConversionBug\NHibernate.XmlConversionBug\SessionFactoryCreator.cs:line 21
   at System.RuntimeMethodHandle.InvokeMethod(Object target, Void** arguments, Signature sig, Boolean isConstructor)
   at System.Reflection.MethodBaseInvoker.InvokeWithNoArgs(Object obj, BindingFlags invokeAttr)
--- End of stack trace from previous location ---
   at NUnit.Framework.Internal.ExceptionHelper.Rethrow(Exception exception)
   at NUnit.Framework.Internal.Reflect.DynamicInvokeWithTransparentExceptions(Delegate delegate)
   at NUnit.Framework.Internal.ExceptionHelper.RecordException(Delegate parameterlessDelegate, String parameterName)>

   at NHibernate.XmlConversionBug.Tests.SessionFactoryCreatorTests.CreatingASessionFactoryWithMbcConvertedToXmlShouldNotThrow() in C:\repos\NHibernate.XmlConversionBug\NHibernate.XmlConversionBug.Tests\SessionFactoryCreatorTests.cs:line 15
```

## Related info

This affects the viability of a workaround for another NHibernate bug; [issue 2307 on the NHibernate issue tracker](https://github.com/nhibernate/nhibernate-core/issues/2307).
That workaround to issue 2307 (which affects the capabilities of MBC mappings) is to use the 'convert MBC mappings to XML' technique.
That workaround is no longer viable for mappings which include any one to one relationships, because that would now cause a crash error.
