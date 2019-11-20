# UnderstandingAsyncAwait
Examples and info around how the Task-based Asynchronous Pattern is implemented in C# under the covers

You can view the state machine info by looking at the IL code.
* If using Rider go to tools and IL Viewer
* You can also use [ILSpy](https://github.com/icsharpcode/ILSpy)
```
ilspycmd -p -il Your.dll  -o /Your/Output/Directory
```
See AsyncAwaitExamples.cs in the logic project to view the info around the async await implementation 
