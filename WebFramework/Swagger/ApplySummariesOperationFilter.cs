using Common.Utilities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Pluralize.NET;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;

namespace WebFramework.Swagger
{
    public class ApplySummariesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor == null) return;

            var pluralizer = new Pluralizer();

            var actionName = controllerActionDescriptor.ActionName;
            var singularizeName = pluralizer.Singularize(controllerActionDescriptor.ControllerName);
            var pluralizeName = pluralizer.Pluralize(singularizeName);

            string Part2Name = "";
            if (controllerActionDescriptor.ActionName.Split('_').Length == 2)
                Part2Name = controllerActionDescriptor.ActionName.Split('_')[1];

            var parameterCount = operation.Parameters.Where(p => p.Name != "version" && p.Name != "api-version").Count();

            if (IsGetAllAction())
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Returns all {pluralizeName}";
            }
            else if (IsActionName("Post", "Create", "Add", "Set"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Creates a {singularizeName}";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
            else if (IsActionName("Read", "Get"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Retrieves a {singularizeName} by unique id";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"a unique id for the {singularizeName}";
            }
            else if (IsActionName("Put", "Edit", "Update"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Updates a {singularizeName} by unique id";

                //if (!operation.Parameters[0].Description.HasValue())
                //    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A {singularizeName} representation";
            }
            else if (IsActionName("Delete", "Remove"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Deletes a {singularizeName} by unique id";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A unique id for the {singularizeName}";
            }

            else if (IsActionNamePart2("Post", "Create", "Add", "Set"))
            {
                if (!operation.Summary.HasValue())
                    operation.Summary = $"Set a {Part2Name} for {singularizeName}";

                if (!operation.Parameters[0].Description.HasValue())
                    operation.Parameters[0].Description = $"A {singularizeName} representation";
            }

            #region Local Functions
            bool IsGetAllAction()
            {
                foreach (var name in new[] { "Get", "Read", "Select" })
                {
                    if ((actionName.Split("Override")[0].Equals(name, StringComparison.OrdinalIgnoreCase) && parameterCount == 0) ||
                        actionName.Split("Override")[0].Equals($"{name}All", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}{pluralizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}All{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}All{pluralizeName}", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsActionName(params string[] names)
            {
                foreach (var name in names)
                {
                    if (actionName.Split("Override")[0].Equals(name, StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("Override")[0].Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }

            bool IsActionNamePart2(params string[] names)
            {
                foreach (var name in names)
                {
                    if (actionName.Split("_")[0].Equals(name, StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("_")[0].Equals($"{name}ById", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("_")[0].Equals($"{name}{singularizeName}", StringComparison.OrdinalIgnoreCase) ||
                        actionName.Split("_")[0].Equals($"{name}{singularizeName}ById", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
                return false;
            }
            #endregion
        }
    }
}
