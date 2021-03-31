using Rhetos.Dsl;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace Rhetos.Events
{
    /// <summary>
    /// Adds a new custom event type, with no event data type specified.
    /// </summary>
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("Event")]
    public class EventWithUndefinedDataTypeInfo : EventInfo, IAlternativeInitializationConcept
    {
        public IEnumerable<string> DeclareNonparsableProperties()
        {
            return new[] { "EventDataType" };
        }

        public void InitializeNonparsableProperties(out IEnumerable<IConceptInfo> createdConcepts)
        {
            EventDataType = "object";
            createdConcepts = null;
        }
    }
}
