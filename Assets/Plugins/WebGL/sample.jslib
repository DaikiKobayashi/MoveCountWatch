mergeInto(LibraryManager.library, {
  GetCurrentPosition: function () {
    navigator.geolocation.getCurrentPosition(
      function(position) {
        xy = position.coords.latitude + "," + position.coords.longitude;   
	    SendMessage('LocationWebEventReceiver', 'GetCurrentPositionCallBack', xy); 
      }
    );
  },
});

mergeInto(LibraryManager.library, {
 
  ConsoleLog: function(message) {
    console.log(Pointer_stringify(message));
  }
 
});