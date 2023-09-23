# Project Arrow Ace

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

### Student Info

-   Name: Owen Gebhardt
-   Section: 04

## Game Design

-   Camera Orientation: Top-down
-   Camera Movement: Stationary camera, moving background
-   Player Health: Healthbar
-   End Condition: Death!
-   Scoring: Points are awarded for skilled play:
    - Hitting an enemy will earn you 10 points
    - Destroying an enemy will earn you 100 points
    - Hitting an enemy in its fuselage instead of the wings or using an enemy's projectile against itself (discussed below) will each double the points that enemy will give. These multipliers can stack so you can get the most out of an enemy with some well-placed shots!

### Game Description

In _Arrow Ace_, you play an ace pilot/archer who wields a versatile bow that can shoot many kinds of projectiles. After stealing designs for a powerful new weapon at an enemy research facility, aerial enemy backup forces have arrived to prevent you from bringing the secrets back. Cut through them and help your side win the arms race!

You'll engage in combat with two different kinds of aerial foes:
    - Toothy melee jets that will charge at you if you get directly in front of them
    - Bulky spear-throwing planes that will throw spears out in front of them and take twice as much damage before dying

### Controls

-   Movement
    -   Up: W / Up Arrow Key
    -   Down: S / Down Arrow Key
    -   Left: A / Left Arrow Key
    -   Right: D / Right Arrow Key
-   Fire Left: J
-   Fire Right: L

## Your Additions

-   All art done by me in a pixellated style
    -   Adherence to the pixel grid: Objects will stay aligned to the grid of pixels without blurring
-   Kinematic Movement: The player and enemies will move with smooth acceleration when speeding up and dampening when slowing down, and will always reach a consistent top speed. The top speed, along with the time it takes to speed up and slow down, can all be adjusted manually.
-   Advanced Collisions: Each object (player, enemy, projectile) can have a number of different Axis-Aligned Bounding Boxes that can collide with each other.
    -   Damaging collisions are always between the damaging object's hitbox and the receiving object's hurtbox, which can be explicitly defined.
    -   Physics bounding boxes are used to determine when the (originally also planned for making objects solid, but this never made it into the game)
-   Projectile Stealing: If you get an enemy's projectile to hits the direct center of your plane, you grab it instead of letting it deal damage. The next time you fire a projectile, you'll launch that stolen projectile instead of a normal arrow.
-   Seamless Scrolling Background with Parallax: The background moves slightly up and down with your plane, and speeds up or slows down its scrolling depending on what direction you're moving left and right.
-   Weighted Randomized Enemy Encounters: You're twice as likely to see a charging jet than a spear plane. They also appear in random places on the screen at random times.
-   The Player cannot move offscreen.

## Sources

-   All art assets were drawn by me in GIMP.
-   Fonts:
    -   Manual Display from DaFont (100% Free) https://www.dafont.com/manualdisplay.font
    -   VCR OSD Mono from DaFont (100% Free) https://www.dafont.com/vcr-osd-mono.font

## Known Issues

-   When played in WebGL, the UI displays incorrectly, especially when viewed in fullscreen.
-   When viewed in fullscreen, the background parallax also stops being seamless.
-   Enemies are supposed to glide smoothly to a halt when reaching their spot on the left side of the screen, but instead stop suddenly.
-   [FIXED!] Your plane can move slightly off the edge of the screen, though not fully. This will be fixed after getting collision set up and know where your plane's hitboxes will go.
    -   [FIXED!] Because I changed how collision is handled and haven't yet implemented screen edge collision, you can go off the screen as far as you want. This will be fixed soon!
-   [FIXED!] The whole canvas appears blurry in WebGL. This will be fixed by increasing the resolution and scaling the canvas accordingly.

### Requirements not completed

- Not sure if this is the section to write this or not, but I'm submitting this using my two-day Grace Period as outlined in the course syllabus.
