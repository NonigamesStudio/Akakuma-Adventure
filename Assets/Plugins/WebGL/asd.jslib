mergeInto(LibraryManager.library, {
  GameOver: function (userName, score) {
    window.dispatchReactUnityEvent("GameOver");
  },
});