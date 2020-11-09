// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using Microsoft.OData.Edm;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData.Common;
using Microsoft.OpenApi.OData.Generator;

namespace Microsoft.OpenApi.OData.Operation
{
    /// <summary>
    /// The Open Api operation for <see cref="IEdmFunction"/>.
    /// </summary>
    internal class EdmFunctionOperationHandler : EdmOperationOperationHandler
    {
        /// <inheritdoc/>
        public override OperationType OperationType => OperationType.Get;

        /// <summary>
        /// Gets the Edm Function.
        /// </summary>
        public IEdmFunction Function => EdmOperation as IEdmFunction;

        /// <inheritdoc/>
        protected override void SetExtensions(OpenApiOperation operation)
        {
            operation.Extensions.Add(Constants.xMsDosOperationType, new OpenApiString("function"));
            base.SetExtensions(operation);
        }

        /// <inheritdoc/>
        protected override void SetParameters(OpenApiOperation operation)
        {
            base.SetParameters(operation);

            OpenApiParameter parameter;
            IEdmStructuredType structuredType = EdmOperation.ReturnType.AsEntity().EntityDefinition();

            if (EdmOperation.ReturnType.Definition.TypeKind == EdmTypeKind.Collection)
            {
                structuredType = (IEdmStructuredType) ((IEdmCollectionType) EdmOperation.ReturnType.Definition).ElementType.Definition;

                parameter = Context.CreateTop(Function);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }

                // $skip
                parameter = Context.CreateSkip(Function);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }

                // $search
                parameter = Context.CreateSearch(Function);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }

                // $filter
                parameter = Context.CreateFilter(Function);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }

                // $count
                parameter = Context.CreateCount(Function);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }

                // $order
                parameter = Context.CreateOrderBy(Function, structuredType);
                if (parameter != null)
                {
                    operation.Parameters.Add(parameter);
                }
            }

            // $select
            parameter = Context.CreateSelect(Function, structuredType);
            if (parameter != null)
            {
                operation.Parameters.Add(parameter);
            }

            // $expand
            parameter = Context.CreateExpand(Function, structuredType);
            if (parameter != null)
            {
                operation.Parameters.Add(parameter);
            }
        }
    }
}
