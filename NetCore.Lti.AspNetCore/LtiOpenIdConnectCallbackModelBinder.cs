using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NetCore.Lti.AspNetCore;

public class LtiOpenIdCallbackLaunchModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var form = bindingContext.HttpContext.Request.Form;
        var launchRequest = new LtiOpenIdConnectCallback(
            form["authenticity_token"],
            form["id_token"],
            form["state"],
            form["lti_storage_target"]
        );

        bindingContext.Result = ModelBindingResult.Success(launchRequest);

        return Task.CompletedTask;
    }
}