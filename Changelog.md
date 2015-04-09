# wwDotnetBridge Change Log
--------------------------
## Version 5.72
*April 9th, 2015*

* **Address concerns for running on XP**<br/>
Change compiler target using Visual Studio 2013 runtimes for ClrHost.dll.
This fixes potential problems on Windows XP.

* **Add support for char parameters and results**<br/>
char parameters can now be set with ComValue::SetChar() by passing in
either a string value or number. You can also use loBridge.ConvertToDotnetValue()
to create the ComValue structure. char results are converted to string when
returned from .NET method calls.

* **ComValue SetSingle()**<br/>
Allows assigning a .NET Single value from a FoxPro number to be used the 
dynamically invoked methods.

* **ComValue.SetChar() with characters or numbers**
.NET char parameters can now be set with ComValue::SetChar() by passing in either a string value or number. You can also use loBridge.ConvertToDotnetValue(val,"char")to create the ComValue structure. char results are converted to string when returned from .NET method calls. 

* **ComValue.SetValueFromEnum()**<br/>
Allows you to set an Enum Value from a type and constant name and 
assign it to ComValue. Allows passing Enum parameters to .NET which
otherwise is not possible as enums are static values. 

* **ComValue.SetValueFromInvokeStaticMethod**<br/>
You can now set a ComValue from the result of a static method
invocation. Useful if the result type of a static method returns a 
result that is not accessible in FoxPro - like a struct or generic
object. You can then use GetProperty/GetPropertyEx to access
values of that object.

* **Fixed error handling to be more consistent**<br/>
Changed the error handling behavior of all method that capture
errors to return the base error message. Made consistent across
wwDotnetBridge calls.

* **Add support for nested member strings to GetProperty/SetProperty/InvokeMethod**<br/>
You can now use . syntax in string based property/method names for all indirect methods, negating the need to use methods like GetPropertyEx or SetPropertyEx. You can now specify member names like "Address.Street" or "Error.ToHtml" directly in the various base methods. Use this feature to get around intermediary types that FoxPro can't access (Value types, generics etc.).

* **Better Support for Reference Parameters**<br/>
You can now pass parameters by reference using the indirect InvokeMethod/InvokeStaticMethod methods by passing parameters using a ComValue structure. The .Value property that holds the inbound parameter also receives any changed values that are modified by the method call.

* **Fix Null Value Handling**<br/>
Fixed bug with NULL values passed to wwDotnetBridge calls. COM Interop changes Fox NULLs to DbNulls which failed. Indirect methods now translate DbNull values to raw .NET nulls when passed. You can still pass DbNull with ComValue.SetDbNull() if needed.

## Version 5.67
*Nov. 8, 2013*

* **ComArray::AddItem() Auto Type Conversion**<br/>
When you add items to an array, AddItem() now automatically
recognizes ComValue and ComArray structures and performs
auto conversions of unsupported FoxPro types (like Guids, Longs etc.)

* **ComArray::Item() AutoType Conversions**<br/>
When you retrieve items out of the Item() method, values are 
automatically fixed up with type conversions for unssupported
FoxPro types. Arrays come back as ComArray, unsupported types
are converted to ComValue and some auto conversions like Longs
happen automatically.

* **Allow for 24 parameters in InvokeMethod()**<br/>
Due to many, many support requests with Invokemethod requirements
for an enormous amount of parameters, we bumped the supported parameter 
count to the max 24 parameters which is the maximum VFP allows 
(26 - 2 for object and method). For more parameters yet you can 
still use the InvokeMethod_ParameterArray() method. This is crazy but there you have it.

* **ComValue::SetValueFromCreateInstance()**
This method allows you to capture output from object creation and convert
it to one of the special types supported by COM value for fixups. ComValue
allows values to stay in .NET so you can access the supported special types
indirectly.

* **ComArray::FromEnumerable()**<br/>
This method allows you to capture any .NET Enumerable into a COM Array.
This includes access from arrays, List<T>, IQueryAble<T> etc. and allows
you to manipulate the resulting structure just like an array. Very useful
as many .NET components use abstract IEnumerables rather than explicit
arrays.