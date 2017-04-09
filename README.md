#### Jint-Unity

`Jint-Unity` is a fork of **[Jint](https://github.com/sebastienros/jint)**: a Javscript _interpreter_ for .NET. Jint does not JIT. This is important. This fork provides a few things that Jint proper does not:

* Compliant with the **.NET 3.5 Subset** that Unity uses.
* Fixes for **iOS compatibility**: yes, this runs on iOS!
* **Deep integration** with Unity: APIs for days!
* An awesome **Unity scene query language** aptly named UQL.
* Useful platform APIs:
	* Modularize with **[require()](Documentation/require.md)**!
	* Plugin to DI frameworks with **[inject()](Documentation/inject.md)**!
* Lots of Unity examples-- including a **full REPL**.

![Example](Documentation/example.png)

##### Build Process

All Jint projects can be found in `Master.sln`, which can be built like any Visual Studio Solution-- via Visual Studio, `msbuild` (.NET), or `xbuild` (Mono). This solution outputs a set of dlls.

We use **[gradle](http://gradle.org)** to orchestrate this process. Individual `gradle` tasks are outlined below.

###### buildAll

Builds all `Jint-Unity` projects and copies all project dlls into each example project.

_Properties_
* **configuration** - Defaults to `Debug`. Specifies which build configuration to copy. Eg - `gradle buildAll -Pconfiguration=Release` will copy release dlls into projects..

##### Roadmap

Our roadmap can be found **[here](Documentation/roadmap.md)**.