import {
  blockAccessRole,
  signOut,
  displayName,
  profilePageRoute,
} from "../site.js";
window.onload = () => {
  blockAccessRole("ADMIN");
};

document.getElementById("signOutBtn").addEventListener("click", () => {
  signOut();
});

document.getElementById("profileBtn").addEventListener("click", () => {
  profilePageRoute();
});

displayName();
