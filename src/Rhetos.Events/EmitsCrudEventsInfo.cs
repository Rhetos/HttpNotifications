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
using Rhetos.Dsl.DefaultConcepts;
using Rhetos.Extensibility;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhetos.Events
{
    /// <summary>
    /// After saving changes emits events "ModuleName_EntityName_Deleted", "ModuleName_EntityName_Updated" and "ModuleName_EntityName_Inserted".
    /// The event data contains IDs of the records, type "IEnumerable&lt;Guid&gt;".
    /// </summary>
    [Export(typeof(IConceptInfo))]
    [ConceptKeyword("EmitsCrudEvents")]
    public class EmitsCrudEventsInfo : IConceptInfo
    {
        [ConceptKey]
        public EntityInfo Entity { get; set; }
    }

    [Export(typeof(IConceptMacro))]
    public class EmitsCrudEventsMacro : IConceptMacro<EmitsCrudEventsInfo>
    {
        public IEnumerable<IConceptInfo> CreateNewConcepts(EmitsCrudEventsInfo conceptInfo, IDslModel existingConcepts)
        {
            string eventPrefix = $"{conceptInfo.Entity.Module.Name}_{conceptInfo.Entity.Name}_";
            return new IConceptInfo[]
            {
                new EventInfo { EventName = eventPrefix + "Deleted", EventDataType = "ICollection<Guid>" },
                new EventInfo { EventName = eventPrefix + "Updated", EventDataType = "ICollection<Guid>" },
                new EventInfo { EventName = eventPrefix + "Inserted", EventDataType = "ICollection<Guid>" },
                new RepositoryUsesInfo { DataStructure = conceptInfo.Entity, PropertyName = "_eventProcessing", PropertyType = "Rhetos.Events.IEventProcessing, Rhetos.Events" },
            };
        }
    }

    [Export(typeof(IConceptCodeGenerator))]
    [ExportMetadata(MefProvider.Implements, typeof(EmitsCrudEventsInfo))]
    public class EmitsCrudEventsCodeGenerator: IConceptCodeGenerator
    {
        public void GenerateCode(IConceptInfo conceptInfo, ICodeBuilder codeBuilder)
        {
            var info = (EmitsCrudEventsInfo)conceptInfo;
            string eventPrefix = $"Rhetos.Events.EventName.{info.Entity.Module.Name}_{info.Entity.Name}_";

            string afterSaveEmitCrudEvents =
                $@"// EmitsCrudEvents:
                if (deletedIds.Any())
                    _eventProcessing.EmitEvent({eventPrefix}Deleted, deletedIds.Select(item => item.ID).ToList());
                if (updatedNew.Any())
                    _eventProcessing.EmitEvent({eventPrefix}Updated, updatedNew.Select(item => item.ID).ToList());
                if (insertedNew.Any())
                    _eventProcessing.EmitEvent({eventPrefix}Inserted, insertedNew.Select(item => item.ID).ToList());

                ";

            codeBuilder.InsertCode(afterSaveEmitCrudEvents, WritableOrmDataStructureCodeGenerator.AfterSaveTag, info.Entity);
        }
    }
}
