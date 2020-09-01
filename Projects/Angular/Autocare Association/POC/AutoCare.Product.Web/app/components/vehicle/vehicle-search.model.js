"use strict";
(function (ConfigurationSystems) {
    ConfigurationSystems[ConfigurationSystems["Select"] = 0] = "Select";
    ConfigurationSystems[ConfigurationSystems["Brake"] = 1] = "Brake";
    ConfigurationSystems[ConfigurationSystems["Bed"] = 2] = "Bed";
    ConfigurationSystems[ConfigurationSystems["Body"] = 3] = "Body";
    ConfigurationSystems[ConfigurationSystems["Engine"] = 4] = "Engine";
    ConfigurationSystems[ConfigurationSystems["Wheel"] = 5] = "Wheel";
    ConfigurationSystems[ConfigurationSystems["Drive"] = 6] = "Drive";
    ConfigurationSystems[ConfigurationSystems["MFR"] = 7] = "MFR";
})(exports.ConfigurationSystems || (exports.ConfigurationSystems = {}));
var ConfigurationSystems = exports.ConfigurationSystems;
(function (SearchType) {
    SearchType[SearchType["None"] = 0] = "None";
    SearchType[SearchType["GeneralSearch"] = 1] = "GeneralSearch";
    SearchType[SearchType["SearchByBaseVehicleId"] = 2] = "SearchByBaseVehicleId";
    SearchType[SearchType["SearchByVehicleId"] = 3] = "SearchByVehicleId";
})(exports.SearchType || (exports.SearchType = {}));
var SearchType = exports.SearchType;
(function (FacetType) {
    FacetType[FacetType["Region"] = 0] = "Region";
    FacetType[FacetType["VehicleType"] = 1] = "VehicleType";
    FacetType[FacetType["VehicleTypeGroup"] = 2] = "VehicleTypeGroup";
    FacetType[FacetType["Year"] = 3] = "Year";
    FacetType[FacetType["Make"] = 4] = "Make";
    FacetType[FacetType["Model"] = 5] = "Model";
    FacetType[FacetType["SubModel"] = 6] = "SubModel";
})(exports.FacetType || (exports.FacetType = {}));
var FacetType = exports.FacetType;
