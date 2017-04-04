#### require()

The `require` method should be familiar to anyone that has worked with **[node.js](https://nodejs.org/en/)**. Essentially, `require` allows for modularization of js code. It is not builtin to the language, it is builtin to the platform.

##### Example

We'll start with a basic example of a math module we wish to reuse across many scripts:

```
function fibonacci(num) {
	if (num <= 1) {
		return 1;
	}

	return fibonacci(num - 1) + fibonacci(num - 2);
}

function randRange(a, b) {
	return a + Math.floor((b - a) * Math.random());
}
```

Ignoring, for the moment, how poorly a recursive fibonacci function would perform, particularly one that is intepreted _on top of_ a managed language-- let's focus our attention on how to share these bits of code, so that we don't have to sprinkle these functions across all of our scripts.

We accomplish this using the `module.exports` object.

```
module.exports = {
	fib: fibonacci,
	random: {
		range: randRange
	}
};
```

Whatever is assigned to `module.exports` will be returned by a `require()`.

Next, we place this script in our `Resources` folder (look below if you don't want to use resources). When `require` is called, this entire script will be executed and that object will be returned.

With the script in the right place, we can now try this out in our REPL:

```
var math = require('math');

math.fib(5)
 => 8

math.random.range(2, 10)
 => 3
```

Magic.

##### Caveats

In order to support this easily, we've reserved the variable `module`. We've done our best to insulate your code, but `require` will muck up your code if you name another variable `module`. So don't.

##### Extending Require
