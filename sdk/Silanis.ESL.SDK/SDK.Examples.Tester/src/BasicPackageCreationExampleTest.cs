using NUnit.Framework;
using System;
using Silanis.ESL.SDK;
using System.Collections.Generic;
using Silanis.ESL.SDK.Builder;

namespace SDK.Examples
{
    [TestFixture()]
    public class BasicPackageCreationExampleTest
    {
        [Test()]
        public void VerifyResult()
        {
            BasicPackageCreationExample example = new BasicPackageCreationExample();
            example.Run();

            DocumentPackage documentPackage = example.RetrievedPackage;

            // Verify if the package is created correctly
            Assert.AreEqual("This is a package created using the eSignLive SDK", documentPackage.Description);
            Assert.AreEqual("This message should be delivered to all signers", documentPackage.EmailMessage);

            Assert.AreEqual(false, documentPackage.Settings.EnableInPerson);

            // Verify if the sdk version is set correctly
            Assert.IsTrue(documentPackage.Attributes.Contents.ContainsKey( "sdk" ));
            Assert.IsTrue(documentPackage.Attributes.Contents["sdk"].ToString().Contains(".NET"));

            // Verify if the origin is set correctly
            Assert.IsTrue(documentPackage.Attributes.Contents.ContainsKey("origin"));
            Assert.IsTrue(documentPackage.Attributes.Contents["origin"].ToString().Contains("api"));

            // Signer 1
            Signer signer = documentPackage.GetSigner(example.email1);
            Assert.AreEqual("Client1", signer.Id);
            Assert.AreEqual("John", signer.FirstName);
            Assert.AreEqual("Smith", signer.LastName);
            Assert.AreEqual("Managing Director", signer.Title);
            Assert.AreEqual("Acme Inc.", signer.Company);

            // Signer 2
            signer = documentPackage.GetSigner(example.email2);
            Assert.AreEqual("Patty", signer.FirstName);
            Assert.AreEqual("Galant", signer.LastName);

            // Document 1
            Document document = documentPackage.GetDocument(example.DOCUMENT1_NAME);
            Assert.AreEqual(1, document.NumberOfPages);

            List<Field> fields = document.Signatures[0].Fields;
            Field field = fields[0];

            Assert.AreEqual(FieldStyle.UNBOUND_CHECK_BOX, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual(FieldBuilder.CHECKBOX_CHECKED, field.Value);

            // Document 2
            document = documentPackage.GetDocument(example.DOCUMENT2_NAME);
            Assert.AreEqual(1, document.NumberOfPages);

            fields = document.Signatures[0].Fields;

            field = findFieldByName("firstField", fields);
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual("", field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

            field = findFieldByName("secondField", fields);
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual(FieldBuilder.RADIO_SELECTED, field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

            field = findFieldByName("thirdField", fields);
            Assert.AreEqual(FieldStyle.UNBOUND_RADIO_BUTTON, field.Style);
            Assert.AreEqual(0, field.Page);
            Assert.AreEqual("", field.Value);
            Assert.AreEqual("group", field.Validator.Options[0]);

        }

        private Field findFieldByName(string fieldName, List<Field> fields)
        {
            foreach (Field field in fields) 
            {
                if (field.Name != null && field.Name.Equals(fieldName)) 
                {
                    return field;
                }
            }
            
            return null;
        }
    }
}

