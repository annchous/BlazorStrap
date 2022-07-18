﻿## FluentValidator
#### Component \<FluentValidator\>
[BlazorStrap.Extensions.FluentValidation](https://www.nuget.org/packages/BlazorStrap.Extensions.FluentValidation/)    


{{sample=V4/Extensions/FluentValidation}}

#### Component \<FluentValidatorInjectable\>

As an alternative to using `FluentValidator`, you can use the `FluentValidatorInjectable` component.  This works
exactly the same way, except that you can define any number of constructor parameters that will be automatically
injected from your DI services.  You can optionally register your validator with your DI as a service, or it will
just construct one as needed.

You can optionally specify an additional `Context` object that will also be passed in to the constructor of your
validator (only as a new instance, not as an existing service), which can be useful to provide some state from
your containing component.

{{sample=V4/Extensions/FluentValidationInjectable}}
