"use strict";
exports.ConstantsWarehouse = {
    api: {
        make: '/api/makes',
        model: '/api/models',
        year: '/api/years',
        vehicleType: '/api/vehicleTypes',
        baseVehicle: '/api/baseVehicles',
        subModel: '/api/subModels',
        region: '/api/regions',
        source: '/api/sources',
        publicationStage: '/api/publicationStages',
        brakeConfig: '/api/brakeConfigs',
        brakeType: '/api/brakeTypes',
        bedType: '/api/bedTypes',
        bedLength: '/api/bedlengths',
        brakeSystem: '/api/brakeSystems',
        brakeABS: '/api/brakeABSes',
        downloadRequest: 'api/downloadRequest',
        vehicle: '/api/vehicles',
        vehicleToBrakeConfig: '/api/vehicleToBrakeConfigs',
        vehicleTypeGroup: '/api/vehicleTypeGroups',
        vehiclePendingChangeRequest: '/api/vehicles/pendingChangeRequests',
        vehicleSearch: '/api/vehicleSearch',
        refreshFacets: '/api/vehicleSearch/facets',
        vehicleToBrakeConfigSearch: '/api/vehicleToBrakeConfigSearch',
        vehicleToBrakeConfigSearchFacets: '/api/vehicleToBrakeConfigSearch/facets',
        changeRequestSearch: 'api/changeRequestSearch',
        likeStaging: 'api/likeStaging',
        bedConfig: 'api/bedConfigs',
        vehicleToBedConfig: 'api/vehicleToBedConfigs',
        vehicleToBedConfigSearch: '/api/vehicleToBedConfigSearch',
        vehicleToBedConfigSearchFacets: '/api/vehicleToBedConfigSearch/facets',
        vehicleToWheelBaseSearchFacets: '/api/vehicleToWheelBaseSearch/facets',
        vehicleToBodyConfig: 'api/vehicleToBodyConfigs',
        bodyStyleConfig: 'api/bodyStyleConfigs',
        bodyType: 'api/bodyTypes',
        wheelBase: 'api/wheelBase',
        vehicleToWheelBase: 'api/vehicleToWheelBases',
        vehicleToWheelBaseSearch: 'api/vehicleToWheelBaseSearch',
        bodyNumDoors: 'api/bodyNumDoors',
        vehicleToBodyStyleConfig: 'api/vehicleToBodyStyleConfigs',
        vehicleToBodyStyleConfigSearch: '/api/vehicleToBodyStyleConfigSearch',
        changeRequestSearchFacets: 'api/changeRequestSearch/facets',
        driveType: 'api/driveTypes',
        vehicleToDriveType: 'api/vehicleToDriveTypes',
        vehicleToDriveTypeSearch: 'api/vehicleToDriveTypeSearch',
        vehicleToDriveTypeSearchFacets: 'api/vehicleToDriveTypeSearch/facets',
        vehicleToMfrBodyCode: '/api/vehicleToMfrBodyCodes',
        vehicleToMfrBodyCodeSearch: '/api/vehicleToMfrBodyCodeSearch',
        vehicleToMfrBodyCodeSearchFacets: '/api/vehicleToMfrBodyCodeSearch/facets',
        mfrBodyCode: 'api/mfrBodyCodes',
        engineDesignation: 'api/engineDesignations',
        fuelDeliverySubType: 'api/fuelDeliverySubTypes',
        engineVersion: 'api/engineVersions',
        fuelType: 'api/fuelTypes',
        engineVin: 'api/engineVins'
    },
    changeRequestType: {
        add: "add",
        modify: "modify",
        remove: "remove",
    },
    notificationMessage: {
        //entity:  Entity Name; changeRequestType: add,modify,remove ; value: data
        success: function (entity, changeRequestType, value) {
            var body = "The status of your pending change requests can be found on the Dashboard.";
            var title = "Your request to " + changeRequestType + " the \"" + value + "\" " + entity + " will be reviewed.";
            return {
                body: body,
                title: title
            };
        },
        //entity:  Entity Name; changeRequestType: add,modify,remove ; value: data
        error: function (entity, changeRequestType, value) {
            var changeRequestTypeNoun = "";
            switch (changeRequestType) {
                case exports.ConstantsWarehouse.changeRequestType.add:
                    changeRequestTypeNoun = "addition";
                    break;
                case exports.ConstantsWarehouse.changeRequestType.remove:
                    changeRequestTypeNoun = "deletion";
                    break;
                case exports.ConstantsWarehouse.changeRequestType.modify:
                    changeRequestTypeNoun = "modification";
                    break;
                default:
                    changeRequestTypeNoun = "";
                    break;
            }
            var title = "Your request to " + changeRequestType + " the \"" + value + " \" " + entity + " cannot be submitted.";
            var body = "";
            if (changeRequestType == exports.ConstantsWarehouse.changeRequestType.add) {
                body = "That " + entity.toLowerCase() + " already exists or is currently being reviewed for " + changeRequestTypeNoun + ".";
            }
            else {
                body = "That " + entity.toLowerCase() + " already exists or is currently being reviewed for " + changeRequestTypeNoun + ".";
            }
            return {
                body: body,
                title: title
            };
        }
    },
    errorTitle: "Error",
    validationTitle: "Validation failed.",
    reviewSubmitSuccessTitle: "Your review was submitted.",
    reviewSubmitErrorTitle: "Your review couldn't be submitted.",
    reviewDeleteSuccessTitle: "Your change request was deleted.",
    reviewDeleteErrorTitle: "Your change request couldn't be deleted."
};
