using System;
using System.Collections.Generic;

namespace Basm.Scripting
{
    public struct DirectiveStack
    {
        private readonly IEnumerable<IDirective> _directives;

        public DirectiveStack(IEnumerable<IDirective> directives)
        {
            _directives = directives;
        }

        /// <summary>
        /// Add a directive to the list of defined directives.
        /// </summary>
        /// <param name="directive">The directive to add.</param>
        /// <returns>The current directive stack including the new directive.</returns>
        public DirectiveStack Add(IDirective directive)
        {
            throw new NotImplementedException();
        }
    }
}
