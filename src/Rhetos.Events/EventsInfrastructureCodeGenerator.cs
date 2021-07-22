/*
    Copyright (C) 2014 Omega software d.o.o.

    This file is part of Rhetos.

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as
    published by the Free Software Foundation, either version 3 of the
    License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Rhetos.Compiler;
using Rhetos.Dom.DefaultConcepts;
using Rhetos.Dsl;
using Rhetos.Extensibility;
using System.ComponentModel.Composition;

namespace Rhetos.Events
{
    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(InitializationConcept))]
    public class EventsInfrastructureCodeGenerator : IConceptCodeGenerator
    {
        public static readonly string EventNameConstantTag = "/*EventsInfrastructure.EventNameConstant*/";

        public static readonly string EventNameAndDataTypeTag = "/*EventsInfrastructure.EventNameAndDataType*/";

        /// <summary>
        /// Note for extensions:
        /// Use Lazy dependencies for event-specific handlers for better performance since this class is used for all event types in the system.
        /// </summary>
        public static readonly string EventProcessing_FieldsTag = "/*EventsInfrastructure.EventProcessing.Fields*/";

        /// <summary>
        /// Note for extensions:
        /// Use Lazy dependencies for event-specific handlers for better performance since this class is used for all event types in the system.
        /// </summary>
        public static readonly string EventProcessing_ConstructorParametersTag = "/*EventsInfrastructure.EventProcessing.ConstructorParameters*/";

        public static readonly string EventProcessing_ConstructorBodyTag = "/*EventsInfrastructure.EventProcessing.ConstructorBody*/";

        /// <summary>
        /// Additional event validation, or event preprocessing for incorrectly emitted events.
        /// </summary>
        public static readonly string EmitEvent_BeforeEventValidatedTag = "/*EventsInfrastructure.EmitEvent.BeforeEventValidated*/";

        /// <summary>
        /// Event preprocessing that allows canceling or overriding events.
        /// </summary>
        public static readonly string EmitEvent_BeforeHandlersTag = "/*EventsInfrastructure.EmitEvent.BeforeHandlers*/";

        /// <summary>
        /// In most cases, specific event handlers (that handle a single specific event) should be inserted directly at the code
        /// where the event occurs (for example, see SaveMethod concept on Entity), and there is no need to use the event processing.
        /// It makes sense to use specific event handlers only when the handler cannot directly reference the component that emits the event.
        /// </summary>
        public static readonly string EmitEvent_SpecificHadlersTag = "/*EventsInfrastructure.EmitEvent.SpecificHadlers*/";

        /// <summary>
        /// Generic event handlers. For example, a handler that manages run-time event subscriptions for all types of events.
        /// </summary>
        public static readonly string EmitEvent_GenericHandlersTag = "/*EventsInfrastructure.EmitEvent.GenericHandlers*/";

        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            const string fileName = "Events";

            codeBuilder.InsertCodeToFile(
$@"using System;
using System.Collections.Generic;
using System.Linq;
using Rhetos.Dom.DefaultConcepts;

namespace Rhetos.Events
{{
    public static class EventName
    {{
        {EventNameConstantTag}
    }}

    public class EventProcessing : IEventProcessing
    {{
        {EventProcessing_FieldsTag}

        public EventProcessing(
            {EventProcessing_ConstructorParametersTag})
        {{
            {EventProcessing_ConstructorBodyTag}
        }}

        private static readonly Dictionary<string, Type> _eventTypes = new (string EventName, Type EventDataType)[]
        {{
            {EventNameAndDataTypeTag}
        }}.ToDictionary(e => e.EventName, e => e.EventDataType, StringComparer.Ordinal);

        public IEnumerable<string> GetEventNames() => _eventTypes.Keys;

        public void EmitEvent(string eventName, object eventData)
        {{
            {EmitEvent_BeforeEventValidatedTag}

            if (_eventTypes.TryGetValue(eventName, out Type eventDataType))
            {{
                if (eventData != null && !eventDataType.IsInstanceOfType(eventData))
                    throw new ArgumentException($""Invalid data type provided for event '{{eventName}}'. Expected '{{eventDataType}}', provided '{{eventData.GetType()}}'."");
            }}
            else
                throw new ArgumentException($""Cannot emit event type '{{eventName}}' because it is not registered. Use DSL concept Event (class EventInfo) to register a new event type."");

            {EmitEvent_BeforeHandlersTag}

            {EmitEvent_SpecificHadlersTag}

            {EmitEvent_GenericHandlersTag}
        }}
    }}
}}
",
                fileName);

            codeBuilder.InsertCode("builder.RegisterType<Rhetos.Events.EventProcessing>().As<Rhetos.Events.IEventProcessing>().InstancePerLifetimeScope();\r\n            ", ModuleCodeGenerator.CommonAutofacConfigurationMembersTag);
        }
    }
}
