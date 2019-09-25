mergeInto(LibraryManager.library, {

	AppReady: function() {
		window.appReady();
	},

	EngineReady: function() {
		window.engineReady();
	},

	ExerciseReady: function() {
		window.exerciseReady();
	},

	CompleteExercise: function(data) {
		window.completeExercise(Pointer_stringify(data));
	},

	CancelExercise: function(data) {
		window.onError(Pointer_stringify(data));
	},

	ShowAlert: function(data) {
		window.onAlert(Pointer_stringify(data));
	},
});
