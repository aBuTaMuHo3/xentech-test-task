# web_unity_sandbox
Test project inside an existing exercise engine environment

## Goal
In the end ideally there should be a playable version of "FlashGlance" that has similar look to our current mobile version of the exercise (the layout needs to be adjusted to landscape instead of portrait):
<div align="center">
    <img src="/Screenshots/1.png" width="45%"</img> 
    <img src="/Screenshots/2.png" width="45%"</img> 
</div>

The mechanism of this exercise is quite simple, the component on top of the screen shows numbers and the one shown in the center needs to be selected in the part below. By clicking on the correct number the selected item disappears and the exercise proceeds. When selecting an incorrect number in this implementation nothing needs to happen.

In this test the main focus is on creating the view on top of the already existing implementation of the exercise engine. That means the view should react on the main states of the exercise cycle.

## Basic flow
For now it should be sufficient to have a rough picture on how our exercises work. We have an exercise engine that is mainly used in the mobile app, plan is to unify the usage to reach a one product experience in web and app. It mainly consists of a Statemachine, which builds the base of our ExerciseController, and an ExerciseModel which is responsible for creating the data needed to show the exercise. The controller handles the different phases of one round of the exercise and communicates between view and model.

To use these parts in the Unity project the classes are connected by the ExerciseControllerProxy and the ExerciseViewAdapter and use signals for communication. The unity views are using mediators to communicate with the controller.

Example of an existing exercise is included -> Memoflow.

## Project structure
* Exercise engine can be found in /Assets/External/ExerciseEngine/
* Non view classes of the exercises are located in /Assets/External/ExerciseTemplate/
* Unity view scripts are in /Assets/Externals/WebExercises/Assets/WebExercises/Exercises/
* Assets, the scene, prefabs, graphics, init etc. are located in /Assets/Modules/Exercises/

## Implementation guidance
* Classes to look at and work with:
    * FlashGlanceViewMediator
    * FlashGlanceView (this actually is the one where you will add your view implementations)
    * FlashGlanceRoundDataVO
    * FlashGlanceRoundItemVO
    * FlashGlanceModel (if interested)
    
If you want to have a look at Memoflow and prefer to mediate the single view components instead of having them all located inside FlashGlanceView feel free to do so. Though then you would need to add the scripts for that including new mediators.
    
The classes needed have some further guidance comments in it, this should help to implement the necessary stuff to get the exercise running.
    
If you want to test the project you need to run a local webserver to have a simulated backend. The project as it is initially should run and show an empty view with time running in the top bar(HUD) and score counting up. (Every second the correct answer is triggered for testing if the whole setup is correct, see comments in FlashGlanceView)

    run "python -m SimpleHTTPServer 8000" in the project's root folder
    the port can be configured with the scriptable object at /Assets/Resources/AppSettings

Run the project in the editor with Exercises/Publish for Editor

    The default exercise is configured in /Configs/AppConfig.json
    If you want to see memoflow change "defaultExercise" to "Memoflow"

The top component (we call it Slider) should display the currently searched item and the next 2 upcoming ones. Basic implementation would be in a horizontal line, doing it like shown in the screenshot is the desired output but depends on available time. The searched item should look different then the upcoming ones (like on the screenshot, Pngs are provided in /Assets/Modules/Exercises/FlashGlance/Graphics, they might not have perfect resolution but is fine for now). Animationwise the new upcoming item should fade in while sliding in from the right side, the last searched item should slide out to the left and fade out. Every item moves one position from right to left. (respectively on a curve if you implement that solution)

For the bottom part the items should be arranged in a grid, the size should be taken from the FlashGlanceRoundDataVO (It is 9 x 4 in each level and doesnt change for now). The grid should spread equally over the available space (defined by the canvas in the unity scene, size is fixed). Nice to have is some fading in and out when showing and hiding the items. Reusing elements is always welcome.
