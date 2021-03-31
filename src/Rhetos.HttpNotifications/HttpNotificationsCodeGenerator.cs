using Rhetos.Compiler;
using Rhetos.Dsl;
using Rhetos.Events;
using Rhetos.Extensibility;
using System.ComponentModel.Composition;

namespace Rhetos.HttpNotifications
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(InitializationConcept))]
    [ExportMetadata(MefProvider.DependsOn, typeof(EventsInfrastructureCodeGenerator))]
    public class HttpNotificationsCodeGenerator : IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            codeBuilder.InsertCode(
                "private readonly Rhetos.HttpNotifications.HttpNotificationsDispatcher _httpNotificationsDispatcher;\r\n        ",
                EventsInfrastructureCodeGenerator.EventProcessing_FieldsTag);

            codeBuilder.InsertCode(
                "Rhetos.HttpNotifications.HttpNotificationsDispatcher httpNotificationsDispatcher\r\n            ",
                EventsInfrastructureCodeGenerator.EventProcessing_ConstructorParametersTag);

            codeBuilder.InsertCode(
                "_httpNotificationsDispatcher = httpNotificationsDispatcher;\r\n            ",
                EventsInfrastructureCodeGenerator.EventProcessing_ConstructorBodyTag);

            codeBuilder.InsertCode(
                "_httpNotificationsDispatcher.NotifySubscribers(eventName, eventData);\r\n            ",
                EventsInfrastructureCodeGenerator.EmitEvent_GenericHandlersTag);
        }
    }
}
