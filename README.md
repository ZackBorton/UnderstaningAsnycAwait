# UnderstandingAsyncAwait
Examples and info around how the Task-based Asynchronous Pattern is implemented in C# under the covers

Uses [ILSpy](https://github.com/icsharpcode/ILSpy) to show the generated State Machine code of async methods

```
ilspycmd -p Your.dll  - o /Your/Output/Directory
```
