import {
  signOut,
  profilePageRoute,
  displayName,
  bookingListPageRoute,
} from "./site.js";

let signInUpBtn = document.getElementById("signInUpBtn");
let userDropdown = document.getElementById("userDropdown");
let currentUser = localStorage.getItem("currentUser");
if (currentUser) {
  signInUpBtn.style.display = "none";
  userDropdown.style.display = "block";
} else {
  signInUpBtn.style.display = "block";
  userDropdown.style.display = "none";
}

let signOutBtn = document.getElementById("signOutBtn");
if (signOutBtn) {
  signOutBtn.addEventListener("click", () => {
    signOut();
  });
}

let profileBtn = document.getElementById("profileBtn");
if (profileBtn) {
  profileBtn.addEventListener("click", () => {
    profilePageRoute();
  });
}

let bookingTour = document.getElementById("bookingTour");
if (bookingTour) {
  bookingTour.addEventListener("click", () => {
    bookingListPageRoute();
  });
}

displayName();
