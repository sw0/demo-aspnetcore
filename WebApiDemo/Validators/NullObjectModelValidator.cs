using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Validators
{
    /// <summary>
    /// please refer to https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-3.1
    /// </summary>
    public class NullObjectModelValidator : IObjectModelValidator
    {
        public void Validate(ActionContext actionContext,
            ValidationStateDictionary validationState, string prefix, object model)
        {

        }
    }
}
