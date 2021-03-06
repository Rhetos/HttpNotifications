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
