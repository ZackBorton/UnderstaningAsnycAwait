# UnderstandingAsyncAwait
Examples and info around how the Task-based Asynchronous Pattern is implemented in C# under the covers

You can use [ILSpy](https://github.com/icsharpcode/ILSpy) to show the generated State Machine code of async methods to get a better understanding of what the code is doing

```
ilspycmd -p Your.dll  -o /Your/Output/Directory
```
See AsyncAwaitExamples.cs in the logic project to view the info around the async await implementation 
