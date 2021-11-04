# Remote.Linq.ExpressionBatch

A demonstration of sending multiple Remote.Linq expressions per single network request. It is useful in case you need to execute multiple expressions per single transaction and you cannot share a server's RDBMS transaction across few network requests (that's never a good idea, I guess). It is based on [Remote.Linq](https://github.com/6bee/Remote.Linq) (v.7.0) and used protobuf for serialization.

I suggest starting the study of the code from:

1. Unit tests.
2. Conditional compilation symbols in `Program.cs` in `Client.csproj` and `Server.csproj`.

Many thanks to the author of Remote.Linq (6bee) for the library and answers to my numerous questions.
