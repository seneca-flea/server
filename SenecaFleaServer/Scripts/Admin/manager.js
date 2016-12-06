var host = 'http://senecafleamarket.azurewebsites.net/api/AdminItem/';
//var host = http://localhost:35810/api/AdminItem/;

function fetchData(url) {

    var reqUrl = url;

    // create an xhr object
    var xhr = new XMLHttpRequest();

    // configure its handler
    xhr.onreadystatechange = function () {
        console.log("**onreadystatechange**\nxhr.readyState: " + xhr.readyState);
        console.log("**hr.status: " + xhr.status);

        // Get a reference to the DOM element to show status
        var e = document.querySelector('#content');
        var s = document.querySelector('#status');
        
        if (xhr.readyState === 4) {
            s.innerHTML = '';

            //console.log("** In readyState==4 >> hr.status: " + xhr.status);

            // request-response cycle has been completed, so continue        
            if (xhr.status === 200) {
                // request was successfully completed, and data was received, so continue

                // If you're interested in seeing the returned JSON...
                // Open the browser developer tools, and look in the JavaScript console
                //console.log(xhr.responseText);

                //console.log('reqUrl: ' + reqUrl);
                //console.log('url: ' + url);

                // Get the response data
                var responseData = JSON.parse(xhr.responseText);

                // ADD MORE CODE HERE - do something with the returned object/collection
                if (reqUrl.endsWith('AdminItem')) {
                    writeAdminItem(responseData);
                }
                else if (reqUrl.endsWith('AdminItem/Details')) {
                    writeAdminItemDetails(responseData);
                }
                else if (reqUrl.endsWith('AdminItem/Delete')) {
                    writeAdminItemDelete(responseData);
                }
                else {
                    // navigate to home page
                    window.location.href = 'AdminItem.html';
                }                
            }
            else
            {  
                e.innerHTML = "<p>Request was not successful<br>(" + xhr.statusText + ")</p>";
                s.innerHTML = '';
            }
        } else {
            
            s.innerHTML = 'Loading...';
        }
    }
    
    // configure the xhr object to fetch content    
    if (url == "AdminItem/Details")
    {
        var ItemId = location.search.split('ItemId=')[1];
        xhr.open('get', host + ItemId, true);
    }
    else if (url == "AdminItem/Delete")
    {
        var ItemId = location.search.split('ItemId=')[1];
        xhr.open('get', host + ItemId, true);
    }
    else if (url == "AdminItem/Delete/Confirm")
    {
        var ItemId = location.search.split('ItemId=')[1];
        xhr.open('delete', host + ItemId, true);
    }
    else
    {
        xhr.open('get', host, true);
    }
    
    // set the request headers 
    xhr.setRequestHeader('Accept', 'application/json');

    // fetch the token
    var token = sessionStorage.getItem('token');
    if (!token) {
        token = 'Empty';
    }
    xhr.setRequestHeader('Authorization', 'Bearer ' + token);

    // send the request
    xhr.send(null);
}

function writeAdminItem(results) {

    // Begin table
    var table = '<table class="table"><tr>'
        + '<th>Title</th>'
        + '<th>Description</th>'
        + '<th>Price</th>'
        + '<th>Status</th>'
        + '<th>SellerId</th>'
        + '<th></th>'
        + '<th></th>'
        + '</tr>';

    // Add rows
    for (var i = 0; i < results.length; i++) {

        table += '<tr><td>' + results[i].Title
            + '</td><td>' + results[i].Description
            + '</td><td>' + results[i].Price
            + '</td><td>' + results[i].Status
            + '</td><td>' + results[i].SellerId
            + '</td><td>' +
                        ' <input type="hidden" id="ItemId" name="ItemId" value="' + results[i].ItemId
                        + '"/><a href="AdminItemDetails.html?ItemId=' + results[i].ItemId + '">Details</a>'
            + '</td></tr>';

        console.log(table);
    }
    
    // End table
    table += '</table>';

    // Get a reference to the DOM element
    var e = document.querySelector('#content');

    // Append the table
    e.innerHTML = table;
}

function writeAdminItemDetails(results) {

    // Images
    var imgSrc= '';
    var len = results.Images.length;
    for (var i = 0; i < len; i++) {
        var bytes = results.Images[i].Photo;
        imgSrc += '<img src="data:image/png;base64,' + bytes + '" alt="Item image" /><br /><br />'
    }

    // Begin table
    var table = '<table class="table">';
    table += '<tr><td>Title</td><td>' + results.Title
        + '</td></tr><tr><td>Description</td><td>' + results.Description
        + '</td></tr><tr><td>Price</td><td>' + results.Price
        + '</td></tr><tr><td>Status</td><td>' + results.Status
        + '</td></tr><tr><td>SellerId</td><td>' + results.SellerId
        + '</td></tr><tr><td>Images</td><td>' + imgSrc
        + '</td></tr><tr><td><a href="AdminItemDelete.html?ItemId=' + results.ItemId
        + '">Delete</a>   |    <a href="AdminItem.html">Back to the list</a></td></tr>';


    // End table
    table += '</table>';

    // Get a reference to the DOM element
    var e = document.querySelector('#content');

    // Append the table
    e.innerHTML = table;
}

function writeAdminItemDelete(results) {

    // Images
    var imgSrc = '';
    var len = results.Images.length;
    for (var i = 0; i < len; i++) {
        var bytes = results.Images[i].Photo;
        imgSrc += '<img src="data:image/png;base64,' + bytes + '" alt="Item image" /><br /><br />'
    }

    // Begin table
    var status = '<p><font color="blue" size="4">Are you sure to delete this item?</font>'
                + '&nbsp;&nbsp;<a href="AdminItemDeleteConfirm.html?ItemId=' + results.ItemId
                + '">Yes</a>  |  <a href="AdminItem.html">No</a></p>';    

    var table = '<table class="table">';
    table += '<tr><td>Title</td><td>' + results.Title
        + '</td></tr><tr><td>Description</td><td>' + results.Description
        + '</td></tr><tr><td>Price</td><td>' + results.Price
        + '</td></tr><tr><td>Status</td><td>' + results.Status
        + '</td></tr><tr><td>SellerId</td><td>' + results.SellerId
        + '</td></tr><tr><td>Images</td><td>' + imgSrc
        + '</td></tr>';
    // End table
    table += '</table>';

    // Get a reference to the DOM element
    var s = document.querySelector('#status');
    var e = document.querySelector('#content');

    // Append the status
    s.innerHTML = status;
    // Append the table
    e.innerHTML = table;
}


function getAccessToken() {

    // get a reference to the login form
    var form = document.querySelector("#html5_login");

    // to monitor your progress, use the F12 developer tools debugger, and/or the Console
    console.log("Login in got from the form: ");
    console.log(form.Email.value);
    console.log(form.Password.value);

    // create an xhr object
    var xhr = new XMLHttpRequest();
    // configure its handler
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            // request-response cycle has been completed, so continue

            if (xhr.status === 200) {
                // request was successfully completed, and data was received, so continue

                // If you're interested in seeing the returned JSON...
                // Open the browser developer tools, and look in the JavaScript console
                console.log(xhr.responseText);

                // Get the response data
                var responseData = JSON.parse(xhr.responseText);
                // save (set) a value in the browser’s session storage
                sessionStorage.setItem('token', responseData.access_token);
                console.log("Returned token:" + responseData.access_token);

                // Need all?
                //sessionStorage.setItem('token_type', responseData.token_type);
                //sessionStorage.setItem('expires_in', responseData.expires_in);
                sessionStorage.setItem('userName', responseData.userName);
                //sessionStorage.setItem('issued', responseData.issued);                
                //sessionStorage.setItem('expires', responseData.expires);

                // navigate to home page
                window.location.href = 'AdminHome.html?UserName=' + responseData.userName;
            }
        }
    }

    // package the data that will be sent to the token endpoint; it's a simple string
    var reqdata = 'grant_type=password&username=' + form.Email.value + '&password=' + form.Password.value;

    // configure the xhr object to fetch content
    xhr.open('post', 'http://senecafleaia.azurewebsites.net/token', true);
    // set the request header
    xhr.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    // send the request
    xhr.send(reqdata);
}
