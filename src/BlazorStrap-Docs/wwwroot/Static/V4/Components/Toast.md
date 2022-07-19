﻿## Toast

#### Component \<BSToast\>
See [shared](layout/shared) for additional parameters    

:::

| Parameter    | Type           | Valid          | Remarks/Output | 
|--------------|----------------|----------------|----------------|
| Color        | Enum           | BSColor        | `.bg-[]`       | {.table-striped .p-2}
| IsActive     | bool           | true/false     | `.active`      |
| ContentClass | string         | string         |                |
| HeaderClass  | string         | string         |                |
| Content      | RenderFragment | RenderFragment | Nested Content |
| Header       | RenderFragment | RenderFragment | Nested Content |
| OnClick      | EventCallback  | MouseEventArgs |                |

:::

### Example

{{sample=V4/Components/Toast/Toast1}}

### Without header
{{sample=V4/Components/Toast/Toast2}}


### Toaster Example
#### Component \<BSToaster\>
| Parameter    | Type    | Valid            | Remarks/Output                  | 
|--------------|---------|------------------|---------------------------------|
| WrapperClass | string  | css class string | Adds your class(es) to wrapper  | {.table-striped .p-2}
| WrapperStyle | string? | style string     | Adds your styles to wrapper     | 

<BSToaster/> should be placed before you `@Body` in your layout. Exact placement depends on your requirements for where you want the toasts to show up. 

{{sample=V4/Components/Toast/Toast3}}
