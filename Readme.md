# How to add new player

## Manual
- Tag the root of the game object with `Player`
- Add Player component to the game object
    - Reference PlayerActions in the `PLayer Input` component
    - Reference animator
    - Reference particles
    - Reference audios
    - Arrange capsule y offset as 1 and height as 2
- Copy paste the two game objects `BallKickDetector` and `BallInRangeDetector` from other player prefabs

## Script
- Put this script into the project: https://gist.github.com/frekons/2d002e560e8251e01841e2d83e8bf364
- Run the game
- Select the source game object and copy all the components from the menu item added to the GameObject menu
- Paste all components to the target game object
- Tag the target game object with `Player`

