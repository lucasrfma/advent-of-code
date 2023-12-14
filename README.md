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

At first I thought just keeping basically everything the same with some small changes would be enough, and the logic itself was valid... But when I tried to run the real input, my PC didn't have enough memory to deal with it...

Then I fixed it so it would take a lot less memory and it was able to run, but way too slow.

I decided to see the difference making the main processing use .AsParallel() in the linq expression that set off the most intensive part of the code would achieve, and it got faster, but still very slow in absolute terms: 8 minutes and 3 seconds to finish running.

But while I was implementing these first fixes I had an idea of how to make it much more efficient. After getting stuck on many silly bugs, it worked: 16ms run time, fast enough that .ToParallel becomes a waste of resources.

It was really fun and I think I learned a lot doing this =)

Also decided to change project structure:
Now it is a single dotnet project. No more repeated main functions, and easy to test performance for each day now. (Kinda messed up though, with every day having a Program class...)

## Day Six
The day I used Bhaskara's Formula after graduating high school

## Day Seven
JJJJJ eluded me for the longest time....

The most obvious (and likely better) way of doing this was by doing double comparison between hands (first of 'sets' like pairs, full houses and such) then by individual card.
Instead I decided to try and synthesize a single number for each hand that reflected its value by setting a huge number for each type of set of cards, and a much smaller value for each individual card, multiplied according to
its position in the hand in a way that a card positioned to the left is always more valuable than a card to the right to reflect the ordered nature of the individual card comparison stage.

## Day Eight
I think every time after doing one of these challenges I end up concluding that I should not anchor myself too much to part one's
solution when moving on to part two.... to do just that again on the next one. 