mergeInto(LibraryManager.library, {
  SaveAndCleanURL: function () {
    let url = new URL(window.location.href);

    let account = url.searchParams.get("account");
    let username = url.searchParams.get("username");
    let profilePicture = url.searchParams.get("profilePicture");

    if (account) {
      localStorage.setItem("unity-account", account);
    }
    if (username) {
      localStorage.setItem("unity-username", username);
    }
    if (profilePicture) {
      localStorage.setItem("unity-profilePicture", profilePicture);
      console.log(profilePicture);
    }

    url.searchParams.delete("account");
    url.searchParams.delete("username");
    url.searchParams.delete("profilePicture");

    // Replace the current URL with the modified one (without reloading)
    window.history.replaceState({}, document.title, url.toString());
  },

  GetAccountAndUsername: function () {
    let account = localStorage.getItem("unity-account");
    let username = localStorage.getItem("unity-username");
    let profilePicture = localStorage.getItem("unity-profilePicture");

    if (account && username) {
      const returnStr = JSON.stringify({ account, username, profilePicture });
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
