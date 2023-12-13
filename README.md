# advent-of-code
Trying to solve advent of code challenges

## Day One
Pretty easy, but part two took me a while because I misunderstood the problem and after that I stuck too much to what I had already done

## Day Two
The easiest so far, part two was already basically done when I finished part one.

## Day Three
Pretty interesting challenge, and took me a while to find out what I was getting wrong in part two

## Day Four
One of the easier ones so far.

## Day Five
This one was very interesting to me. Specially in the transition from part one to part two.

At first I thought just keeping basically everything the same with some small changes would be enough, and the logic itsel was valid... But when I tried to run the real input, my PC didn't have enough memory to deal with it...

Then I fixed it so it would take a lot less memory and it was able to run, but way too slow.

I decided to see the difference making the main processing use .AsParallel() would achieve, and it got faster, but still very slow in absolute terms: 8 minutes and 3 seconds to finish running.

But while I was implementing these first fixes I had an idea of how to make it much more efficient. After many silly bugs it worked: 16ms run time, fast enough that .ToParallel becomes a waste of resources.

It was really fun and I think I learned a lot doing this =)

Also decided to change project structure:
Now it is a single dotnet project. No more repeated main functions, and easy to test performance for each day now. (Kinda messed up though, with every day having a Program class...)