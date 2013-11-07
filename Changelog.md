#wwDotnetBridge Change Log
--------------------------


##Version 5.67
*Nov. 8, 2013


* **ComArray::AddItem() Auto Type Conversion**
When you add items to an array, AddItem() now automatically
recognizes ComValue and ComArray structures and performs
auto conversions of unsupported FoxPro types (like Guids, Longs etc.)

* **ComArray::Item() AutoType Conversions**
When you retrieve items out of the Item() method, values are 
automatically fixed up with type conversions for unssupported
FoxPro types. Arrays come back as ComArray, unsupported types
are converted to ComValue and some auto conversions like Longs
happen automatically.

* **Allow for 24 parameters in InvokeMethod()**
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

* **ComArray::FromEnumerable()**
This method allows you to capture any .NET Enumerable into a COM Array.
This includes access from arrays, List<T>, IQueryAble<T> etc. and allows
you to manipulate the resulting structure just like an array. Very useful
as many .NET components use abstract IEnumerables rather than explicit
arrays.