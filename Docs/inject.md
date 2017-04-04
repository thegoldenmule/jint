#### inject()

The `inject()` method is a very basic dependency injection hook. It allows `jint` to plug into dependency injection frameworks.

##### IScriptDependencyResolver

When a `UnityScriptingHost` is created, an `IScriptDependencyResolver` implementation must be passed to it. This interface defines a single method:

```
/// <summary>
/// Resolves a dependency by name.
/// </summary>
/// <param name="name"></param>
/// <returns></returns>
object Resolve(string name);
```

When a script calls `inject()`, it calls the host's `IScriptDependencyResolver` implementation, which can resolve the request. A few implementations are provided for more popular DI frameworks.

##### Ninject

`Jint.Ninject` comes with `NinjectScriptDependencyResolver` and `NinjectScriptDependencyAttribute`. The former resolves dependencies using a provided `IKernel`. The latter can be placed on classes for direct lookup.

```
[NinjectScriptDependency("Players")]
public class PlayerManager
{
	// elided
}
```

Then in our script:

```
var players = inject('Players');
```

A `PlayerManager` will be resolved via the Ninject kernel and passed through to the script.