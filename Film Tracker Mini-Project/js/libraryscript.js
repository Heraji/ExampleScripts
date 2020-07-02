var filmnames = [];
var filmratings = [];
var libnum = 0;
var arrayNum = 0;

var table = document.getElementById("libtable");

add_film = function (rating, input, imgNum) { // Function for adding films to the library list


    // Checks if localStorage is not null. Else do not attempt load from localstorage
       if (localStorage.hasOwnProperty('LibList') &&
           typeof localStorage.LibList !== 'undefined' &&
           localStorage.LibList !== '') {

           // Load arrays from local storage and assign to local arrays
           filmnames = JSON.parse(localStorage.getItem("LibList"));
           filmratings = JSON.parse(localStorage.getItem("Ratings"));
           libnum = filmnames.length;
           // console.log("Array is NOT null!") // Used for debugging

        } else {
           console.log("Array is null!") // Used for debugging
        }

        // console.log(input);

        // Add data to arrays
        filmratings[libnum] = rating;
        filmnames[libnum] = input;
        libnum++; // Count up (used for indexing)

    // Save arrays and "index" number in localStorage
    localStorage.setItem("LibraryNum", libnum);
    localStorage.setItem("LibList", JSON.stringify(filmnames));
    localStorage.setItem("Ratings", JSON.stringify(filmratings));

    // Set opacity of clicked image
    var imgPoster = document.getElementById('image' + imgNum);
    imgPoster.style.opacity = "0.4";

}

loadlist = function () { // Function for loading films to the library list, from local browser storage

    // Load arrays from localstorage
    var LibraryList = JSON.parse(localStorage.getItem("LibList"));
    var RatingsList = JSON.parse(localStorage.getItem("Ratings"));

    // Assign loaded arrays
    filmnames = LibraryList;
    filmratings = RatingsList;

    if (LibraryList.length != null) { // If arrays is empty, do nothing

        // for-loop for going through all elements in array and run the function "createTable" for each of them
        for (i = 0; i < LibraryList.length; i++) {

            arrayNum++;
            createTable(arrayNum, [[LibraryList[i], "IMDb Rating: " + RatingsList[i], "Remove" ]]);
        }
    }

}


removeListItem = function (arrayID) {   // Function for removing items/cells from list (arrayID is index number for specific film in array)

    document.getElementById('filmRow' + arrayID).deleteRow(0);  // Removes the HTML element/row from the table

    filmnames.splice(arrayID-1,1); // Removes specific element from array
    filmratings.splice(arrayID-1,1); // Removes specific element from array

    console.log(filmnames); // Used for debugging

    // Rewrite "database"
    localStorage.setItem("LibList", JSON.stringify(filmnames));
    localStorage.setItem("Ratings", JSON.stringify(filmratings));

    location.reload(); // Refresh page
}


createTable = function (num, Data) {    // Function used for creating tables

    var table = document.createElement('table'); // Creates table element
    var tableContainer = document.createElement('tbody'); // Creates tbody element

    Data.forEach(function(rowData) {    // For each row
        var row = document.createElement('tr'); // variable row = tr element

        rowData.forEach(function(cellData) {    // for each cell in a row
            var cell = document.createElement('td');    // variable cell = td element
            cell.appendChild(document.createTextNode(cellData));    // Create textnode in cell
            row.appendChild(cell);  // Append the new cells to a row
        });

        tableContainer.appendChild(row); // Append the new rows to the container
    });

    table.appendChild(tableContainer);  // Append container to table
    document.body.appendChild(table);   //

    table.id = "filmRow" + num; // Assign id to each row
    table.className = "filmTable"; // Assign class to table

    // Assign onclick function to each row (calling removeListItem() function)
    document.getElementById('filmRow' + num).onclick = function () {
        removeListItem(num);
    }
}

loginFunc = function (textInput) {  // Takes string input from text field

    console.log("LoginID: " + textInput);   // Paste string in console
    alert("Login ID: " + textInput + " - Login feature not implemented yet!"); // Show alert message

}

saveToCloud = function () {
    alert("Not implemented yet!"); // Show alert message
}

loadFromCloud = function () {
    alert("Not implemented yet!"); // Show alert message
}

// document.addEventListener('DOMContentLoaded', tableCreate());