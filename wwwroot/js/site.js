// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Search
export function search(listSearch, searchProperty, compareValue) {
  listSearch.filter((x) => {
    return x[searchProperty] == compareValue;
  });
}

// Block Access
export function blockAccessId(Id) {
  let currentUser = localStorage.getItem("currentUser");

  if (!Id) {
    if (!currentUser) {
      location.href = "/Home";
    }
  } else {
    if (!currentUser || JSON.parse(currentUser).Id !== Id) {
      location.href = "/Home";
    }
  }
}

export function blockAccessRole(roleName) {
  let currentUser = localStorage.getItem("currentUser");
  if (!currentUser) {
    location.href = "/Home";
  }
  if (roleName) {
    let check = false;
    JSON.parse(currentUser).Roles.forEach((role) => {
      if (role.RoleName == roleName) {
        check = true;
      }
    });
    if (!check) {
      location.href = "/Home";
    }
  }
}

// Sign Out
export function signOut() {
  let currentUser = localStorage.getItem("currentUser");
  if (currentUser) {
    localStorage.removeItem("currentUser");
    location.href = "/Home";
  }
}

export function displayName() {
  let currentUser = localStorage.getItem("currentUser");
  if (currentUser) {
    document.getElementById("displayName").innerHTML =
      JSON.parse(currentUser).FullName;
  }
}

export function profilePageRoute() {
  let currentUser = localStorage.getItem("currentUser");
  if (currentUser) {
    location.href = `/User/Profile/${JSON.parse(currentUser).Id}`;
  }
}

export function bookingListPageRoute() {
  let currentUser = localStorage.getItem("currentUser");
  if (currentUser) {
    location.href = `/User/BookingList/${JSON.parse(currentUser).Id}`;
  }
}
