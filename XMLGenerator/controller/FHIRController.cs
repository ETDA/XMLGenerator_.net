using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization;
using Hl7.Fhir.Specification.Source;
using Hl7.Fhir.Validation;
//using Hl7.Fhir.Utility;
namespace XMLGenerator.controller
{
    class FHIRController
    {
        public static bool Validate(string filePath)
        {
            try
            {
                var parser = new FhirXmlParser(new ParserSettings
                {
                    AcceptUnknownMembers = true,
                    AllowUnrecognizedEnums = true,
                });

                Bundle bundle = parser.Parse<Bundle>((new StreamReader(filePath)).ReadToEnd());

                var ctx = new ValidationSettings()
                {
                    GenerateSnapshot = true,
                    Trace = false,
                    EnableXsdValidation = true,
                    ResolveExternalReferences = false
                };
                var validator = new Validator(ctx);
                validator.Validate(bundle);
                return true;
            } catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                return false;
            }
        } 
    }
}
