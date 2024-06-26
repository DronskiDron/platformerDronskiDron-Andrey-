mergeInto(LibraryManager.library, {
    Hello: function () {
        window.alert("Hello, world!");
        console.log("Hello, Test!");
    },

    GetYandexPlayerData: function () {
        myGameInstance.SendMessage('Yandex', 'SetName', player.getName());
        myGameInstance.SendMessage('Yandex', 'SetPhoto', player.getPhoto("medium"));
    },
});