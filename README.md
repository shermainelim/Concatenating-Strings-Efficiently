# Concatenating-Strings-Efficiently
String Concatenation Versus String Builder

# The Problem We're Trying To Avoid

Building a load of strings (usually of increasing size) which are never used apart from contributing to the creation of other strings. 
Here's an example:
![image](https://user-images.githubusercontent.com/65886071/116094498-2b08a400-a6da-11eb-9d9f-06ef5e9c81cb.png)

On my gaming laptop (so it is quite fast), that takes nearly 10 seconds. Double the number of iterations, and it will take over a minute. The problem is that strings are immutable - just because we're using "+=" here doesn't mean the runtime actually appends to the end of the existing string. In fact, x += "!"; is absolutely equivalent to x = x+"!";. The concatenation here is creating an entirely new string, allocating enough memory for everything, copying all the data from the existing value of x and then copying the data from the string being appended ("!"). As the string grows, the amount of data it has to copy each time grows too, which is why the time taken didn't just double when I doubled the number of iterations.

This is clearly inefficient. If someone asked you to add something to a shopping list, you wouldn't write a new copy of the shopping list first, would you? Enter StringBuilder...

# The StringBuilder Solution
Much faster:
![image](https://user-images.githubusercontent.com/65886071/116094885-7e7af200-a6da-11eb-9815-d02c27acac98.png)

Changing to a million iterations (i.e. ten times what the first program took nearly ten seconds to do), it takes about 30-40ms. The time taken is roughly linear in the number of iterations (i.e. double the iterations and it takes twice as long). It does this by avoiding unnecessary copying - only the data we're actually appending gets copied. 

StringBuilder maintains an internal buffer and appends to that, only copying its buffer when there isn't room for any more data. (In fact, the internal buffer is just a string - strings are immutable from a public interface perspective, but not from within the mscorlib assembly.) We could make the above code even more efficient by passing the final size of the string (which we happen to know in this case) to the constructor of StringBuilder to make it use a buffer of the right size to start with - then there'd be no unnecessary copying at all. Unless you're in a situation where you have that information readily to hand though, it's usually not worth worrying about - StringBuilder doubles its buffer size when it runs out of room, so it doesn't end up copying the data very many times anyway.

# Speed Comparison:
![image](https://user-images.githubusercontent.com/65886071/116094058-cc432a80-a6d9-11eb-80d7-8e2d1094d1e2.png)

# Notes
The important difference between this example and the previous one is that we can easily present all the strings which need to be concatenated together in one call to String.Concat. That means that no intermediate strings are needed. StringBuilder is efficient in the first example because it acts as a container for the intermediate result without having to copy that result each time - when there's no intermediate result anyway, it has no advantage.

# Rules Of Thumb
So, when should you use StringBuilder, and when should you use the string concatenation operators?

- Definitely use StringBuilder when you're concatenating in a non-trivial loop - especially if you don't know for sure (at compile time) how many iterations you'll make through the loop. For example, reading a file a character at a time, building up a string as you go using the += operator is potentially performance suicide.

- Definitely use the concatenation operator when you can (readably) specify everything which needs to be concatenated in one statement. (If you have an array of things to concatenate, consider calling String.Concat explicitly - or String.Join if you need a delimiter.)

- Don't be afraid to break literals up into several concatenated bits - the result will be the same. You can aid readability by breaking a long literal into several lines, for instance, with no harm to performance.

- If you need the intermediate results of the concatenation for something other than feeding the next iteration of concatenation, StringBuilder isn't going to help you. For instance, if you build up a full name from a first name and a last name, and then add a third piece of information (the nickname, maybe) to the end, you'll only benefit from using StringBuilder if you don't need the (first name + last name) string for other purpose (as we do in the example which creates a Person object).

- If you just have a few concatenations to do, and you really want to do them in separate statements, it doesn't really matter which way you go. Which way is more efficient will depend on the number of concatenations the sizes of string involved, and what order they're concatenated in. If you really believe that piece of code to be a performance bottleneck, profile or benchmark it both ways.

Reference:
https://jonskeet.uk/csharp/stringbuilder.html 
