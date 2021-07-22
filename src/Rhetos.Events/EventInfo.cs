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
