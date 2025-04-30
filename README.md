# Spring is Coming 2: Electric Boogaloo

By Ilya V., Uri M.

Date: 4/29/25

<https://manyak404.github.io/BTD_Spring/>

**CONTROLS:**

**Click “play” to play the game. Click “leave” to leave the game back to the play menu. Click the monkey button in the bottom right to place a tower if you have enough money to buy the tower.**

# Balloons

- Balloons are “enemies” they travel along the brown winding road
- Balloons have 4 levels. The from most to least dangerous: Yellow, green, blue and red. The most dangerous levels travel the fastest and deal the most damage to your health (4 HP). If one of your towers shoots a balloon it decreases in level and continues moving. If you shoot a red balloon no levels of balloons remain, it disappears.
- Every time a tower pops a balloon you get 1 credit.

# Towers

- Towers automatically target the first balloon in line. This is calculated by tracking the distance each balloon has travelled and targeting the longest travelled balloon. A tower will only target a balloon that is not currently targeted by another tower.
- Towers can be bought for 50 credits. Their placement is limited to areas off path and UI elements. A cursor changes color depending on the validity of placement.

# Darts

- Darts will fly towards its targeted balloon. It has a collider and trigger effect.
- On impact with ANY balloon, the dart will pop said balloon.

All in all, we learned how to import json files, implement a cursor placement mechanic, and connect two scenes together. It took us about 20 hours across two weeks to complete this project. The most challenging part was creating different balloon types in a json file, loading the file, and turning the file into data/scriptable_object for each color/type of balloon.
