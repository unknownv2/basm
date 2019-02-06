using System;
using System.Collections.Generic;
using System.Text;

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
        /// <param name="directive"></param>
        /// <returns></returns>
        public DirectiveStack Add(IDirective directive)
        {
            throw new NotImplementedException();
        }
    }
}
