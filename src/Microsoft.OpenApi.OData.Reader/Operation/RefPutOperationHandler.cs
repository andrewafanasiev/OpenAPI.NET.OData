// ------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All rights reserved.
//  Licensed under the MIT License (MIT). See LICENSE in the repo root for license information.
// ------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Microsoft.OData.Edm;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.OData.Common;
using Microsoft.OpenApi.OData.Generator;

namespace Microsoft.OpenApi.OData.Operation
{
    /// <summary>
    /// Update a navigation property ref for a navigation source.
    /// </summary>
    internal class RefPutOperationHandler : NavigationPropertyOperationHandler
    {
        /// <inheritdoc/>
        public override OperationType OperationType => OperationType.Put;

        /// <inheritdoc/>
        protected override void SetBasicInfo(OpenApiOperation operation)
        {
            // Summary
            operation.Summary = "Update the ref of navigation property " + NavigationProperty.Name + " in " + NavigationSource.Name;

            // OperationId
            if (Context.Settings.EnableOperationId)
            {
                string prefix = "UpdateRef";
                operation.OperationId = GetOperationId(prefix);
            }

            base.SetBasicInfo(operation);
        }

        /// <inheritdoc/>
        protected override void SetRequestBody(OpenApiOperation operation)
        {
            OpenApiSchema schema = new OpenApiSchema
            {
                Type = "String"
            };

            operation.RequestBody = new OpenApiRequestBody
            {
                Required = true,
                Description = "New navigation property ref values",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    {
                        Constants.ApplicationJsonMediaType, new OpenApiMediaType
                        {
                            Schema = schema
                        }
                    }
                }
            };

            base.SetRequestBody(operation);
        }

        /// <inheritdoc/>
        protected override void SetResponses(OpenApiOperation operation)
        {
            OpenApiSchema schema = null;

            if (Context.Settings.EnableDerivedTypesReferencesForResponses)
            {
                schema = EdmModelHelper.GetDerivedTypesReferenceSchema(NavigationProperty.ToEntityType(), Context.Model);
            }

            if (schema == null)
            {
                schema = new OpenApiSchema
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.Schema,
                        Id = NavigationProperty.ToEntityType().FullName()
                    }
                };
            }

            operation.Responses = new OpenApiResponses
            {
                {
                    Constants.StatusCode204,
                    new OpenApiResponse
                    {
                        Description = "Modified navigation property ref values",
                        Content = new Dictionary<string, OpenApiMediaType>
                        {
                            {Constants.ApplicationJsonMediaType, new OpenApiMediaType {Schema = schema}}
                        }
                    }
                },
                {Constants.StatusCodeDefault, Constants.StatusCodeDefault.GetResponse()}
            };

            base.SetResponses(operation);
        }

        protected override void SetSecurity(OpenApiOperation operation)
        {
            if (Restriction == null || Restriction.UpdateRestrictions == null)
            {
                return;
            }

            operation.Security = Context.CreateSecurityRequirements(Restriction.UpdateRestrictions.Permissions).ToList();
        }

        protected override void AppendCustomParameters(OpenApiOperation operation)
        {
            if (Restriction == null || Restriction.UpdateRestrictions == null)
            {
                return;
            }

            if (Restriction.UpdateRestrictions.CustomHeaders != null)
            {
                AppendCustomParameters(operation, Restriction.UpdateRestrictions.CustomHeaders, ParameterLocation.Header);
            }

            if (Restriction.UpdateRestrictions.CustomQueryOptions != null)
            {
                AppendCustomParameters(operation, Restriction.UpdateRestrictions.CustomQueryOptions, ParameterLocation.Query);
            }
        }
    }
}
