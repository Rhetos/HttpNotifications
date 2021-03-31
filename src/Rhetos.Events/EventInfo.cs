using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using System.ComponentModel.Composition;

namespace Rhetos.Events
{
    /// <summary>
    /// Adds a new custom event type.
    /// EventDataType parameter is a C# type of the event data (for example, 'IEnumerable&lt;Guid&gt;').
    /// </summary>
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("Event")]
    public class EventInfo : IValidatedConcept
    {
        [ConceptKey]
        public string EventName { get; set; }

        public string EventDataType { get; set; }

        public void CheckSemantics(IDslModel existingConcepts)
        {
            DslUtility.ValidateIdentifier(EventName, this);
        }
    }

    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(EventInfo))]
    public class EventCodeGenerator : IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (EventInfo)conceptInfo;

            codeBuilder.InsertCode(
                $"public const string {info.EventName} = \"{info.EventName}\";\r\n        ",
                EventsInfrastructureCodeGenerator.EventNameConstantTag);

            codeBuilder.InsertCode(
                $"(EventName.{info.EventName}, typeof({info.EventDataType})),\r\n            ",
                EventsInfrastructureCodeGenerator.EventNameAndDataTypeTag);
        }
    }
}
