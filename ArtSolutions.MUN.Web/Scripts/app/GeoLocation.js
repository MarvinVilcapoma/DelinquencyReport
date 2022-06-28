/// <reference path="../jquery-2.1.1.js" />

var GeoPosition;
var objMap;
var infowindowCurrentLocation;
var infowindowContent = document.getElementById('infowindow-content');
/* Map related function - start*/
function fnInitializeGeoLocation() {
    // If the browser supports the Geolocation API
    if (!navigator.geolocation) {
        showAlert("warning", browserSupportGeoLocationMsg);
    }
    else {
        var objPositionOptions = {
            enableHighAccuracy: true,
            timeout: 10 * 1000 // 10 seconds            
        };
        navigator.geolocation.getCurrentPosition(fnGeoLocationSuccess, fnGeoLocationError, objPositionOptions);

    }
}

function fnGeoLocationSuccess(position) {

    var geocoder = new google.maps.Geocoder;
    var updateTimeout = null;
    var objMarker;
    GeoPosition = position;

    var currLatLng = $("#Latitude").val() != "" && $("#Longitude").val() != "" ? new google.maps.LatLng($("#Latitude").val(), $("#Longitude").val()) : new google.maps.LatLng(position.coords.latitude, position.coords.longitude);
    fnSetLocation(currLatLng.lat(), currLatLng.lng());
    var objMapOption = {
        zoom: 17,
        center: currLatLng,
        streetViewControl: false,
        mapTypeControl: false,
        fullscreenControl: false,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    // Draw the map
    objMap = new google.maps.Map(document.getElementById("map"), objMapOption);

    //Set marker on current location
   infowindowCurrentLocation = new google.maps.InfoWindow();
    infowindowContent = document.getElementById('infowindow-content');

    geocoder.geocode({ 'location': currLatLng }, function (results, status) {
        infowindowCurrentLocation.close();
        objMap.setZoom(17);
        objMarker = new google.maps.Marker({
            position: currLatLng,
            map: objMap,
            anchorPoint: new google.maps.Point(0, -29)
        });
        infowindowCurrentLocation.setContent(results[0].formatted_address);
        infowindowCurrentLocation.open(objMap, objMarker);
    });

    // Create new marker on single click event on the map  
    if (isViewMode === "False" || !isViewMode) {
        google.maps.event.addListener(objMap, 'click', function (event) {
            if (infowindowCurrentLocation) {
                infowindowCurrentLocation.close();                
            }
            updateTimeout = setTimeout(function () {
                objMarker.setPosition(event.latLng);
                fnSetLocation(event.latLng.lat(), event.latLng.lng());
            }, 200);
        });
    }
    else
        return false;

    google.maps.event.addListener(objMap, 'dblclick', function (event) {
        clearTimeout(updateTimeout);
    });

    if (isViewMode === "False" || !isViewMode) {
        /*Auto complete and search functionality - start*/
        var card = document.getElementById('pac-card');
        var input = document.getElementById('pac-input');

        objMap.controls[google.maps.ControlPosition.TOP_RIGHT].push(card);

        var autocomplete = new google.maps.places.Autocomplete(input);
        
        infowindowContent = document.getElementById('infowindow-content');        

        autocomplete.addListener('place_changed', function () {            
            if (infowindowCurrentLocation) //close current location window
            {
                infowindowCurrentLocation.close();
                infowindowCurrentLocation.setContent(infowindowContent);
            }

            objMarker.setVisible(false);
            var objPlace = autocomplete.getPlace();
            if (!objPlace.geometry) {
                // User entered the name of a Place that was not suggested and
                // pressed the Enter key, or the Place Details request failed.
                showAlert("warning", noDetailsAvailableGeoLocationMsg + ": '" + objPlace.name + "'");
                return;
            }

            // If the place has a geometry, then present it on a map.
            if (objPlace.geometry.viewport) {
                objMap.fitBounds(objPlace.geometry.viewport);
            } else {
                objMap.setCenter(objPlace.geometry.location);
                objMap.setZoom(17);
            }
            objMarker.setPosition(objPlace.geometry.location);

            fnSetLocation(objPlace.geometry.location.lat(), objPlace.geometry.location.lng());

            objMarker.setVisible(true);

            var arrAddress = '';
            if (objPlace.address_components) {
                arrAddress = [
                    (objPlace.address_components[0] && objPlace.address_components[0].short_name || ''),
                    (objPlace.address_components[1] && objPlace.address_components[1].short_name || ''),
                    (objPlace.address_components[2] && objPlace.address_components[2].short_name || '')
                ].join(' ');
            }

            infowindowContent.children['place-icon'].src = objPlace.icon;
            infowindowContent.children['place-name'].textContent = objPlace.name;
            infowindowContent.children['place-address'].textContent = arrAddress;           
            infowindowCurrentLocation.open(objMap, objMarker);
        });
        /*Auto complete and search functionality - end*/
    }
}


function fnSetLocation(latitude, longitude) {
    $(".lblLatitude").html(latitude);
    $(".lblLongitude").html(longitude);
}

function fnGeoLocationError(positionError) {   
    //showAlert("warning", positionError.message);   //not needed this message after dont allow click
}

function UpdateMap() {
    var geocoder = new google.maps.Geocoder;
    var updateTimeout = null;
    var objMarker;   

    var currLatLng = $("#Latitude").val() != "" && $("#Longitude").val() != "" ? new google.maps.LatLng($("#Latitude").val(), $("#Longitude").val()) : new google.maps.LatLng(GeoPosition.coords.latitude, GeoPosition.coords.longitude);
    fnSetLocation(currLatLng.lat(), currLatLng.lng());    

    //Set marker on current location
    if (infowindowCurrentLocation) {
        infowindowCurrentLocation.close();      
    }

    geocoder.geocode({ 'location': currLatLng }, function (results, status) {
        infowindowCurrentLocation.close();
        objMap.setZoom(17);
        objMarker = new google.maps.Marker({
            position: currLatLng,
            map: objMap,
            anchorPoint: new google.maps.Point(0, -29)
        });
        infowindowCurrentLocation.setContent(results[0].formatted_address);
        infowindowCurrentLocation.open(objMap, objMarker);
    });   
}


/* Map related function - end*/



