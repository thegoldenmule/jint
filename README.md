#### Jint-Unity

`Jint-Unity` is a fork of **[Jint](https://github.com/sebastienros/jint)**: a Javscript _interpreter_ for .NET. Jint does not JIT. This is important. This fork provides a few things that Jint proper does not:

* Compliant with the **.NET 3.5 Subset** that Unity uses.
* Fixes for **iOS compatibility**: yes, this runs on iOS!
* **Deep integration** with Unity: APIs for days!
* An awesome **Unity scene query language** aptly named UQL.
* Useful platform APIs!
	* Modularize with **[require()](docs/require.md)**!
	* Plugin to DI with **[inject()](docs/inject.md)**!
* Lots of Unity examples-- including a **full REPL**.

![Example](docs/example.png)