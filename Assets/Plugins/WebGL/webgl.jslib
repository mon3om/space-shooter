mergeInto(LibraryManager.library, {
  SaveAndCleanURL: function () {
    let url = new URL(window.location.href);

    let account = url.searchParams.get("account");
    let username = url.searchParams.get("username");

    if (account) {
      localStorage.setItem("unity-account", account);
    }
    if (username) {
      localStorage.setItem("unity-username", username);
    }

    url.searchParams.delete("account");
    url.searchParams.delete("username");

    // Replace the current URL with the modified one (without reloading)
    window.history.replaceState({}, document.title, url.toString());
  },

  GetAccountAndUsername: function () {
    let account = localStorage.getItem("unity-account");
    let username = localStorage.getItem("unity-username");

    if (account && username) {
      const returnStr = JSON.stringify({ account, username });
      var bufferSize = lengthBytesUTF8(returnStr) + 1;
      var buffer = _malloc(bufferSize);
      stringToUTF8(returnStr, buffer, bufferSize);
      return buffer;
    } else {
      return null;
    }
  },

  ClearAccountAndUsername: function () {
    localStorage.removeItem("unity-account");
    localStorage.removeItem("unity-username");

    console.log("Account and Username cleared");
  },
});
