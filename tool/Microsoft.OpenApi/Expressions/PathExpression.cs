﻿// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

namespace Microsoft.OpenApi.Expressions
{
    /// <summary>
    /// Path expression, the name in path is case-sensitive.
    /// </summary>
    public sealed class PathExpression : SourceExpression
    {
        /// <summary>
        /// path. string
        /// </summary>
        public const string Path = "path.";

        /// <summary>
        /// Gets the expression string.
        /// </summary>
        public override string Expression
        {
            get
            {
                return Path + Value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PathExpression"/> class.
        /// </summary>
        /// <param name="name">The name string, it's case-insensitive.</param>
        public PathExpression(string name)
            : base(name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw Error.ArgumentNullOrWhiteSpace(nameof(name));
            }
        }
    }
}
